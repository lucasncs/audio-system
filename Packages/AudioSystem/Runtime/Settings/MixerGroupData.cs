using System;
using UnityEngine.Audio;

namespace Seven.AudioSystem.Settings
{
    [Serializable]
    internal struct MixerGroupData
    {
        public AudioMixerGroup AudioMixerGroup;
        public string VolumeExposedKey;
    }
}