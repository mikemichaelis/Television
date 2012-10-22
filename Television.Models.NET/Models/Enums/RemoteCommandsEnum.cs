using System;
using System.Net;

namespace Television.Models.Enums
{
    public enum RemoteCommandsEnum
    {
        REQUEST_CONTROL_CODE,       // Sent by remote to ask receiving TV to broadcast their CONTROL_CODE
        CONTROL_CODE,               // Sent by TV w/ Value set to their CONTROL_CODE
        CHANNEL_UP,                 // Channel++
        CHANNEL_DOWN,               // Channel--
        CHANNEL,                    // Value is specific channel
        STATE,                      // Value is TelevisionStateEnum     
    }
}
