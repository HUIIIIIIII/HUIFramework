using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LocalCode.Common
{
    public abstract class SingletonMonoBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected SingletonMonoBase(){}
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }

                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            DontDestroyOnLoad(this.gameObject);
        }

        public virtual async UniTask InitAsync()
        {
        }

        private void OnDestroy()
        {
            instance = null;
        }
    }
}