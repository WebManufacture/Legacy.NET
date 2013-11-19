namespace MRS.Hardware.UART
{
    public delegate void OnReceiveHandler(byte[] data);
    public delegate void OnReceiveByteHandler(byte data);
    public delegate void OnStateChangeHandler(EDeviceState state);

    public enum EDeviceState
    {
        Unknown,
        PortOpen,
        Offline,
        Error,
        Busy,
        Online,
        Working
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