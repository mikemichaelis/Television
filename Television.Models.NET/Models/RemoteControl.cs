using System;
using System.Net;
using System.Diagnostics;
using Television.Models.Enums;
using Television.Interfaces;

namespace Television.Models
{
    public class RemoteControl : IRemoteControl
    {
        /// <summary>
        /// This is the CONTROL_CODE RemoteControl is linked to
        /// </summary>
        private Guid CONTROL_CODE;
      
        public RemoteControl()
        {            
        }

        /// <summary>
        /// Ask the Remote Control to program with any listening Televisions
        /// </summary>
        /// <param name="callback">callback once programming is complete with a bool success result</param>
        public void Program( Action<bool> callback )
        {           
            // Begin receiving NotificationMessage<RemoteControlCommand> messages
            Messenger.Default.Register<RemoteControlCommand>(this,
                (message) =>
                {
                    // The first Television to transmit CONTROL_CODE we will link to
                    if( message.Command == RemoteCommandsEnum.CONTROL_CODE) 
                    {                        
                        try
                        {
                            CONTROL_CODE = (Guid)message.Value;                            
                            Messenger.Default.Unregister<RemoteControlCommand>(this);                            
                            if (callback != null) callback(true);
                            
                        }
                        catch { Debug.Assert(false); }
                    }
                });

            Broadcast_Request_Control_Code();
        }

        /// <summary>
        /// Tell Remote Control to stop listening, this is really unnecessary but cleans up unit testing
        /// </summary>
        public void Dispose()
        {
            Messenger.Default.Unregister<RemoteControlCommand>(this);
        }

        /// <summary>
        /// Sends a broadcast message requesting all listeners to transmit CONTROL_CODE
        /// </summary>
        private void Broadcast_Request_Control_Code()
        {                       
            Messenger.Default.Send<RemoteControlCommand>(                
                    new RemoteControlCommand()
                    {
                        Command = RemoteCommandsEnum.REQUEST_CONTROL_CODE,
                        Value = null
                    });
        }

        /// <summary>
        /// Sends a message directly to the currently linked CONTROL_CODE
        /// </summary>
        /// <param name="command">Command action requested</param>
        /// <param name="value">Associated required parameter for requested command</param>        
        private void Transmit(RemoteCommandsEnum command, object value)
        {
            Messenger.Default.Send<RemoteControlCommand>(                
                    new RemoteControlCommand()
                    {
                        Command = command,
                        Value = value
                    }, CONTROL_CODE);
        }

        #region IRemoteControl Methods
        /// <summary>
        /// Set to a specific channel
        /// </summary>
        /// <param name="channel"></param>
        public void Channel(int channel) 
        {
            Transmit(RemoteCommandsEnum.CHANNEL, channel);
        }

        /// <summary>
        /// Increment current channel.
        /// </summary>
        public void Channel_Up() 
        {
            Transmit(RemoteCommandsEnum.CHANNEL_UP, null);
        }

        /// <summary>
        /// Decrement current channel
        /// </summary>
        public void Channel_Down() 
        {
            Transmit(RemoteCommandsEnum.CHANNEL_DOWN, null);
        }

        /// <summary>
        /// Set State of Television (ON/OFF)
        /// </summary>
        /// <param name="state"></param>
        public void State(TelevisionStateEnum state) 
        {
            Transmit(RemoteCommandsEnum.STATE, state);
        }        
        #endregion
    }
}
