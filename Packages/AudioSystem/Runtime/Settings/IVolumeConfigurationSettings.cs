using UnityEngine.Audio;

namespace Seven.AudioSystem.Settings
{
    internal interface IVolumeConfigurationSettings
    {
        MixerGroupData MainMixerData { get; }
        MixerGroupData AudioEffectsMixerData { get; }
        MixerGroupData BackgroundMusicMixerData { get; }
    }
}