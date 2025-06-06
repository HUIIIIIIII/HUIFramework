using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using HybridCLR;
using LocalCode.Common;
using UnityEditor;
using UnityEngine;

namespace LocalCode
{
    public class LoadDlls : MonoBehaviour
    {
        private static List<string> aot_meta_assembly_files { get; } = new List<string>()
        {
            "HotUpdate.dll.bytes",
            "LocalCode.dll.bytes",
            "Newtonsoft.Json.dll.bytes",
            "Sirenix.Utilities.dll.bytes",
            "System.Core.dll.bytes",
            "UniTask.dll.bytes",
            "UnityEngine.CoreModule.dll.bytes",
            "YooAsset.dll.bytes",
            "mscorlib.dll.bytes",
        };
        private static Dictionary<string, byte[]> ass_datas = new Dictionary<string, byte[]>();
        private static Assembly hot_update_ass;

        public async UniTask LoadGame()
        {
            await LoadDll();
            LoadMetadataForAOTAssemblies();
#if !UNITY_EDITOR
            hot_update_ass = Assembly.Load(ass_datas["HotUpdate.dll.bytes"]);
#else
            hot_update_ass = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotUpdate");
#endif
        }
        
        private async UniTask LoadDll()
        {
            foreach (var aot_meta_ass in aot_meta_assembly_files)
            {
               var text_asset = await AssetSystem.Instance.LoadAssetAsync<TextAsset>($"AOT/{EditorUserBuildSettings.activeBuildTarget}/{aot_meta_ass}");
               ass_datas[aot_meta_ass] = text_asset.bytes;
            }
        } 
       
        private static void LoadMetadataForAOTAssemblies()
        {
            HomologousImageMode mode = HomologousImageMode.SuperSet;
            foreach (var aot_dll_name in aot_meta_assembly_files)
            {
                byte[] dll_bytes = ass_datas[aot_dll_name];
                LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dll_bytes, mode);
                GameLog.Log($"LoadMetadataForAOTAssembly:{aot_dll_name}. mode:{mode} ret:{err}");
            }
        }
    }
}
