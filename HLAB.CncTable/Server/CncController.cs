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

    public class CncController
    {
        public static MotorState LastState = null;
        public static CncProgram Program;
        public static Serial Uart = null;
        public static MotorCommand LastCommand = null;
        public static bool SpindleState = false;
        public static event MessageHandler OnMessage;
        public static event CommandHandler OnCommand;
        public static byte CncAddress;
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

        public static void ProcessMessage(byte[] data)
        {
            MotorState obj = MotorState.Deserialize(data);
            if (obj != null)
            {
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
        
        public static bool SendCommand(MotorCommand command)
        {
            if (command == null || command.Command <= CommandType.Null)
            {
                return false;
            }
            LastCommand = command;
            Uart.SendSized(command.Serialize(CncAddress));
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
            Uart.SendSized(MotorCommand.GetLowLevel(command, CncAddress));
            return true;
        }

        public static bool GetStateAsync()
        {
            return SendCommand(CommandType.State);
        }

        public static bool Spindle(bool enable)
        {
            var mc = new MotorCommand(CommandType.Spindle1);
            mc.speed = enable ? (ushort)1 : (ushort)0;
            SpindleState = enable;
            return SendCommand(mc);
        }

        public static bool Stop()
        {
            return SendCommand(CommandType.Stop);
        }

        public static bool Pause()
        {
            return SendCommand(CommandType.Pause);
        }

        public static bool Resume()
        {
            return SendCommand(CommandType.Resume);
        }

        public static void Close()
        {
            Uart.Close();
        }

        public static void Init(Serial serialPort, string cfgPath, byte cncAddress)
        {
            CncController.cfgPath = cfgPath;
            CncAddress = cncAddress;
            Uart = serialPort;
            Uart.Connect();
            Uart.OnReceive += ProcessMessage;
            if (!File.Exists(cfgPath + "\\config.json"))
            {
                StreamWriter writer = new StreamWriter(cfgPath + "\\config.json", false, Encoding.UTF8);
                writer.Close();
                GetStateAsync();
                return;
            }
            StreamReader reader = new StreamReader(cfgPath + "\\config.json");
            var cfgStr = reader.ReadToEnd();
            reader.Close();
            if (cfgStr.Length > 0)
            {
                LastState = JsonConvert.DeserializeObject<MotorState>(cfgStr);
            }
            else
            {
                LastState = new MotorState();
            }
            initialized = true;
            
        }

        public static void Poll()
        {
            if (Uart.GetState() < EDeviceState.Online)
            {
                Uart.Connect();
            }
            GetStateAsync();
        }
    }
}