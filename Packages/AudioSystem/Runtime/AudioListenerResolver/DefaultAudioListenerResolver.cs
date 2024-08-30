using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Seven.AudioSystem.AudioListenerResolver
{
    internal class DefaultAudioListenerResolver : IAudioListenerResolver
    {
        private AudioListener _activeAudioListener;
        private AudioListener[] _cachedAudioListeners;

        public AudioListener ActiveListener
        {
            get
            {
                if (!_activeAudioListener.isActiveAndEnabled)
                {
                    _activeAudioListener = GetCachedActiveAudioListener() ?? GetActiveAudioListenerInScene();
                }

                return _activeAudioListener;
            }
        }

        internal DefaultAudioListenerResolver()
        {
            Debug.LogWarning(
                $"[{nameof(AudioSystem)}] You are currently using the default audio listener resolver that finds the audio listener through " +
                "the find objects calls, which is not ideal. To get a better performance in this behaviour, implement a custom " +
                $"{nameof(IAudioListenerResolver)} and pass it in the initialization call of the audioService");

            _activeAudioListener = GetActiveAudioListenerInScene();
        }

        private AudioListener GetCachedActiveAudioListener()
        {
            return Array.Find(_cachedAudioListeners,
                audioListener => audioListener != null && audioListener.isActiveAndEnabled);
        }

        private AudioListener GetActiveAudioListenerInScene()
        {
            _cachedAudioListeners = Object.FindObjectsOfType<AudioListener>();
            return GetCachedActiveAudioListener();
        }
    }
}