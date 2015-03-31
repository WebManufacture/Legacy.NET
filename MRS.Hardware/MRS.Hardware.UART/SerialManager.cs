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
    public class SerialManager
    {
        public static string[] GetPorts()
        {
            return SerialPort.GetPortNames();
        }

        private EDeviceState _state = EDeviceState.Unknown;

        public string PortName{
            get{
                return device.PortName;
            }
        }

        public EDeviceState State
        {
            get
            {
                return _state;
            }
            protected set
            {
                if (value != _state)
                {
                    _state = value;
                    if (OnStateChange != null)
                    {
                        OnStateChange(value, this);
                    }
                }
            }
        }


        private List<string> errors = new List<string>();

        public List<string> Errors
        {
            get
            {
                return errors;
            }
        }


        public string LastError
        {
            get
            {
                if (errors.Count > 0) return errors[errors.Count - 1];
                return "";
            }
        }

        public event OnDataHandler OnReceive;
        public event OnDataHandler OnSend;
        public event OnReceiveByteHandler OnReceiveByte;
        public event OnErrorHandler OnError;
        public event OnStateChangeHandler OnStateChange;

        private Timer StateTimer;

        public UARTReadingState readState { get; private set; }

        public UARTWritingState writeState { get; private set; }

        protected BackgroundWorker worker;

        protected SerialPort device;
        protected bool WorkerInProgress;


        protected virtual void OnLoad(){

        }

        public SerialManager()
        {
            OnLoad();
        }


        public SerialPort Device
        {
            get
            {
                return device;
            }
        }

        public SerialManager(SerialPort port) : this()
        {
            device = port;
            if (device.IsOpen)
            {
                State = EDeviceState.PortOpen;
            }
            device.ReadTimeout = 1000;
            device.WriteTimeout = 1000;
            port.Disposed += port_Disposed;
            device.ReadBufferSize = 1024;
        }


        public SerialManager(SerialConfig config) : this(new SerialPort(config.PortName, (int)config.Speed, config.Parity, config.DataBits, config.StopBits))
        {

        }

        public SerialManager(string portName)
            : this(portName, 38400, 1000)
        {

        }

        public SerialManager(string portName, int speed)
            : this(portName, speed, 1000)
        {

        }

        public SerialManager(string portName, int speed, int timeout)
            : this(portName, speed, Parity.None, 8, StopBits.One, timeout)
        {

        }

        public SerialManager(string portName, int speed, Parity parity, byte dataBits, StopBits stops, int timeout)
            : this(new SerialPort(portName, speed, parity, dataBits, stops))
        {
            device.ReadTimeout = timeout;
            device.WriteTimeout = timeout;
        }

        public virtual bool Connect()
        {
            var res = connect();
            if (res != "ok") return false;
            if (worker != null) throw new Exception("WORKED!");
            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += new DoWorkEventHandler(worker_DoSimpleWork);
            worker.RunWorkerAsync();
            return true;
        }

        public bool IsOpened
        {
            get
            {
                return device.IsOpen;
            }
        }


        protected string connect()
        {
            if (device == null) return "uninitialized";
            if (!device.IsOpen)
            {
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
            }
            return "ok";
        }
                
        protected virtual bool callError(Exception e)
        {
            errors.Add(e.Message);// + " status: " + status);
            if (OnError != null)
            {
                OnError(e, this);
                return true;
            }
            return false;
        }

        private void callReceiveByte(byte data)
        {
            if (OnReceiveByte != null)
            {
                OnReceiveByte(data, this);
            }
        }

        public bool PreventSendingEvent { get; set; }

        protected virtual bool callSend(byte[] data)
        {
            if (!PreventSendingEvent && OnSend != null)
            {
                OnSend(data, this);
                return true;
            }
            return false;
        }

        protected virtual bool callReceive(byte[] data)
        {
            if (OnReceive != null)
            {
                OnReceive(data, this);
                return true;
            }
            return false;
        }

        void worker_DoSimpleWork(object sender, DoWorkEventArgs e)
        {
            if (device == null) return;
            var worker = (BackgroundWorker)sender;
            WorkerInProgress = true;
            while (!worker.CancellationPending && device.IsOpen)
            {
                try
                {
                    int bytesRead = 0;
                    bytesRead = device.BytesToRead;
                    if (bytesRead == 0 && readState == UARTReadingState.free)
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    try
                    {
                        var buf = new byte[bytesRead];
                        if (worker.CancellationPending || !device.IsOpen) break;
                        lock (device)
                        {
                            bytesRead = device.Read(buf, 0, bytesRead);
                        }
                        if (OnReceiveByte != null)
                        {
                            for (var i = 0; i < bytesRead; i++)
                            {
                                callReceiveByte(buf[i]);
                            }
                        }
                        else
                        {
                            callReceive(buf);
                        }
                    }
                    catch (TimeoutException)
                    {

                    }
                }
                catch (Exception err)
                {
                    callError(err);
                }
            }
            WorkerInProgress = false;
            e.Cancel = true;
        }

        public bool HasData()
        {
            if (device == null) return false;
            return device.BytesToRead > 0;
        }

        public uint BytesAvailable()
        {
            if (device == null) return 0;
            return (uint)device.BytesToRead;
        }

        public EDeviceState GetState()
        {
            return State;
        }

        protected void setWriteState(UARTWritingState state){
            writeState = state;
        }

        public virtual byte[] ReadData()
        {
            return ReadData(device.BytesToRead);
        }

        public virtual byte[] ReadData(uint bytes)
        {
            return ReadData((int)bytes);
        }

        public virtual byte[] ReadData(int bytes)
        {
            if (device == null) return null;
            readState = UARTReadingState.reading;
            var data = new byte[bytes];
            try
            {
                device.Read(data, 0, bytes);
            }
            catch (TimeoutException)
            {
                return null;
            }
            readState = UARTReadingState.free;
            return data;
        }

        public virtual byte ReadByte()
        {
            if (device == null) return 0;
            readState = UARTReadingState.reading;
            int res = -1;
            do
            {
                res = device.ReadByte();
            }
            while (res < 0);
            readState = UARTReadingState.free;
            return (byte)res;
        }

        public virtual string ReadString()
        {
            return Encoding.ASCII.GetString(ReadData());
        }
        
        void port_Disposed(object sender, EventArgs e)
        {
            if (State != EDeviceState.Offline)
            {
                device = null;
                Close();
            }
        }

        public virtual void Close()
        {
            if (worker != null && !worker.CancellationPending)
            {
                State = EDeviceState.Busy;
                worker.CancelAsync();
                while (WorkerInProgress)
                {

                }
                worker.Dispose();
                worker = null;
            }
            if (StateTimer != null)
            {
                StateTimer.Dispose();
                StateTimer = null;
            }
            State = EDeviceState.Offline;
            if (device != null && device.IsOpen)
            {
                device.Close();
            }            
        }

        public virtual bool Send(byte[] buf)
        {
            if (device == null) return false;
            if (writeState > UARTWritingState.free) return false;
            if (buf.Length > 255 || buf.Length == 0) return false;
            if (!device.IsOpen)
            {
                return false;
            }
            writeState = UARTWritingState.writing;
            device.Write(buf, 0, buf.Length);
            callSend(buf);
            writeState = UARTWritingState.free;
            return true;
        }

        public virtual bool Send(byte data)
        {
            if (device == null) return false;
            if (writeState > UARTWritingState.free) return false;
            if (!device.IsOpen)
            {
                return false;
            }
            writeState = UARTWritingState.writing;
            device.Write(new byte[] { data }, 0, 1);
            callSend(new byte[] { data });
            writeState = UARTWritingState.free;
            return true;
        }

        public virtual bool Send(string str)
        {
            return Send(ASCIIEncoding.ASCII.GetBytes(str));
        }

    }
}
