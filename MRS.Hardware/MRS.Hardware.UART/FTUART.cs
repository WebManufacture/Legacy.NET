using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using FTD2XX_NET;

namespace MRS.Hardware.UART
{
    public static class Device
    {
        private static EDeviceState _state = EDeviceState.Unknown;

        public static string PortName;

        public static EDeviceState State
        {
            get
            {
                return _state;
            }
            private set
            {
                if (value != _state)
                {
                    _state = value;
                    if (OnStateChange != null)
                    {
                        OnStateChange(value);
                    }
                }
            }
        }

        public static List<string> Errors = new List<string>();

        public static event OnReceiveHandler OnReceive;

        public static event OnReceiveByteHandler OnReceiveByte;

        public static event OnStateChangeHandler OnStateChange;

        private static Timer StateTimer;

        public static UARTReadingState readState { get; private set; }

        public static UARTWritingState writeState { get; private set; }

        private static FTDI device = new FTDI();

        private static BackgroundWorker worker;

        private static void log(string error)
        {
            Errors.Add(error);
        }

        private static void log(FTDI.FT_STATUS status, string func, string error)
        {
            Errors.Add(func + status.ToString() + error);
        }

        private static void log(FTDI.FT_STATUS status, string error)
        {
            Errors.Add(string.Format(error, status.ToString()));
        }

        private static void error(FTDI.FT_STATUS status, string error)
        {
            error = string.Format(error, status.ToString());
            Errors.Add(error);
            throw new Exception(error);
        }

        public static FTDI.FT_DEVICE_INFO_NODE[] GetDevicesList()
        {
            uint devCount = 0;
            var status = device.GetNumberOfDevices(ref devCount);
            if (status != FTDI.FT_STATUS.FT_OK)
            {
                return null;
            }
            FTDI.FT_DEVICE_INFO_NODE[] devices = new FTDI.FT_DEVICE_INFO_NODE[devCount];
            status = device.GetDeviceList(devices);
            if (status == FTDI.FT_STATUS.FT_OK)
            {
                return devices;
            }
            return null;
        }

        public static bool Init(uint speed, uint timeout)
        {
            var devs = GetDevicesList();
            foreach (var ftDeviceInfoNode in devs)
            {
                if (ftDeviceInfoNode.Type == FTDI.FT_DEVICE.FT_DEVICE_232R)
                {
                    return Init(ftDeviceInfoNode.SerialNumber, speed, timeout);
                }
            }
            return false;
        }

        public static bool Init(string serial, uint speed, uint timeout)
        {
            if (device.IsOpen)
            {
                State = EDeviceState.Busy;
                device.Close();
                State = EDeviceState.Unknown;
            }
            var status = device.OpenBySerialNumber(serial);
            if (status == FTDI.FT_STATUS.FT_OK)
            {
                _state = EDeviceState.PortOpen;
            }
            else
            {
                error(status, "Init device with SN: " + serial + " error {0}");
            }
            status = device.GetCOMPort(out PortName);
            if (status != FTDI.FT_STATUS.FT_OK || PortName == "")
            {
                error(status, "Init device with SN: " + serial + " No Port Specified {0}");
            }
            if (worker != null)
            {
                State = EDeviceState.Busy;
                worker.CancelAsync();
                while (worker.IsBusy)
                {

                }
            }
            if (worker == null)
            {
                worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                worker.WorkerSupportsCancellation = true;
            }
            device.SetTimeouts(timeout, timeout);
            device.SetBaudRate(speed);
            device.SetDataCharacteristics(8, 1, FTDI.FT_PARITY.FT_PARITY_ODD);
            // port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            worker.RunWorkerAsync();
            return true;
        }

        static void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            byte[] receivedBuf = null;
            int readingIndex = 0;
            byte[] buf = new byte[1];
            while (!worker.CancellationPending && device.IsOpen)
            {
                uint bytesRead = 0;
                var status = device.GetRxBytesAvailable(ref bytesRead);
                if (status != FTDI.FT_STATUS.FT_OK)
                {
                    log(status, "GetRxBytesAvailable: {0}");
                    break;
                }
                if (bytesRead == 0)
                {
                    Thread.Sleep(100);
                    continue;
                }
                if (OnReceive != null)
                {
                    byte b = 0;
                    status = device.Read(buf, 1, ref bytesRead);
                    if (status != FTDI.FT_STATUS.FT_OK)
                    {
                        log(status, "Read: {0}");
                        break;
                    }
                    b = buf[0];
                    if (OnReceiveByte != null)
                    {
                        OnReceiveByte(b);
                    }
                    if (readState == UARTReadingState.free && b == 01)
                    {
                        readState = UARTReadingState.reading;
                        continue;
                    }
                    if (readState == UARTReadingState.reading)
                    {
                        receivedBuf = new byte[b];
                        readingIndex = 0;
                        readState = UARTReadingState.readingSized;
                        continue;
                    }
                    if (readState == UARTReadingState.readingSized)
                    {
                        if (receivedBuf == null)
                        {
                            log("Received buf is null!");
                            break;
                        }
                        if (readingIndex >= receivedBuf.Length)
                        {
                            if (b == 4)
                            {
                                var bytes = new byte[receivedBuf.Length / 2];
                                for (int i = 0; i < bytes.Length; i++)
                                {
                                    bytes[i] = (byte)((receivedBuf[i * 2] & (byte)0x0F) * 16 + (receivedBuf[i * 2 + 1] & (byte)0x0F));
                                }
                                OnReceive(bytes);
                            }
                            else
                            {
                                State = EDeviceState.Error;
                            }
                            readState = UARTReadingState.free;
                        }
                        else
                        {
                            receivedBuf[readingIndex] = b;
                            readingIndex++;
                        }
                    }
                }
            }
        }

        public static EDeviceState GetState()
        {
            if (writeState > UARTWritingState.free || readState > UARTReadingState.free)
            {
                State = EDeviceState.Working;
                return State;
            }
            try
            {
                if (device.IsOpen)
                {
                    FTDI.FT_DEVICE dev = FTDI.FT_DEVICE.FT_DEVICE_UNKNOWN;
                    if (device.GetDeviceType(ref dev) != FTDI.FT_STATUS.FT_OK)
                    {
                        State = EDeviceState.Unknown;
                        return State;
                    }
                    if (dev == FTDI.FT_DEVICE.FT_DEVICE_UNKNOWN)
                    {
                        State = EDeviceState.Error;
                    }
                    else
                    {
                        State = EDeviceState.Online;
                    }
                }
                else
                {
                    State = EDeviceState.Offline;
                }
            }
            catch (Exception e)
            {
                State = EDeviceState.Error;
            }
            return State;
        }

        public static void Close()
        {
            if (worker != null && !worker.CancellationPending)
            {
                worker.CancelAsync();
            }
            if (StateTimer != null)
            {
                StateTimer.Dispose();
                StateTimer = null;
            }
            if (device.IsOpen)
            {
                device.Close();
                State = EDeviceState.Offline;
            }
        }

        public static bool Send(byte[] buffer)
        {
            if (writeState > UARTWritingState.free) return false;
            if (buffer.Length > 255 || buffer.Length == 0) return false;
            if (!device.IsOpen)
            {
                return false;
            }
            writeState = UARTWritingState.writing;
            var buf = new byte[buffer.Length + 4];
            byte crc = 255;
            buf[0] = 0;
            buf[1] = (byte)buffer.Length;
            buffer.CopyTo(buf, 2);
            for (int i = 0; i < buffer.Length; i++)
            {
                crc ^= buffer[i];
            }
            buf[buf.Length - 2] = crc;
            buf[buf.Length - 1] = (byte)ASCII.EOT;
            uint bwrite = 0;
            var status = device.Write(buf, buf.Length, ref bwrite);
            if (status != FTDI.FT_STATUS.FT_OK)
            {
                log(status, "Write: {0}");
                return false;
            }
            writeState = UARTWritingState.free;
            return true;
        }

        public static void Send(byte command)
        {
            Send(new byte[] { command });
        }

        public static void Send(string str)
        {
            Send(ASCIIEncoding.ASCII.GetBytes(str));
        }
    }
}
