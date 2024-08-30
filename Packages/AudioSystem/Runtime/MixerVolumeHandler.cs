using UnityEngine;
using UnityEngine.Audio;

namespace Seven.AudioSystem
{
    internal class MixerVolumeHandler
    {
        private const float MIXER_SLIDER_CEIL = 20f;
        private const float MIN_DECIBEL_VOLUME_THRESHOLD = float.Epsilon;

        private readonly AudioMixer _audioMixer;
        private readonly string _volumeExposedKey;

        private float _currentVolume;
        private bool _isMuted;

        private float MixerVolume
        {
            get
            {
                if (!_audioMixer) return 0;
                _audioMixer.GetFloat(_volumeExposedKey, out float mixerVolume);
                float normalizedValue = Mathf.Pow(10, mixerVolume / MIXER_SLIDER_CEIL);
                return normalizedValue;
            }
            set
            {
                float normalizedValue = value > 0 ? value : MIN_DECIBEL_VOLUME_THRESHOLD;
                float decibelsValue = Mathf.Log10(normalizedValue) * MIXER_SLIDER_CEIL;
                _audioMixer?.SetFloat(_volumeExposedKey, decibelsValue);
            }
        }

        public float Volume
        {
            get
            {
                if (!_isMuted)
                {
                    _currentVolume = MixerVolume;
                }

                return _currentVolume;
            }
            set
            {
                if (_isMuted) return;

                MixerVolume = _currentVolume = value;
            }
        }

        public bool Mute
        {
            get => _isMuted;
            set
            {
                _isMuted = value;
                if (_isMuted)
                {
                    MixerVolume = 0f;
                    return;
                }

                MixerVolume = _currentVolume;
            }
        }

        public MixerVolumeHandler(AudioMixer audioMixer, string volumeExposedKey)
        {
            _audioMixer = audioMixer;
            _volumeExposedKey = volumeExposedKey;
            _currentVolume = MixerVolume;
        }
    }
}