using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HUIFramework.Common.IAP
{
    public class IAPItem : MonoBehaviour
    {
        public async UniTask InitAsync()
        {
            await UniTask.Yield();
        }
        public async UniTask<bool> Purchase(string productId)
        {
            return true;
        }
    }
}