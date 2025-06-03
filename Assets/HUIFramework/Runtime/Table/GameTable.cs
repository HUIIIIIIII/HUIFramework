using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HUIFramework.Common
{
    public class GameTable<TValue> where TValue : TableValue
    {
        private const string table_path = "Assets/Game/Table/{0}.txt";
        private static Dictionary<string, TValue> table_value_dic = new();
        private static List<TValue> table_value_list = new();

        public static Dictionary<string, TValue> TableValueDic
        {
            get
            {
                if (table_value_dic == null)
                {
                    LoadTableValue();
                }

                return table_value_dic;
            }
        }

        public static List<TValue> TableValueList
        {
            get 
            {
                if (table_value_list == null)
                {
                    LoadTableValue();
                }

                return table_value_list;
            }
        }

        public static void LoadTableValue()
        {
            var table_name = string.Format(table_path, typeof(TValue).Name.Replace("Value",""));
            var table = Addressables.LoadAssetAsync<TextAsset>(table_name).WaitForCompletion();
            table_value_list = JsonConvert.DeserializeObject<List<TValue>>(CryptoUtil.Decrypt(table.text));
            table_value_dic = table_value_list.ToDictionary(value => value.id, value => value);
        }

        public static TValue GetTableValue(string id)
        {
            if (table_value_dic == null)
            {
                LoadTableValue();
            }
            if (table_value_dic.TryGetValue(id, out var value))
            {
                return value;
            }
            else
            {
                GameLog.LogError($"Table value with id {id} not found in {typeof(TValue).Name}");
                return default;
            }
        }
    }
}