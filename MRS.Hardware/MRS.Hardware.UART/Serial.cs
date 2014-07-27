using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.IO.Ports;

namespace MRS.Hardware.UART
{
    public class Serial
    {
        public static string[] GetPorts()
        {
            return SerialPort.GetPortNames();
        }

        private EDeviceState _state = EDeviceState.Unknown;

        public string PortName;

        public EDeviceState State
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

        public event OnReceiveHandler OnReceive;
        public event OnReceiveHandler OnError;

        public event OnReceiveByteHandler OnReceiveByte;

        public event OnStateChangeHandler OnStateChange;

        private Timer StateTimer;

        public UARTReadingState readState { get; private set; }

        public UARTWritingState writeState { get; private set; }

        private BackgroundWorker worker;

        private SerialPort device;

        public Serial(string portName)
            : this(portName, 38400, 1000)
        {

        }

        public Serial(string portName, int speed)
            : this(portName, speed, 1000)
        {

        }

        public Serial(string portName, int speed, int timeout)
            : this(portName, speed, Parity.None, 8, StopBits.One, timeout)
        {

        }

        public Serial(string portName, int speed, Parity parity, byte dataBits, StopBits stops,int timeout)
        {
            device = new SerialPort(portName, speed, parity, dataBits, stops);
            if (device.IsOpen)
            {
                State = EDeviceState.Busy;
                device.Close();
                State = EDeviceState.Unknown;
            }
            if (worker == null)
            {
                worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                worker.WorkerSupportsCancellation = true;
            }
            device.ReadTimeout = timeout;
            device.WriteTimeout = timeout;
            PortName = portName;
        }

        public string Connect()
        {
            if (worker != null)
            {
                State = EDeviceState.Busy;
                worker.CancelAsync();
                while (worker.IsBusy)
                {

                }
            }
            if (device.IsOpen)
            {
                device.Close();
                State = EDeviceState.Offline;
            }
            try
            {
                device.Open();
                State = EDeviceState.PortOpen;
            }
            catch (Exception e)
            {
                State = EDeviceState.Error;
                return e.Message;
            }
            worker.RunWorkerAsync();
            return "ok";
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            byte[] receivedBuf = null;
            int readingIndex = 0;
            State = EDeviceState.Online;
            while (!worker.CancellationPending && device.IsOpen)
            {
                try
                {
                    int bytesRead = device.BytesToRead;
                    if (bytesRead == 0 && readState == UARTReadingState.free)
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    //State = EDeviceState.Busy;
                    int b = device.ReadByte();
                    if (b < 0) continue;
                    if (readState <= UARTReadingState.free && (b == 01 || b == 02))
                    {
                        readState = UARTReadingState.reading;
                        int size = device.ReadByte();
                        if (size <= 0)
                        {
                            readState = UARTReadingState.free;
                            continue;
                        }
                        receivedBuf = new byte[size];
                        readingIndex = 0;
                        readState = UARTReadingState.readingSized;
                        continue;
                    }
                    if (readState == UARTReadingState.readingSized)
                    {
                        if (readingIndex >= receivedBuf.Length)
                        {
                            readState = UARTReadingState.free;
                            if (b == 4)
                            {
                                if (OnReceive != null)
                                {
                                    OnReceive(receivedBuf);
                                }
                            }
                            else
                            {
                                State = EDeviceState.Error;
                                if (OnError != null)
                                {
                                    State = EDeviceState.Online;
                                    OnError(receivedBuf);
                                }
                            }
                        }
                        else
                        {
                            receivedBuf[readingIndex] = (byte)b;
                            readingIndex++;
                        }
                    }
                }
                catch (Exception err)
                {

                }
            }
            State = EDeviceState.Offline;
        }

        public EDeviceState GetState()
        {
            return State;
        }

        public void Close()
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

        public bool SendUnsized(byte[] buffer)
        {
            return Send(buffer, 0, false);
        }

        public bool SendSized(byte[] buffer)
        {
            if (buffer.Length > 255)
            {
                return Send(buffer, 2, false);
            }
            else
            {
                return Send(buffer, 1, false);
            }
        }

        public bool Send(byte[] buffer, byte sizeByteLength, bool useCRC)
        {
            if (writeState > UARTWritingState.free) return false;
            if (buffer.Length > 255 || buffer.Length == 0) return false;
            if (!device.IsOpen)
            {
                return false;
            }
            writeState = UARTWritingState.writing;
            var bufAddLen = 2;
            bufAddLen += sizeByteLength;
            if (useCRC) bufAddLen += 1;
            var buf = new byte[buffer.Length + bufAddLen];
            buf[0] = sizeByteLength > 0 ? (byte)ASCII.SOH : (byte)ASCII.STX;
            if (sizeByteLength > 0)
            {
                buf[sizeByteLength] = (byte)buffer.Length;
            }
            buffer.CopyTo(buf, 1 + sizeByteLength);
            if (useCRC)
            {
                byte crc = 255;
                for (int i = 0; i < buffer.Length; i++)
                {
                    crc ^= buffer[i];
                }
                buf[buf.Length - 2] = crc;
            }
            buf[buf.Length - 1] = (byte)ASCII.EOT;
            uint bwrite = 0;
            device.Write(buf, 0, buf.Length);
            writeState = UARTWritingState.free;
            return true;
        }
        
        public void Send(string str)
        {
            Send(ASCIIEncoding.ASCII.GetBytes(str), 0, false);
        }
    }
}
