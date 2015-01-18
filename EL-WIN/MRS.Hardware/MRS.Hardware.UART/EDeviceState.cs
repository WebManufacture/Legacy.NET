namespace MRS.Hardware.UART
{
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
        reading
    }
}