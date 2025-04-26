using UnityEngine;

namespace HUIFramework.Common
{
    public class BaseComponent<T> :MonoBehaviour where T : BaseSystem<T>
    {
        private T owner;
        public T Owner => owner;

    }
}