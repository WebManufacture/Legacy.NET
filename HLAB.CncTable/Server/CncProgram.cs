﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace MRS.Hardware.Server
{
    
    public delegate void ProgramStateHandler(CncProgramState state, MotorCommand current);
    public class CncProgram
    {
        public event ProgramStateHandler OnStateChange;
        public MotorCommand[] Commands;
        private CncProgramState state;

        public CncProgramState State
        {
            get { return state; }
            private set { 
                state = value;
                if (OnStateChange != null)
                {
                    OnStateChange(value, LastSended);
                }
            }
        }

        public MotorCommand LastSended;
        public int CurrentLine;

        public MotorCommand CurrentCommand
        {
            get
            {
                return Commands[CurrentLine];
            }
        }

        public CncProgram(string data)
        {
            CurrentLine = -1;
            State = CncProgramState.NotStarted;
            Commands = JsonConvert.DeserializeObject<MotorCommand[]>(data);
            for (var i = 0; i < Commands.Length; i++)
            {
                Commands[i].line = i + 1;
            }
            CncController.OnMessage += CncControllerOnOnMessage;
        }

        public void Start()
        {
            if (Commands.Length > 0)
            {
                State = CncProgramState.Running;
                CurrentLine = -1;
                NextCommand();
            }
        }

        private void Stop()
        {
            CurrentLine = Commands.Length;
            if (OnStateChange != null)
            {
                State = CncProgramState.Completed;
            }
        }

        private void Abort()
        {
            CurrentLine = Commands.Length;
            if (OnStateChange != null)
            {
                State = CncProgramState.Aborted;
            }
        }

        private void NextCommand()
        {
            NextCommand(null);
        }

        private void NextCommand(MotorState ms)
        {
            CurrentLine++;
            if (CurrentLine < Commands.Length)
            {
                CncController.SendCommand(CurrentCommand);
                LastSended = CurrentCommand;
            }
            else
            {
                Stop();
            }
        }

        private void CncControllerOnOnMessage(MotorState obj)
        {
            if (state != CncProgramState.NotStarted && state != CncProgramState.Aborted)
            {
                if (obj.Command == CommandType.Stop)
                {
                    Abort();
                    return;
                }
                if (obj.line > 0)
                {
                    switch (obj.State)
                    {
                        case CncState.Completed:
                            NextCommand(obj);
                            break;
                        case CncState.Error:
                        case CncState.Aborted:
                            Stop();
                            break;
                    }
                }
            }
        }
        
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
