namespace MRS.Hardware.UART
{
    public delegate void OnReceiveHandler(byte[] data);
    public delegate void OnReceiveByteHandler(byte data);
    public delegate void OnStateChangeHandler(EDeviceState state);

    public enum EDeviceState
    {
        Unknown,
        PortOpen,        
        Error,
        Busy,
        Online,
        Working,
        Offline
    }

    public enum UARTWritingState
    {
        free,
        writing
    }
    public enum UARTReadingState
    {
        free,
        reading,
        readingSized
    }
}