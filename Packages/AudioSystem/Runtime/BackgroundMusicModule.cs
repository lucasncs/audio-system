using System;
using System.Collections.Generic;
using Seven.AudioSystem.EventData;
using Seven.AudioSystem.Settings;
using UnityEngine;
using UnityEngine.Audio;

namespace Seven.AudioSystem
{
    internal class BackgroundMusicModule : IDisposable
    {
        private readonly HashSet<AudioEventHandler> _eventHandlers = new HashSet<AudioEventHandler>();
        private readonly AudioMixerGroup _audioMixerGroup;
        private readonly IAudioSourceProvider _audioSourceProvider;

        internal BackgroundMusicModule(
            IBackgroundMusicModuleSettings settings,
            IAudioSourceProvider audioSourceProvider)
        {
            _audioMixerGroup = settings.BackgroundMusicMixerData.AudioMixerGroup;
            _audioSourceProvider = audioSourceProvider;
        }

        internal AudioEventHandler PlayAudio(AAudioEventData audioEvent, bool isAdditive = false)
        {
            if (!isAdditive)
            {
                StopAllAudios();
            }

            AudioSource audioSource = _audioSourceProvider.Get();

            var handler = new AudioEventHandler(
                audioEvent,
                audioSource,
                audioEvent.AudioMixerOutput == null ? _audioMixerGroup : audioEvent.AudioMixerOutput);

            if (handler.State != AudioEventState.Ready)
            {
                _audioSourceProvider.Release(handler.AudioSource);
                return handler;
            }

            _eventHandlers.Add(handler);

            handler.Play();
            return handler;
        }

        internal void StopAllAudios()
        {
            if (_eventHandlers.Count == 0) return;

            foreach (AudioEventHandler handler in _eventHandlers)
            {
                handler.Stop();
                _audioSourceProvider.Release(handler.AudioSource);
                handler.Dispose();
            }

            _eventHandlers.Clear();
        }

        internal void SetAudiosVolume(float value)
        {
            if (_eventHandlers.Count == 0) return;

            foreach (AudioEventHandler handler in _eventHandlers)
            {
                float lerpValue = Mathf.InverseLerp(0, 1, value);
                handler.AudioSource.volume = Mathf.Lerp(0, handler.OriginalVolume, lerpValue);
            }
        }

        public void Dispose()
        {
            StopAllAudios();
        }
    }
}