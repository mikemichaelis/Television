using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Television.Interfaces
{
    public interface IMessenger
    {        
        void Register<T>(object receiver, object code, Action<T> callback);
        void Register<T>(object receiver, Action<T> callback);

        void Send<T>(T message);
        void Send<T>(T message, object code);

        void Unregister<T>(object receiver, object code);
        void Unregister<T>(object receiver);    
    }
}
