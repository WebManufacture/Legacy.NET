using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using MRS.Hardware.Server;
using Newtonsoft.Json;
using MRS.Hardware.UART;

namespace MRS.Hardware.Server
{
    public delegate void MessageHandler(MotorState state);
    public delegate void ProgramStateHandler(CncProgramState state);
    public delegate void CommandHandler(MotorCommand state);

    public class MotorState
    {
        public byte command;
        public int x;
        public int y;
        public int z;
        public int xLimit;
        public int yLimit;
        public int zLimit;
        public byte state;
        public DateTime date = DateTime.Now;
        public int line;

        public CommandType Command
        {
            get { return (CommandType)command; }
        }

        public override string ToString()
        {
            var str = JsonConvert.SerializeObject(this);
                        return str.Remove(str.Length - 1) + ", \"type\" : \"state\"}";
        }
    }

    public enum CommandType : byte
    {
        Null = 0,
        Go = 1,
        Rebase = 2,
        Stop = 3,
        State = 4,
        Pause = 6,
        Resume = 7,
        Error = 10,
        Spindle = 20,
        Contact = 30        
    }

    public class MotorCommand
    {
        protected byte command;
        public int Line = 0;
        public ushort Speed = 0;
        public bool Sended;

        public CommandType Command
        {
            get { return (CommandType)command; }
            set { command = (byte)value; }
        }

        public MotorCommand()
        {
            command = 0;
        }

        public MotorCommand(byte command)
        {
            this.command = command;
        }

        public MotorCommand(CommandType command)
        {
            this.Command = command;
        }

        public override string ToString()
        {
            var str = JsonConvert.SerializeObject(this);
            return str.Remove(str.Length - 1) + ", \"type\" : \"out-command\"}";
        }

        public static MotorCommand StateCommand
        {
            get
            {
                return new MotorCommand(CommandType.State);
            }
        }

        public static MotorCommand StopCommand
        {
            get
            {
                return new MotorCommand(CommandType.Stop);
            }
        }

        public static MotorCommand GetCommand(byte command)
        {
            return new MotorCommand(command);
        }
    }

    public class CoordMotorCommand : MotorCommand
    {
        public int X;
        public int Y;
        public int Z;

        public CoordMotorCommand()
            : base(CommandType.Go)
        {
        }

        public CoordMotorCommand(CommandType command)
            : base(command)
        {
        }

        public static CoordMotorCommand Rebase(int x, int y, int z)
        {
            var command = new CoordMotorCommand(CommandType.Rebase);
            command.X = x;
            command.Y = y;
            command.Z = z;
            return command;
        }

        public static CoordMotorCommand Go(int x, int y, int z)
        {
            var command = new CoordMotorCommand(CommandType.Go);
            command.X = x;
            command.Y = y;
            command.Z = z;
            return command;
        }

        public static bool IsCoordCommand(CommandType command)
        {
            return command == CommandType.Go || command == CommandType.Rebase;
        }

        public static bool IsCoordCommand(byte command)
        {
            return IsCoordCommand((CommandType)command);
        }
    }

    public enum CncProgramState : byte
    {
        NotStarted,
        Running,
        Paused,
        Completed,
        Aborted
    }

    public class CncProgram
    {        
        public static List<MotorCommand> Commands;
        public static int CurrentLine;
        public static bool DebugMode = false;

        public static event ProgramStateHandler OnStateChange;
        public static event CommandHandler OnCommand;

        private static CncProgramState _state;
        public static CncProgramState State
        {
            get { return _state; }
            set
            {
                _state = value;
                if (OnStateChange != null)
                {
                    OnStateChange(value);
                }
            }
        }

        public static  bool Exists
        {
            get { return Commands.Count > 0; }
        }

        public static bool InProgress
        {
            get { return _state > CncProgramState.NotStarted && _state < CncProgramState.Completed; }
        }


        static CncProgram()
        {
            CurrentLine = 0;
            Commands = new List<MotorCommand>();
            _state = CncProgramState.NotStarted;
            CncController.OnMessage += CncControllerOnOnMessage;
        }

        private static void CncControllerOnOnMessage(MotorState obj)
        {
            if (obj.command == 3)
            {
                CncProgram.Stop();
                return;
            }
            if (CncProgram.Exists && CncProgram.InProgress && obj.line > 0)
            {
                if (obj.state == 1)
                {
                    CncProgram.Prepare(obj);
                }
                if (obj.state == 2)
                {
                    CncProgram.Next(obj);
                }
            }
        }

        public static void NewProgram(MotorCommand[] commands)
        {
            CurrentLine = 0;
            Commands = new List<MotorCommand>(commands);
            State = CncProgramState.NotStarted;
        }

        public static void NewProgram(List<MotorCommand> commands)
        {
            CurrentLine = 0;
            Commands = commands;
            State = CncProgramState.NotStarted;
        }

        public static  void Stop()
        {
            State = CncProgramState.Aborted;
        }

        public static void Run()
        {
            CurrentLine = 1;
            if (DebugMode)
            {
                State = CncProgramState.Paused;
            }
            else
            {
                State = CncProgramState.Running;
                Next(null);
            }
        }

        public static void Pause()
        {
            if (State < CncProgramState.Completed)
            {
                State = CncProgramState.Paused;
            }
        }

        public static void Resume()
        {
            if (State < CncProgramState.Completed)
            {
                State = CncProgramState.Running;
            }
        }

        public static bool Next(MotorState state)
        {
            if (State > CncProgramState.Running)
            {
                return false;
            }
            if (CurrentLine > 0 && CurrentLine <= Commands.Count)
            {
                /*                if (state.line > 0)
                                {
                                    CurrentLine = state.line + 1;
                                }*/
                var command = Commands[CurrentLine - 1];
                command.Line = CurrentLine;
                CurrentLine++;
                if (command.Command == CommandType.Pause)
                {
                    Pause();
                }
                if (command.Command == CommandType.Resume)
                {
                    Resume();
                }
                if (!command.Sended)
                {
                    CncController.SendCommand(command);
                }
                if (OnCommand != null)
                {
                    try
                    {
                        OnCommand(command);
                    }
                    catch (Exception)
                    {

                    }
                }
                if (DebugMode) Pause();
                return true;
            }
            if (State == CncProgramState.Running)
            {
                State = CncProgramState.Completed;
            }
            return false;
        }

        public static bool Prepare(MotorState state)
        {
            if (State > CncProgramState.Running)
            {
                return false;
            }
            if (CurrentLine > 0 && CurrentLine < Commands.Count) // CurrentLine 1...N, тут нужно иметь возможность выбрать еще 1 комманду
            {
                var command = Commands[CurrentLine - 1];
                if (command.Sended) return true;
                command.Line = CurrentLine;
                if (command.Command == CommandType.Go) //Отсылаем только комманды мотору
                {
                    CncController.SendCommand(command);
                }
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }

    public class CncController
    {
        public static MotorState LastState = null;
        public static MotorCommand LastCommand = null;
        public static bool SpindleState = false;
        public static event MessageHandler OnMessage;
        public static event CommandHandler OnCommand;
        public static int X;
        public static int Y;
        public static int Z;
        public static int XLim;
        public static int YLim;
        public static int ZLim;
        public static byte State;
        public static byte StateA;
        public static byte StateB;

        public static bool InMove
        {
            get { return LastState != null && LastState.state == 1; }
        }

        public static bool Ready
        {
            get
            {
                return Uart.writeState == UARTWritingState.free;
            }
        }

        public static bool IsOpen
        {
            get
            {
                return Uart.State > EDeviceState.Error;
            }
        }

        private static string cfgPath;

        protected static bool initialized = false;


        private static int loadInt(byte[] arr, int index)
        {
            return (int)(arr[index] * 16777216 + arr[index + 1] * 65536 + arr[index + 2] * 256 + arr[index + 3]);
        }

        private static int saveInt(byte[] arr, int index, int value)
        {
            arr[index] = (byte)(value >> 24);
            arr[index + 1] = (byte)(value >> 16);
            arr[index + 2] = (byte)(value >> 8);
            arr[index + 3] = (byte)(value);
            return value;
        }

        public static void ProcessMessage(byte[] data)
        {
            MotorState obj = new MotorState();
            if (data.Length >= 32)
            {
                obj.command = data[0];
                obj.state = State = data[1];
                obj.line = loadInt(data, 2);
                obj.x = X = loadInt(data, 6);
                obj.y = Y = loadInt(data, 10);
                obj.z = Z = loadInt(data, 14);
                obj.xLimit = XLim = loadInt(data, 18);
                obj.yLimit = YLim = loadInt(data, 22);
                obj.zLimit = ZLim = loadInt(data, 26);
                StateA = data[30];
                StateB = data[31];
                /*if (!initialized && LastState != null)
                {
                    if (LastState.x != obj.x || LastState.y != obj.y || LastState.z != obj.z)
                    {
                        Uart.SendCommand(MotorCommand.Rebase(LastState.x, LastState.y, LastState.z));
                    }
                }*/
                if (OnMessage != null)
                {
                    OnMessage(obj);
                }
                LastState = obj;
                /*StreamWriter writer = new StreamWriter(cfgPath + "\\config.json", false, Encoding.UTF8);
                writer.WriteLine(Uart.PortName);
                writer.WriteLine(JsonConvert.SerializeObject(obj));
                writer.Close();*/
                initialized = true;
                
            }
            else
            {
                if (data.Length == 1 && (data[0] >= 50 || data[0] < 100))
                {
                    if (LastState != null)
                    {
                        //SendCoordCommand(CoordMotorCommand.Rebase(LastState.x, LastState.y, LastState.z));
                    }
                }
            }
        }

        public static bool SendCoordCommand(CoordMotorCommand data)
        {
            if (data == null || data.Command <= CommandType.Null)
            {
                return false;
            }
            LastCommand = data;
            byte[] bytes = new byte[20];
            bytes[0] = (byte)data.Command;
            bytes[1] = (byte)(data.Speed / 256);
            bytes[2] = (byte)(data.Speed % 256);
            saveInt(bytes, 3, data.Line);
            saveInt(bytes, 7, data.X);
            saveInt(bytes, 11, data.Y);
            saveInt(bytes, 15, data.Z);
            Uart.Send(bytes);
            data.Sended = true;
            if (OnCommand != null)
            {
                OnCommand(data);
            }
            return true;
        }

        public static bool SendCommand(MotorCommand command)
        {
            if (command == null || command.Command <= CommandType.Null)
            {
                return false;
            }
            if (command is CoordMotorCommand)
            {
                return SendCoordCommand(command as CoordMotorCommand);
            }
            LastCommand = command;
            byte[] bytes = new byte[7];
            bytes[0] = (byte)command.Command;
            bytes[1] = (byte)(command.Speed / 256);
            bytes[2] = (byte)(command.Speed % 256);
            saveInt(bytes, 3, command.Line);
            Uart.Send(bytes);
            command.Sended = true;
            if (OnCommand != null)
            {
                OnCommand(command);
            }
            return true;
        }

        public static bool SendCommand(CommandType command)
        {
            if (OnCommand != null)
            {
                OnCommand(new MotorCommand(command));
            }
            return SendCommand((byte)command);
        }

        private static bool SendCommand(byte command)
        {
            if (command == 0)
            {
                return false;
            }
            if (!Ready) throw new Exception("UART not ready!");
            Uart.Send(new byte[] { command });
            return true;
        }

        public static bool GetStateAsync()
        {
            return SendCommand(CommandType.State);
        }

        public static bool Spindle(bool enable)
        {
            var mc = new MotorCommand(CommandType.Spindle);
            mc.Speed = enable ? (ushort)1 : (ushort)0;
            SpindleState = enable;
            return SendCommand(mc);
        }

        public static bool Stop()
        {
            CncProgram.Stop();
            return SendCommand(CommandType.Stop);
        }

        public static bool Pause()
        {
            CncProgram.Pause();
            return SendCommand(CommandType.Pause);
        }

        public static bool Resume()
        {
            CncProgram.Resume();
            return SendCommand(CommandType.Resume);
        }

        public static Dictionary<string, string> GetDevicesList()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            var list = Uart.GetDevicesList();
            if (list != null)
            {
                foreach (var ftDeviceInfoNode in list)
                {
                    var devString = ftDeviceInfoNode.Type.ToString() + " (" + ftDeviceInfoNode.Description + ")";
                    result.Add(ftDeviceInfoNode.SerialNumber, devString);
                }
            }
            return result;
        }

        public static void Close()
        {
            Uart.Close();
        }

        public static void Init(string cfgPath)
        {
            CncController.cfgPath = cfgPath;
            Uart.OnReceive += ProcessMessage;
            if (!File.Exists(cfgPath + "\\config.json"))
            {
                Uart.Init(38400, 1000);
                StreamWriter writer = new StreamWriter(cfgPath + "\\config.json", false, Encoding.UTF8);
                writer.Close();
                GetStateAsync();
                return;
            }
            StreamReader reader = new StreamReader(cfgPath + "\\config.json");
            var portName = reader.ReadLine();
            if (!reader.EndOfStream)
            {
                LastState = JsonConvert.DeserializeObject<MotorState>(reader.ReadLine());
                initialized = true;
            }
            reader.Close();
        }

        public static void Open()
        {
            if (initialized)
            {
                Uart.Init(38400, 1000);
                GetStateAsync();
            }
        }
    }
}