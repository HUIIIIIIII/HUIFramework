using System;
using Cysharp.Threading.Tasks;
using HUIFramework.Common;
using HUIFramework.Runtime.Http;
using UnityEngine;

namespace HotUpdate
{
    public class TestData : MonoBehaviour
    {
        private void Start()
        {
            TestHttp().Forget();
        }

        private async UniTask TestHttp()
        {
            await EasyHttpSystem.Instance.InitAsync();
            EasyHttpSystem.Instance.Post(new PlayerInfoItem());
        }
    }
}