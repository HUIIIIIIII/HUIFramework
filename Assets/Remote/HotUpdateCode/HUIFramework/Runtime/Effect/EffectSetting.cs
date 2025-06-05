using LocalCode.Common;
using System.Collections.Generic;
using UnityEngine;
namespace HUIFramework.Common.Effect
{
    [CreateAssetMenu(fileName = "EffectSetting", menuName = "HUIFramework/Setting/EffectSetting", order = 2)]
    public class EffectSetting : SingletonSoBase<EffectSetting>
    { 
        [SerializeField] private List<EffectItem> effect_items = new List<EffectItem>();
        public List<EffectItem> EffectItems => effect_items;
    }

    public enum EffectType
    {
        None,
    }
}