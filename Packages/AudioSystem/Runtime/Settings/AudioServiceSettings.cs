using Seven.AudioSystem.SubSystems.ObjectPool;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace Seven.AudioSystem.Settings
{
    internal class AudioServiceSettings : ScriptableObject,
        IAudioPoolSettings,
        IVolumeConfigurationSettings, 
        IAudioEffectsModuleSettings,
        IBackgroundMusicModuleSettings
    {
        private static AudioServiceSettings _instance;

        [Header("Default Audio Mixer Settings")]
        [SerializeField] private MixerGroupData _mainMixerData;
        [SerializeField] private MixerGroupData _audioEffectsMixerData;
        [SerializeField] private MixerGroupData _backgroundMusicMixerData;

        public MixerGroupData MainMixerData => _mainMixerData;
        public MixerGroupData AudioEffectsMixerData => _audioEffectsMixerData;
        public MixerGroupData BackgroundMusicMixerData => _backgroundMusicMixerData;


        [Header("Audio Object Pool Settings")]
        [SerializeField] private int _initialObjectPoolSize = 40;
        [SerializeField] private int _maxObjectPoolSize = 60;
        [SerializeField] private bool _warmUpPoolElements = true;

        public int InitialObjectPoolSize => _initialObjectPoolSize;
        public int MaxObjectPoolSize => _maxObjectPoolSize;
        public bool WarmUpPoolElements => _warmUpPoolElements;

        private void OnEnable()
        {
            if (_instance == null)
            {
                _instance = this;
            }
        }

        public static AudioServiceSettings GetInstance()
        {
            if (_instance) return _instance;
#if UNITY_EDITOR
            _instance = Editor.AudioSystemSettingsProvider.GetOrCreateSettings();
#else
            _instance = FindObjectOfType<AudioServiceSettings>();
#endif
            return _instance;
        }
    }
}