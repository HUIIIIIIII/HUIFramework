using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using LocalCode.Common;

namespace HUIFramework.Common.Effect
{
    public class EffectSystem : SingletonMonoBase<EffectSystem>
    {
        private Dictionary<EffectType,SimpleObjectPool<EffectItem>> effects = new Dictionary<EffectType, SimpleObjectPool<EffectItem>>();
        public override async UniTask InitAsync()
        {
            foreach (var effect_item in EffectSetting.Instance.EffectItems)
            {
                if (!effects.ContainsKey(effect_item.type))
                {
                    effects.Add(effect_item.type, new SimpleObjectPool<EffectItem>(() =>
                    {
                        EffectItem effect = Instantiate(effect_item);
                        effect.transform.SetParent(this.transform);
                        return effect;
                    }, 1));
                }
            }
        }

        public EffectItem GetEffect(EffectType type)
        {
            if (!effects.ContainsKey(type))
            {
                return null;
            }
            return effects[type].Get();
          
        }
        public void ReturnEffect(EffectItem effect)
        {
            effect.particle.Stop();
            effect.gameObject.SetActive(false);
            effect.transform.SetParent(this.transform);
            if (effects.ContainsKey(effect.type))
            {
                effects[effect.type].Return(effect);
            }
        }

        public EffectItem ShowEffectWithTransform(EffectType type, Transform target ,float duration)
        {
            var effect = GetEffect(type);
            if (effect != null)
            {
                effect.transform.position = target.position;
                effect.transform.rotation = target.rotation;
                effect.transform.SetParent(target);
                effect.gameObject.SetActive(true);
                effect.particle.Play();
                if (duration > 0)
                {
                    TimerSystem.Instance.ScheduleOnce(duration, () =>
                    {
                        ReturnEffect(effect);
                    });
                }
            }
            return effect;
        }

        public EffectItem ShowEffectWithPos(EffectType type, Vector3 pos, float duration)
        {
            var effect = GetEffect(type);
            if (effect != null)
            {
                effect.transform.position = pos;
                effect.gameObject.SetActive(true);
                effect.particle.Play();
                if (duration > 0)
                {
                    TimerSystem.Instance.ScheduleOnce(duration, () =>
                    {
                        ReturnEffect(effect);
                    });
                }
            }

            return effect;
        }
    }
}