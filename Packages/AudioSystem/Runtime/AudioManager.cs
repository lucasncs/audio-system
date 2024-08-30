using System;
using Seven.AudioSystem.EventData;
using Seven.AudioSystem.Settings;
using Seven.AudioSystem.SubSystems.ObjectPool;
using Seven.AudioSystem.SubSystems.SoftParenting;
using UnityEngine;

namespace Seven.AudioSystem
{
    public class AudioManager : IDisposable
    {
        private readonly AudioEffectsModule _audioEffectsModule;
        private readonly BackgroundMusicModule _backgroundMusicModule;

        private readonly IAudioSourceProvider _audioSourceProvider;
        private readonly SoftParentingController _softParentController;
        private readonly AudioServiceSettings _settings;
        private readonly VolumeConfiguration _volumeConfiguration;

        public VolumeConfiguration VolumeConfiguration => _volumeConfiguration;

        public AudioManager() : this(new AudioPoolController(AudioServiceSettings.GetInstance()))
        {
        }

        public AudioManager(IAudioSourceProvider audioSourceProvider)
        {
            _audioSourceProvider = audioSourceProvider;

            _settings = AudioServiceSettings.GetInstance();

            _softParentController = new SoftParentingController();

            _audioEffectsModule = new AudioEffectsModule(
                _settings,
                _audioSourceProvider,
                _softParentController);

            _backgroundMusicModule = new BackgroundMusicModule(
                _settings,
                _audioSourceProvider);

            _volumeConfiguration = new VolumeConfiguration(_settings);
        }

        public IReadOnlyAudioEventHandler PlayAudioEffect(AAudioEventData audioEvent, Transform objectToFollow = null)
        {
            return _audioEffectsModule.PlayAudio(audioEvent, objectToFollow);
        }

        public void StopAudioEffect(IReadOnlyAudioEventHandler readOnlyHandler)
        {
            _audioEffectsModule.StopAudio(readOnlyHandler);
        }

        public IReadOnlyAudioEventHandler PlayBackgroundMusic(AAudioEventData audioEvent, bool isAdditive = false)
        {
            return _backgroundMusicModule.PlayAudio(audioEvent, isAdditive);
        }

        public void SetAudioVolume(IReadOnlyAudioEventHandler readOnlyHandler, float volume)
        {
            if (!(readOnlyHandler is AudioEventHandler handler) || handler.State != AudioEventState.Playing) return;

            handler.SetVolume(volume);
        }

        public void StopAllBackgroundMusic()
        {
            _backgroundMusicModule.StopAllAudios();
        }

        public void Dispose()
        {
            _audioEffectsModule.Dispose();
            _backgroundMusicModule.Dispose();
            _audioSourceProvider.Dispose();
            _softParentController.Dispose();
            _volumeConfiguration.Dispose();
        }
    }
}