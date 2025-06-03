using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HUIFramework.Common
{
    public class GåmeConfig<T> where T : new()
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
            var table =  Addressables.LoadAssetAsync<TextAsset>(table_name).WaitForCompletion();
            value = JsonConvert.DeserializeObject<T>(CryptoUtil.Decrypt(table.text));
        }
    }
}