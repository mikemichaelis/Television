using System;
using System.Net;
using Television.Models.Enums;
using Television.Interfaces;

namespace Television.Models
{    
    public class RemoteControlCommand : IRemoteControlCommand
    {        
        public RemoteCommandsEnum Command { get; set; }        
        public object Value { get; set; }
    }
}
