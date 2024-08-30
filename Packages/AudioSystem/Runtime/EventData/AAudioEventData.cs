using UnityEngine;
using UnityEngine.Audio;

namespace Seven.AudioSystem.EventData
{
    public abstract class AAudioEventData : ScriptableObject
    {
        [Min(0)] [SerializeField] private float _delaySeconds;
        [SerializeField] private AudioMixerGroup _audioMixerOutput;

        public float DelaySeconds => _delaySeconds;
        public AudioMixerGroup AudioMixerOutput => _audioMixerOutput;

        public abstract void Configure(AudioSource source);
    }
}