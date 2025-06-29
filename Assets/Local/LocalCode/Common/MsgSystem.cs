using System;
using System.Collections.Generic;

namespace LocalCode.Common{
    public static class MsgSystem
    {
        private static Dictionary<string, List<Action<object[]>>> event_table = new();
        public static void AddMsg(string event_name, Action<object[]> msg_action)
        {
            if (event_table.ContainsKey(event_name))
            {
                if (event_table[event_name].Contains(msg_action) == false)
                {
                    event_table[event_name].Add(msg_action);
                }
            }
            else
            {
                event_table.Add(event_name, new List<Action<object[]>>(){msg_action});
            }
            
        }

        public static void RemoveMsg(string event_name, Action<object[]> msg_action)
        {
            if (event_table.ContainsKey(event_name))
            {
                if (event_table[event_name].Contains(msg_action))
                {
                    event_table[event_name].Remove(msg_action);
                }
            }
        }

        public static void SendMsg(string event_name, object[] args)
        {
            if (event_table.ContainsKey(event_name))
            {
                for (var i = 0; i < event_table[event_name].Count; i++)
                {
                    event_table[event_name][i](args);
                }
            }
        }

        public static void ClearAllMsg()
        {
            event_table.Clear();
        }
    }
}