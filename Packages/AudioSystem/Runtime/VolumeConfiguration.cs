using System;
using Seven.AudioSystem.Settings;

namespace Seven.AudioSystem
{
    public class VolumeConfiguration : IDisposable
    {
        private MixerVolumeHandler _generalMixerHandler;
        private MixerVolumeHandler _audioEffectsMixerHandler;
        private MixerVolumeHandler _backgroundMusicMixerHandler;

        public float AudioEffectsVolume
        {
            get => _audioEffectsMixerHandler.Volume;
            set => _audioEffectsMixerHandler.Volume = value;
        }

        public float BackgroundAudioVolume
        {
            get => _backgroundMusicMixerHandler.Volume;
            set => _backgroundMusicMixerHandler.Volume = value;
        }

        public bool MuteBackgroundMusic
        {
            get => _backgroundMusicMixerHandler.Mute;
            set => _backgroundMusicMixerHandler.Mute = value;
        }

        public bool MuteAudioEffects
        {
            get => _audioEffectsMixerHandler.Mute;
            set => _audioEffectsMixerHandler.Mute = value;
        }

        public float GeneralVolume
        {
            get => _generalMixerHandler.Volume;
            set => _generalMixerHandler.Volume = value;
        }

        public bool Mute
        {
            get => _generalMixerHandler.Mute;
            set => _generalMixerHandler.Mute = value;
        }

        internal VolumeConfiguration(IVolumeConfigurationSettings settings)
        {
            _generalMixerHandler = new MixerVolumeHandler(
                settings.MainMixerData.AudioMixerGroup
                    ? settings.MainMixerData.AudioMixerGroup.audioMixer
                    : null,
                settings.MainMixerData.VolumeExposedKey);

            _audioEffectsMixerHandler = new MixerVolumeHandler(
                settings.AudioEffectsMixerData.AudioMixerGroup
                    ? settings.AudioEffectsMixerData.AudioMixerGroup.audioMixer
                    : null,
                settings.AudioEffectsMixerData.VolumeExposedKey);

            _backgroundMusicMixerHandler = new MixerVolumeHandler(
                settings.BackgroundMusicMixerData.AudioMixerGroup
                    ? settings.BackgroundMusicMixerData.AudioMixerGroup.audioMixer
                    : null,
                settings.BackgroundMusicMixerData.VolumeExposedKey);
        }

        public void Dispose()
        {
            _generalMixerHandler = null;
            _audioEffectsMixerHandler = null;
            _backgroundMusicMixerHandler = null;
        }
    }
}