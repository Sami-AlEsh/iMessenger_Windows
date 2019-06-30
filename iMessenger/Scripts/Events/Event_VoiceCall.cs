using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace iMessenger.Scripts.Events
{
    public enum Command
    {
        Invite = 0,
        Accept = 1,
        Drop = 3
    }

    class Event_VoiceCall : Message
    {
        public string Receiver;
        public Command command;

        /// <summary>
        /// Create Event_VoiceCall to send to SERVER
        /// </summary>
        /// <param name="Receiver"></param>
        /// <param name="command"></param>
        public Event_VoiceCall(string Receiver, Command command)
        {
            this.type = "VoiceCall";
            //this.ID = "null"; //TODO get ID from server
            this.Receiver = Receiver;
            this.command = command;
        }

        /// <summary>
        /// Parsing Received Voip Call and excute the command
        /// </summary>
        /// <param name="VoiceMessage"></param>
        public Event_VoiceCall(JObject VoiceMessage)
        {
            string commandStr = (string)VoiceMessage["command"];
            this.command = (Command)Enum.Parse(typeof(Command), commandStr, true);
            this.Receiver = (string)VoiceMessage["from"];

            Event_Voice_Handler((string)VoiceMessage["ip"]);
        }
        private void Event_Voice_Handler(string ip)
        {
            switch (this.command)
            {
                case Command.Invite:
                    {
                        Application.Current.Dispatcher.Invoke(() => new Call(Receiver, CallStatus.Receiver, ip).ShowDialog());
                        break;
                    }
                case Command.Accept:
                    {
                        Application.Current.Dispatcher.Invoke(() => Call.AcceptCallRecieved(ip, 1550));
                        break;
                    }
                case Command.Drop:
                    {
                        Application.Current.Dispatcher.Invoke(()=> Call.DropCallRecieved());
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Unknown VOIP Command type !");
                        break;
                    }
            }
        }




        public override byte[] GetBytes()
        {
            throw new NotImplementedException();
        }

        public override string GetJson()
        {
            JObject Jobj = null;

            switch (this.command)
            {
                case Command.Invite:
                    {
                        Jobj = new JObject( new JProperty("type", type),
                                            new JProperty("command", "invite"),
                                            new JProperty("to", Receiver)
                                            );
                        break;
                    }
                case Command.Accept:
                    {
                        Jobj = new JObject(new JProperty("type", type),
                                            new JProperty("command", "accept"),
                                            new JProperty("to", Receiver)
                                            );
                        break;
                    }
                case Command.Drop:
                    {
                        Jobj = new JObject(new JProperty("type", type),
                                            new JProperty("command", "drop"),
                                            new JProperty("to", Receiver)
                                            );
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Unknown VOIP Command type !");
                        break;
                    }
            }
            return Jobj.ToString();
        }
    }
}
