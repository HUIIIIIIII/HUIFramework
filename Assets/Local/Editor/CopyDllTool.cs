using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

# if UNITY_EDITOR
public class CopyDllTool
{
    private const string aot_path = "Assets/Remote/GameAssets/AOT";
    
    public static string GetDllPath()
    {
        return Application.dataPath.Replace("Assets",
            $"HybridCLRData/AssembliesPostIl2CppStrip/{EditorUserBuildSettings.activeBuildTarget}");
    }

    private static string GetHotUpdatePath()
    {
        return GetDllPath().Replace("AssembliesPostIl2CppStrip", "HotUpdateDlls");
    }

    [MenuItem("HUITool/CopyDll")]
    public static void CopyDll()
    {
        var targetDir = $"{Application.dataPath}/Remote/GameAssets/AOT/{EditorUserBuildSettings.activeBuildTarget}";
        if (!Directory.Exists(targetDir))
        {
            Directory.CreateDirectory(targetDir);
        }
        foreach (var aot_assemly in AOTGenericReferences.PatchedAOTAssemblyList)
        {
            var aot_path = $"{GetDllPath()}/{aot_assemly}"; 
            var targetPath = $"{targetDir}/{aot_assemly}.bytes";
            using (var stream = File.Open(targetPath, FileMode.Create))
            {
                using (var fileStream = File.Open(aot_path, FileMode.Open))
                {
                    fileStream.CopyTo(stream);
                }
            }
        }
        var hot_update_dll_path = $"{GetHotUpdatePath()}/HotUpdate.dll";
        var target_hot_path = $"{targetDir}/HotUpdate.dll.bytes";
        using (var stream = File.Open(target_hot_path, FileMode.Create))
        {
            using (var fileStream = File.Open(hot_update_dll_path, FileMode.Open))
            {
                fileStream.CopyTo(stream);
            }
        }
        AssetDatabase.Refresh();
    }
  
}
# endif