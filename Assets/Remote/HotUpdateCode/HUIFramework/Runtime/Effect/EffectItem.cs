using System;
using UnityEngine;

namespace HUIFramework.Common.Effect
{
    [Serializable]
    public class EffectItem : MonoBehaviour
    {
        public EffectType type;
        public ParticleSystem particle;
    }
}