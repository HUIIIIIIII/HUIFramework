using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HUIFramework.Common
{
    public class SystemRoot : MonoBehaviour
    {
        private static Dictionary<string,object> systems = new Dictionary<string,object>();
        
        public static void RegistSystem<T>(T system) where T : BaseSystem<T>
        {
            if (!systems.ContainsKey(typeof(T).ToString()))
            {
                
            }
            system.OnAdd();
        }
        public static void UnRegistSystem<T>(T system) where T : BaseSystem<T>
        {
            if (systems.ContainsKey(typeof(T).ToString()))
            {
                system.OnRemove();
            }
        }
    }
}