using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Television.Models.Enums;

namespace Television.Interfaces
{
    public interface IRemoteControl
    {
        void Program( Action<bool> callback );

        void Channel(int channel);
        void Channel_Up();
        void Channel_Down();
        void State(TelevisionStateEnum state);

        void Dispose();
    }
}
