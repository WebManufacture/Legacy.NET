﻿using System;
namespace MRS.Hardware.UART
{

    public delegate void OnDataHandler(byte[] data, SerialManager manager);
    public delegate void OnReceiveByteHandler(byte data, SerialManager manager);
    public delegate void OnErrorHandler(Exception e, SerialManager manager);
    public delegate void OnStateChangeHandler(EDeviceState state, SerialManager manager);


    public enum EDeviceState
    {
        Unknown,        
        Error,
        Offline,
        PortOpen,
        Online,
        Working,
        Busy
        
    }

    public enum UARTWritingState
    {
        free,
        writing
    }
    public enum UARTReadingState
    {
        free,
        reading
    }
}