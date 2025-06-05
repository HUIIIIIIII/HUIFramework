using System;
using Cysharp.Threading.Tasks;
using LocalCode.Common;
using UniFramework.Event;
using UnityEngine;
using YooAsset;

namespace LocalCode
{
    public class GameEntry : MonoBehaviour
    {
        [SerializeField] private LoadingGameForm loading_form;
        [SerializeField] private GameLocalSetting local_setting;
        private void Start()
        {
            InitSync().Forget();
            DontDestroyOnLoad(this);
        }

        private async UniTask InitSync()
        {
            UniEvent.Initalize();
            YooAssets.Initialize();
            
            var operation = new PackageLoad("DefaultPackage",local_setting);
            YooAssets.StartOperation(operation);
            await operation;
            
            var gamePackage = YooAssets.GetPackage("DefaultPackage");
            YooAssets.SetDefaultPackage(gamePackage);
            await AssetSystem.Instance.InitAsync();
            AssetSystem.Instance.LoadSceneAsync("Scene/Game");
        }
    }
}

