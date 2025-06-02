using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace HUIFramework.Common.Ad
{
    [CreateAssetMenu(fileName = "AdSetting", menuName = "HUIFramework/Setting/AdSetting", order = 1)]
    public class AdSetting : ScriptableObject
    {
        [OdinSerialize,OnValueChanged("OnPlatformChanged")] private HashSet<AdPlatform> ad_platforms = new HashSet<AdPlatform>();
        public void OnPlatformChanged()
        {
            foreach (var all_platform in Enum.GetValues(typeof(AdPlatform)))
            {
                var platform = (AdPlatform)all_platform;
                if (!ad_platforms.Contains(platform))
                {
                    var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
                    if (defines.Contains($"ENABLE_{platform};"))
                    {
                        defines = defines.Replace($"ENABLE_{platform};", "");
                        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, defines);
                    }
                }
                else
                {
                    var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
                    if (!defines.Contains($"ENABLE_{platform}"))
                    {
                        defines += $"ENABLE_{platform};";
                        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, defines);
                    }
                }
            }
        }
    }
    public enum AdPlatform
    {
        MAX_AD,
        UNITY_AD,
    }
}