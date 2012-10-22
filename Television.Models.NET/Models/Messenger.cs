using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Television.Interfaces;
using System.Diagnostics;

namespace Television.Models
{    
    public class Messenger : IMessenger
    {
        private static Messenger m_DefaultMessenger = null;
        private Dictionary<object, Dictionary<object, object>> m_Registrations = new Dictionary<object, Dictionary<object, object>>();        
        private object broadcast = new object();
        
        public static IMessenger Default
        {
            get 
            {
                if (m_DefaultMessenger == null) m_DefaultMessenger = new Messenger();
                return m_DefaultMessenger;
            }
        }

        public void Register<T>(object receiver, Action<T> callback)
        {
            Register<T>(receiver, broadcast, callback);
        }

        public void Register<T>(object receiver, object code, Action<T> callback)
        {
            Debug.Assert(receiver != null);
            Debug.Assert(code != null);
            Debug.Assert(callback != null);

            Dictionary<object, object> registration = null;

            if (m_Registrations.ContainsKey(receiver))
            {
                registration = m_Registrations[receiver];
            }
            else
            {
                registration = new Dictionary<object, object>();
                m_Registrations.Add(receiver, registration);
            }
            
            CallbackRegistration<T> callbackRegistration = new CallbackRegistration<T>(callback);

            if (!registration.ContainsKey(code))
            {
                registration.Add(code, callbackRegistration);
            }
        }

        public void Unregister<T>(object receiver)
        {
            Unregister<T>(receiver, broadcast);
        }

        public void Unregister<T>(object receiver, object code)
        {             
            Dictionary<object, object> registration = m_Registrations[receiver];

            if (registration != null) registration.Remove(code);
        }
      
        public void Send<T>(T message)
        {
            Send<T>(message, broadcast);
        }

        public void Send<T>(T message, object code)
        {            
            foreach (var registration in m_Registrations.Values)
            {
                foreach (var keyValuePair in registration.Where(r => r.Key.Equals(code)).ToList())
                {
                    CallbackRegistration<T> callbackRegistration = keyValuePair.Value as CallbackRegistration<T>;
                    if (callbackRegistration != null) callbackRegistration.Callback(message);
                }
            }
        }      
    }
}
