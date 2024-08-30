#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Seven.AudioSystem.Settings.Editor
{
    internal sealed class AudioSystemSettingsProvider : AssetSettingsProvider
    {
        public const string CONFIG_NAME = "com.seven.audio-system.settings";
        private const string SETTINGS_OBJECT_PATH = "Assets/Plugins/Seven/AudioSystem/Settings.asset";

        private string _searchContext;
        private VisualElement _rootElement;

        internal static AudioServiceSettings CurrentSettings
        {
            get => GetOrCreateSettings();
            set
            {
                if (value == null)
                {
                    EditorBuildSettings.RemoveConfigObject(CONFIG_NAME);
                }
                else
                {
                    EditorBuildSettings.AddConfigObject(CONFIG_NAME, value, overwrite: true);
                }
            }
        }

        private AudioSystemSettingsProvider() : base("Seven/Audio System", () => CurrentSettings)
        {
            CurrentSettings = FindAudioSystemSettings();
            keywords = GetSearchKeywordsFromGUIContentProperties<AudioServiceSettings>();
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _rootElement = rootElement;
            _searchContext = searchContext;
            base.OnActivate(searchContext, rootElement);
        }

        public override void OnGUI(string searchContext)
        {
            DrawCurrentSettingsGUI();
            EditorGUILayout.Space();
            base.OnGUI(searchContext);
        }

        private void DrawCurrentSettingsGUI()
        {
            EditorGUI.BeginChangeCheck();

            EditorGUI.indentLevel++;
            var settings = EditorGUILayout.ObjectField("Current Settings", CurrentSettings,
                typeof(AudioServiceSettings), allowSceneObjects: false) as AudioServiceSettings;
            if (settings)
            {
                DrawValidSettingsMessage();
            }

            EditorGUI.indentLevel--;

            bool newSettings = EditorGUI.EndChangeCheck();
            if (newSettings)
            {
                CurrentSettings = settings;
                RefreshEditor();
            }
        }

        private void RefreshEditor() => base.OnActivate(_searchContext, _rootElement);

        private void DrawValidSettingsMessage()
        {
            const string message = "The current Audio System Settings will be automatically included into any builds.";
            EditorGUILayout.HelpBox(message, MessageType.Info, wide: true);
        }

        private static AudioServiceSettings FindAudioSystemSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<AudioServiceSettings>(SETTINGS_OBJECT_PATH);
            if (settings == null)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(SETTINGS_OBJECT_PATH));
                settings = ScriptableObject.CreateInstance<AudioServiceSettings>();
                AssetDatabase.CreateAsset(settings, SETTINGS_OBJECT_PATH);
            }

            return settings;
        }

        internal static AudioServiceSettings GetOrCreateSettings()
        {
            if (!EditorBuildSettings.TryGetConfigObject(CONFIG_NAME, out AudioServiceSettings settings))
                settings = FindAudioSystemSettings();
            return settings;
        }

        [SettingsProvider]
        private static SettingsProvider CreateProjectSettingsMenu() => new AudioSystemSettingsProvider();
    }
}
#endif