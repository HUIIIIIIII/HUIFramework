using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace HUIFramework.Common
{
    public static class CommonExtension
    {
        private static readonly System.Random rng = new System.Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
        
        public static string GetRandomId() 
        {
            string local_time = DateTime.Now.ToShortTimeString();
            string character_list = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";//list中存放着随机的元素
            string user_host_name = Dns.GetHostName();
            MD5 mD5 = new MD5CryptoServiceProvider();
            user_host_name = BitConverter.ToString(mD5.ComputeHash(Encoding.Default.GetBytes(user_host_name)), 4, 4);//生成多长的值，可以自行调参
            user_host_name = user_host_name.Replace("-", "");
            Random random = new Random();
            string code = "";   
            for (int i = 0; i < 10; i++)
            {
                code += character_list[random.Next(0, character_list.Length - 1)];
            }
            string client_id = "client_"+ local_time + user_host_name + code;
 
            return client_id;
        }
        public static Tvalue GetValue<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey key)
        {
            Tvalue value = default(Tvalue);
            dict.TryGetValue(key, out value);
            return value;
        }
        public static T[] GetEnumValues<T>() where T : System.Enum
        {
            return (T[])System.Enum.GetValues(typeof(T));
        }
    }
}