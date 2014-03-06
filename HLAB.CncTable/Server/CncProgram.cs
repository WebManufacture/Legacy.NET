using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace MRS.Hardware.Server
{

    public class CncProgram
    {
        public MotorCommand[] Commands;
        public int LastSendedLine;
        public int CurrentLine;

        public MotorCommand LastSended
        {
            get
            {
                return Commands[LastSendedLine];
            }
        }

        public MotorCommand CurrentCommand
        {
            get
            {
                return Commands[CurrentLine];
            }
        }

        public CncProgram(string data)
        {
            CurrentLine = 0;
            Commands = JsonConvert.DeserializeObject<MotorCommand[]>(data);
            for (var i = 0; i < Commands.Length; i++)
            {
                Commands[i].line = i + 1;
            }
            CncController.OnMessage += CncControllerOnOnMessage;
        }

        private void CncControllerOnOnMessage(MotorState obj)
        {
            if (obj.line > 0 && obj.Command < CommandType.Config)
            {
                CurrentLine = obj.line;
            }
        }
        
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
