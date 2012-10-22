using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Television.Models.Enums;

namespace Television.Interfaces
{
    public interface IRemoteControlCommand
    {
        RemoteCommandsEnum Command { get; set; }
        object Value { get; set; }
    }
}
