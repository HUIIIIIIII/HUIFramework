using System;
using System.Collections.Generic;

namespace LocalCode.Common{
    public static class MsgSystem
    {
        private static readonly Dictionary<string, Delegate> event_table = new Dictionary<string, Delegate>();

        public static void AddMsg<T>(string event_name, Action<T> callback)
        {
            if (event_table.TryGetValue(event_name, out var del))
            {
                event_table[event_name] = Delegate.Combine(del, callback);
            }
            else
            {
                event_table[event_name] = callback;
            }
            
        }

        public static void RemoveMsg<T>(string event_name, Action<T> callback)
        {
            if (event_table.TryGetValue(event_name, out var del))
            {
                del = Delegate.Remove(del, callback);
                if (del == null)
                    event_table.Remove(event_name);
                else
                    event_table[event_name] = del;
            }
        }

        public static void SendMsg<T>(string event_name, T arg)
        {
            if (event_table.TryGetValue(event_name, out var del))
            {
                if (del is Action<T> callback)
                {
                    callback.Invoke(arg);
                }
            }
        }

        public static void Clear()
        {
            event_table.Clear();
        }
    }
}