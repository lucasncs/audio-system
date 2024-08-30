#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using Object = UnityEngine.Object;

namespace Seven.AudioSystem.Settings.Editor
{
    internal sealed class AudioSystemSettingsBuildPlayer : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            List<Object> preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();

            AudioServiceSettings currentSettings = AudioSystemSettingsProvider.CurrentSettings;
            Type currentSettingsType = currentSettings.GetType();
            preloadedAssets.RemoveAll(asset => asset != null && asset.GetType() == currentSettingsType);
            preloadedAssets.Add(currentSettings);

            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
        }
    }
}
#endif