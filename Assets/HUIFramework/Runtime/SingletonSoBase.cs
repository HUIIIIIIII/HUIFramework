using UnityEngine;
using UnityEngine.AddressableAssets;

namespace HUIFramework.Common
{
    public class SingletonSoBase<T> : ScriptableObject where T : ScriptableObject
    {
        private const string so_path = "Assets/Game/Setting/{0}.asset";
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Addressables.LoadAssetAsync<T>(string.Format(so_path,typeof(T).Name)).WaitForCompletion();
                }
                return instance;
            }
        }
        
    }
  
}