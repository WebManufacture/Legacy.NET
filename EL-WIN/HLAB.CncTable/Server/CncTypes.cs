using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace MRS.Hardware.Server
{

    public class MotorState
    {
        public static int loadInt(byte[] arr, int index)
        {
            return (int)(arr[index] * 16777216 + arr[index + 1] * 65536 + arr[index + 2] * 256 + arr[index + 3]);
        }

        public static int saveInt(byte[] arr, int index, int value)
        {
            arr[index] = (byte)(value >> 24);
            arr[index + 1] = (byte)(value >> 16);
            arr[index + 2] = (byte)(value >> 8);
            arr[index + 3] = (byte)(value);
            return value;
        }

        public byte command;
        public byte address;
        public int x;
        public int y;
        public int z;
        public int xLimit;
        public int yLimit;
        public int zLimit;
        public byte state;
        public byte stateA;
        public byte stateB;
        public DateTime date = DateTime.Now;
        public int line;

        public CommandType Command
        {
            get { return (CommandType)command; }
        }

        public CncState State
        {
            get { return (CncState)state; }
        }

        public override string ToString()
        {
            var str = JsonConvert.SerializeObject(this);
            return str.Remove(str.Length - 1) + ", \"type\" : \"state\"}";
        }

        public static MotorState Deserialize(byte[] data)
        {
            if (data == null) return null;
            if (data.Length >= 33)
            {
                MotorState obj = new MotorState();
                obj.date = DateTime.Now;
                obj.address = data[0];
                obj.command = data[1];
                obj.state = data[2];
                obj.line = MotorState.loadInt(data, 3);
                obj.x = loadInt(data, 7);
                obj.y = loadInt(data, 11);
                obj.z = loadInt(data, 15);
                obj.xLimit = loadInt(data, 19);
                obj.yLimit = loadInt(data, 23);
                obj.zLimit = loadInt(data, 27);
                obj.stateA = data[31];
                obj.stateB = data[32];
                return obj;
            }
            return null;
        }
    }
    

    public enum CommandType : byte
    {
        Null = 0,
        Go = 1,
        Rebase = 2,
        Stop = 3,
        State = 4,
        Move = 5,
        Pause = 6,
        Resume = 7,
        Spindle1 = 8,
        Spindle2 = 9,
        WaitMask = 10,
        Config = 11,
        Reset = 12,
        FlushBuffer = 13,
        NextBuffer = 14,
        BufferEmpty = 15,
        BufferFull = 16,
        Error = 20
    }

    public class MotorCommand
    {
        public byte command;
        public int line = 0;
        public ushort speed = 0;
        public bool Sended;
        public int x;
        public int y;
        public int z;
        public byte paramA;
        public byte paramB;

        public int X { get { return x; } set { x = value; } }
        public int Y { get { return y; } set { y = value; } }
        public int Z { get { return z; } set { z = value; } }
        public ushort Speed { get { return speed; } set { speed = value; } }


        public static MotorCommand FromJSON(string data)
        {
            var command = JsonConvert.DeserializeObject<MotorCommand>(data);
            return command;
        }

        public static MotorCommand Rebase(int x, int y, int z)
        {
            var command = new MotorCommand(CommandType.Rebase);
            command.x = x;
            command.y = y;
            command.z = z;
            return command;
        }

        public static MotorCommand Go(int x, int y, int z)
        {
            var command = new MotorCommand(CommandType.Go);
            command.x = x;
            command.y = y;
            command.z = z;
            return command;
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

        public static MotorCommand CreateCommand(byte command)
        {
            return new MotorCommand(command);
        }

        internal static byte[] GetLowLevel(byte command, byte addr)
        {
            return (new MotorCommand(command)).Serialize(addr);
        }

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

        public byte[] Serialize(byte Address)
        {
            byte[] bytes = new byte[22];
            bytes[0] = (byte)Address;
            bytes[1] = (byte)this.Command;
            bytes[2] = (byte)(this.speed / 256);
            bytes[3] = (byte)(this.speed % 256);
            MotorState.saveInt(bytes, 4, this.line);
            MotorState.saveInt(bytes, 8, this.x);
            MotorState.saveInt(bytes, 12, this.y);
            MotorState.saveInt(bytes, 16, this.z);
            bytes[20] = this.paramA;
            bytes[21] = this.paramB;
            return bytes;
        }


        public bool IsCoordCommand
        {
            get
            {
                return Command == CommandType.Go || Command == CommandType.Move || Command == CommandType.Rebase;
            }
        }

    }

    public enum CncState : byte
    {
        Info = 0,
        Working = 1,
        Completed = 2,
        Paused = 3,
        Error = 4,
        Aborted = 5
    }
    
    public enum CncProgramState : byte
    {
        NotStarted,
        Running,
        Paused,
        Completed,
        Aborted
    }

}
