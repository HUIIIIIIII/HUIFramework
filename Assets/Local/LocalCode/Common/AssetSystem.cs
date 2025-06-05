using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LocalCode.Common;
using NPOI.SS.Formula.Functions;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace LocalCode.Common
{
    public class AssetSystem : SingletonMonoBase<AssetSystem>
    {
        private const string asset_root_path = "Assets/Remote/GameAssets/";

        private Dictionary<string, List<AssetHandle>> asset_handles = new Dictionary<string, List<AssetHandle>>();

        public override async UniTask InitAsync()
        {
        }

        public T LoadAsset<T>(string name) where T : UnityEngine.Object
        {
            name = asset_root_path + name;
            var handle = YooAssets.LoadAssetSync<T>(name);
            AddAssetHandle(name, handle);
            return handle.AssetObject as T;
        }

        public async UniTask<T> LoadAssetAsync<T>(string name) where T : UnityEngine.Object
        {
            name = asset_root_path + name;
            var handle = YooAssets.LoadAssetAsync<T>(name);
            AddAssetHandle(name, handle);
            await handle.Task;
            return handle.AssetObject as T;
        }

        public GameObject LoadGameObject(string name)
        {
            name = asset_root_path + name;
            var handle = YooAssets.LoadAssetSync<GameObject>(name);
            AddAssetHandle(name, handle);
            return handle.InstantiateSync();
        }

        public async UniTask<GameObject> LoadGameObjectAsync(string name)
        {
            name = asset_root_path + name;
            var handle = YooAssets.LoadAssetAsync<GameObject>(name);
            AddAssetHandle(name, handle);
            await handle.Task;
            return handle.InstantiateSync();
        }

        public Scene LoadScene(string name, LoadSceneMode mode = LoadSceneMode.Single)
        {
            name = asset_root_path + name;
            var handle = YooAssets.LoadSceneSync(name);
            return handle.SceneObject;
        }

        public async UniTask<Scene> LoadSceneAsync(string name, LoadSceneMode mode = LoadSceneMode.Single)
        {
            name = asset_root_path + name;
            var handle = YooAssets.LoadSceneAsync(name, mode);
            await handle.Task;
            return handle.SceneObject;
        }

        public async UniTask UnloadScene(string name)
        {
            name = asset_root_path + name;
            await SceneManager.UnloadSceneAsync(name);
        }

        public void UnloadAsset(string name)
        {
            name = asset_root_path + name;
            if (!asset_handles.ContainsKey(name))
            {
                return;
            }

            var handles = asset_handles[name];
            foreach (var handle in handles)
            {
                handle.Release();
            }

            asset_handles.Remove(name);
        }

        private void AddAssetHandle(string name, AssetHandle handle)
        {
            name = asset_root_path + name;
            if (!asset_handles.ContainsKey(name))
            {
                asset_handles.Add(name, new List<AssetHandle>());
            }

            asset_handles[name].Add(handle);
        }
    }
}