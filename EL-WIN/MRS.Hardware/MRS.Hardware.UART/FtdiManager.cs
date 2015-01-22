using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;
using FTDIDriver = FTD2XX_NET.FTDI;

namespace MRS.Hardware.UART
{
    public class FtdiManager : SerialManager
    {
        private FTDIDriver device = new FTDIDriver();

        private BackgroundWorker worker;

        public List<string> Errors = new List<string>();

        public string LastError
        {
            get
            {
                if (Errors.Count > 0)
                {
                    return Errors.Last();
                }
                return "";
            }
        }


        public void Error(string message, FTDIDriver.FT_STATUS status)
        {
            callError(new Exception(message + " status: " + status));
        }


        public FtdiManager()
        {

        }

        public FtdiManager(uint index)
        {
            device = new FTDIDriver();
            FTDIDriver.FT_STATUS ftStatus = device.OpenByIndex(index);
            CheckError("Failed to open device 0", ftStatus);
            if (!device.IsOpen)
            {
                Error("Failed to open device. Device not opened!", ftStatus);
            }
        }

        public bool CheckError(string message, FTDIDriver.FT_STATUS status)
        {
            if (status != FTDIDriver.FT_STATUS.FT_OK)
            {
               // Error(message, status);
                return true;
            }
            return false;
        }
        private void log(string error)
        {
            Errors.Add(error);
        }

        private void log(FTDIDriver.FT_STATUS status, string func, string error)
        {
            Errors.Add(func + status.ToString() + error);
        }

        private void log(FTDIDriver.FT_STATUS status, string error)
        {
            Errors.Add(string.Format(error, status.ToString()));
        }

        private void error(FTDIDriver.FT_STATUS status, string error)
        {
            error = string.Format(error, status.ToString());
            Errors.Add(error);
            throw new Exception(error);
        }

        public static FTDIDriver.FT_DEVICE_INFO_NODE[] GetDevicesList()
        {
            var device = new FTDIDriver();
            uint devCount = 0;
            var status = device.GetNumberOfDevices(ref devCount);
            if (status != FTDIDriver.FT_STATUS.FT_OK)
            {
                return null;
            }
            FTDIDriver.FT_DEVICE_INFO_NODE[] devices = new FTDIDriver.FT_DEVICE_INFO_NODE[devCount];
            status = device.GetDeviceList(devices);
            if (status == FTDIDriver.FT_STATUS.FT_OK)
            {
                return devices;
            }
            return null;
        }


        public static string GetDeviceInfo()
        {
            FTDIDriver chip = new FTDIDriver();
            string info = "";
            uint deviceCount = 0;
            FTDIDriver.FT_STATUS status = chip.GetNumberOfDevices(ref deviceCount);
            if (status != FTDIDriver.FT_STATUS.FT_OK)
            {
                info = "GetDeviceInfo: Device Error! " + status;
                return info;
            }
            if (deviceCount > 0)
            {
                // Allocate storage for device info list
                FTDIDriver.FT_DEVICE_INFO_NODE[] ftdiDeviceList = new FTDIDriver.FT_DEVICE_INFO_NODE[deviceCount];

                // Populate our device list
                status = chip.GetDeviceList(ftdiDeviceList);
                if (status != FTDIDriver.FT_STATUS.FT_OK)
                {
                    info = "GetDeviceInfo: Error getting list!" + status;
                    return info;
                }
                for (int i = 0; i < deviceCount; i++)
                {
                    info += "Device: " + ftdiDeviceList[i].ID + "\n";
                    info += "Type: " + ftdiDeviceList[i].Type + "\n";
                    info += "SerialNumber: " + ftdiDeviceList[i].SerialNumber + "\n";
                    info += "Description: " + ftdiDeviceList[i].Description + "\n";
                    info += "\n";
                }
                return info;
            }
            return "";
        }


        /*
        public FtdiManager(uint speed, uint timeout) : this(ftDeviceInfoNode.SerialNumber, speed, 1000)
        {
        }

        public FtdiManager(string serial, uint speed, uint timeout)
        {
            if (device.IsOpen)
            {
                State = EDeviceState.Busy;
                device.Close();
                State = EDeviceState.Unknown;
            }
            var status = device.OpenBySerialNumber(serial);
            if (status == FTDIDriver.FT_STATUS.FT_OK)
            {
                _state = EDeviceState.PortOpen;
            }
            else
            {
                error(status, "Init device with SN: " + serial + " error {0}");
            }
            status = device.GetCOMPort(out PortName);
            if (status != FTDIDriver.FT_STATUS.FT_OK || PortName == "")
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
            device.SetDataCharacteristics(8, 1, FTDIDriver.FT_PARITY.FT_PARITY_ODD);
            // port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            byte[] receivedBuf = null;
            int readingIndex = 0;
            byte[] buf = new byte[1];
            while (!worker.CancellationPending && device.IsOpen)
            {
                uint bytesRead = 0;
                var status = device.GetRxBytesAvailable(ref bytesRead);
                if (status != FTDIDriver.FT_STATUS.FT_OK)
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
                    if (status != FTDIDriver.FT_STATUS.FT_OK)
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
        */
        public EDeviceState GetState()
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
                    FTDIDriver.FT_DEVICE dev = FTDIDriver.FT_DEVICE.FT_DEVICE_UNKNOWN;
                    if (device.GetDeviceType(ref dev) != FTDIDriver.FT_STATUS.FT_OK)
                    {
                        State = EDeviceState.Unknown;
                        return State;
                    }
                    if (dev == FTDIDriver.FT_DEVICE.FT_DEVICE_UNKNOWN)
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

        public void ResetChip(){
            device.ResetPort();
            device.ResetDevice();
            device.Rescan();
        }


        public void Close()
        {
            if (worker != null && !worker.CancellationPending)
            {
                worker.CancelAsync();
            }/*
            if (StateTimer != null)
            {
                StateTimer.Dispose();
                StateTimer = null;
            
              }*/
            if (device.IsOpen)
            {
                device.Close();
                State = EDeviceState.Offline;
            }
        }


        public virtual byte ReadByte()
        {
            byte[] buffer = new byte[1];
            UInt32 numBytesReader = 0;
            FTDIDriver.FT_STATUS ftStatus = device.Read(buffer, 1, ref numBytesReader);
            if (!CheckError("Failed to read from device", ftStatus))
            {
                //callReceiveByte(buffer[0]);
            }
            return buffer[0];
        }

        public virtual byte[] ReadData(uint bytesCount)
        {
            byte[] buffer = new byte[bytesCount];
            UInt32 numBytesReader = 0;
            FTDIDriver.FT_STATUS ftStatus = device.Read(buffer, bytesCount, ref numBytesReader);
            if (!CheckError("Failed to read from device", ftStatus))
            {
                //callReceive(buffer);
            }
            return buffer;
        }

        public virtual string ReadString(uint bytesCount)
        {
            string buffer = "";
            UInt32 numBytesReader = 0;
            FTDIDriver.FT_STATUS ftStatus = device.Read(out buffer, bytesCount, ref numBytesReader);
            if (!CheckError("Failed to read from device", ftStatus))
            {
               /* if (OnReceive != null)
                {
                    OnReceive(buffer);
                }*/
            }
            return buffer;
        }
    }
}
