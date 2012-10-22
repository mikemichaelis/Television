using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Television.Interfaces;
using Television.Models.Enums;
using System.Diagnostics;
using Television.Models;

namespace Television.Models
{
    /// <summary>
    /// This is a component of a Television used to communicate with a RemoteControl
    /// </summary>
    class RemoteReceiver : IRemoteReceiver
    {
        // I am hard coding the CONTROL_CODE during unit testing
        internal Guid CONTROL_CODE = Guid.Parse("E6D56146-1D81-4B7A-829E-06D230F53CC2");     // Guid.NewGuid();

        ITelevision m_Television;

        public RemoteReceiver(ITelevision television)
        {
            m_Television = television;
                       
            // Register to receive remote control commands with specific CONTROL_CODE
            Messenger.Default.Register<RemoteControlCommand>(this, CONTROL_CODE, RemoteControlCommandReceived); //, CONTROL_CODE, RemoteControlCommandReceived);            

            // Register to receive all requests to transmit RemoteControlCode
            Messenger.Default.Register<RemoteControlCommand>(this, (message) =>
            {
                if (message.Command == RemoteCommandsEnum.REQUEST_CONTROL_CODE)
                {
                    // Broadcast RemoteControlCode
                    Messenger.Default.Send<RemoteControlCommand>(                        
                        // This is the return RemoteControlCommand with our static CONTROL_CODE
                        new RemoteControlCommand() { Command = RemoteCommandsEnum.CONTROL_CODE, Value = CONTROL_CODE });
                }
            });
        }
       
        // This should be an IDisposable implementation but I'm doing this quickly
        public void Dispose()
        {
            Messenger.Default.Unregister<RemoteControlCommand>(this, CONTROL_CODE);            
            Messenger.Default.Unregister<RemoteControlCommand>(this);
        }

        private void RemoteControlCommandReceived(RemoteControlCommand command)
        {
            // If State is OFF only recognize the STATE change command
            if (m_Television.State == TelevisionStateEnum.OFF && command.Command != RemoteCommandsEnum.STATE) return;

            switch (command.Command)
            {
                case RemoteCommandsEnum.CHANNEL:
                    try
                    {
                        m_Television.Channel = (int)command.Value;
                    }
                    catch { Debug.Assert(false); }

                    break;
                case RemoteCommandsEnum.CHANNEL_DOWN:
                    m_Television.Channel--;
                    break;
                case RemoteCommandsEnum.CHANNEL_UP:
                    m_Television.Channel++;
                    break;                
                case RemoteCommandsEnum.STATE:
                    try
                    {
                        m_Television.State = (TelevisionStateEnum)command.Value;
                    }
                    catch { Debug.Assert(false); }
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }
    }
}
