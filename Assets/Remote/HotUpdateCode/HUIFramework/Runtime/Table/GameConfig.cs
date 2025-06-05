using LocalCode.Common;
using Newtonsoft.Json;
using UnityEngine;

namespace HUIFramework.Common
{
    public class GameConfig<T> where T : new()
    {
        private static T value;
        private const string table_path = "Assets/Game/Table/{0}.txt";

        public static T Value
        {
            get
            {
                if (value == null)
                {
                    LoadConfigValue();
                }

                return value;
            }
        }
        public static void LoadConfigValue()
        {
            var table_name = string.Format(table_path, typeof(T).Name.Replace("Config",""));
            var table = AssetSystem.Instance.LoadAsset<TextAsset>(table_name);
            value = JsonConvert.DeserializeObject<T>(CryptoUtil.Decrypt(table.text));
        }
    }
}