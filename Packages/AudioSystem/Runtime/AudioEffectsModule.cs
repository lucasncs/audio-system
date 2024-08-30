using System;
using System.Collections.Generic;
using Seven.AudioSystem.EventData;
using Seven.AudioSystem.Settings;
using Seven.AudioSystem.SubSystems.SoftParenting;
using UnityEngine;
using UnityEngine.Audio;

namespace Seven.AudioSystem
{
    internal class AudioEffectsModule : IDisposable
    {
        private readonly HashSet<AudioEventHandler> _eventHandlers = new HashSet<AudioEventHandler>();
        private readonly AudioMixerGroup _audioMixerGroup;
        private readonly IAudioSourceProvider _audioSourceProvider;
        private readonly SoftParenting _softParentSystem;

        internal AudioEffectsModule(
            IAudioEffectsModuleSettings settings,
            IAudioSourceProvider audioSourceProvider,
            SoftParentingController softParentController)
        {
            _audioMixerGroup = settings.AudioEffectsMixerData.AudioMixerGroup;
            _audioSourceProvider = audioSourceProvider;
            _softParentSystem = softParentController.SoftParent;
        }

        internal AudioEventHandler PlayAudio(AAudioEventData audioEvent, Transform objectToFollow = null)
        {
            AudioSource audioSource = _audioSourceProvider.Get();
            bool shouldFollow = objectToFollow != null;

            var handler = new AudioEventHandler(
                audioEvent,
                audioSource,
                audioEvent.AudioMixerOutput == null ? _audioMixerGroup : audioEvent.AudioMixerOutput,
                UnregisterHandler,
                objectToFollow);

            if (handler.State != AudioEventState.Ready)
            {
                _audioSourceProvider.Release(handler.AudioSource);
                return handler;
            }

            if (shouldFollow)
            {
                _softParentSystem.Register(handler.AudioSourceTransform, objectToFollow);
            }

            _eventHandlers.Add(handler);

            handler.Play();
            return handler;
        }

        internal void StopAudio(IReadOnlyAudioEventHandler readOnlyHandler)
        {
            if (!(readOnlyHandler is AudioEventHandler handler) ||
                !_eventHandlers.Contains(handler) ||
                handler.State != AudioEventState.Playing) return;

            handler.Stop();
            UnregisterHandler(handler);
            handler.Dispose();
        }

        private void UnregisterHandler(AudioEventHandler handler)
        {
            _audioSourceProvider.Release(handler.AudioSource);

            if (handler.IsFollowing)
            {
                _softParentSystem.Unregister(handler.AudioSourceTransform);
            }

            _eventHandlers.Remove(handler);
        }

        private void StopAllAudios()
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

        public void Dispose()
        {
            StopAllAudios();
            _eventHandlers.Clear();
        }
    }
}