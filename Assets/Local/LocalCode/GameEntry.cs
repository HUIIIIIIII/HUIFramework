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
        [SerializeField] private LoadDlls load_dlls;
        private void Start()
        {
            InitSync().Forget();
            DontDestroyOnLoad(this);
        }

        private async UniTask InitSync()
        {
            loading_form.Show(true);
            UniEvent.Initalize();
            YooAssets.Initialize();
            
            var operation = new PackageLoad("DefaultPackage",local_setting);
            YooAssets.StartOperation(operation);
            await operation;
            
            await AssetSystem.Instance.InitAsync();
            await load_dlls.LoadGame();
            AssetSystem.Instance.LoadSceneAsync("Scene/Game");
            loading_form.Show(false);
        }
    }
}

