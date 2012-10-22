using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Television.Models
{
    internal class CallbackRegistration<T>
    {
        public object objectType { get; private set; }

        public Action<T> Callback { get; private set; }

        public CallbackRegistration(Action<T> callback)
        {
            Callback = callback;
            objectType = typeof(T);
        }
    }
}
