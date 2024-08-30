#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;

namespace Seven.AudioSystem.EventData.Editor
{
    [CustomEditor(typeof(SimpleAudioEventData), true)]
    internal class SimpleAudioEventEditor : AAudioEventEditor
    {
        private SimpleAudioEventData _simpleEventData;
        private bool _audioSource3DSettingsFoldout;

        protected override void OnEnable()
        {
            base.OnEnable();
            _simpleEventData = (SimpleAudioEventData)_eventData;
        }

        protected override void DrawInspector()
        {
            base.DrawInspector();

            EditorGUILayout.Space();
            _audioSource3DSettingsFoldout = EditorGUILayout.Foldout(_audioSource3DSettingsFoldout, "3D Audio Settings");
            if (_audioSource3DSettingsFoldout)
            {
                EditorGUI.indentLevel++;
                DrawAudioSource3DSettings();
                EditorGUI.indentLevel--;
            }
        }

        private void DrawAudioSource3DSettings()
        {
            _simpleEventData.Setup3DAudioCurvesOnSource(_previewer);
            _previewer.dopplerLevel = _simpleEventData.DopplerLevel;
            _previewer.spread = _simpleEventData.Spread;
            _previewer.rolloffMode = _simpleEventData.VolumeRolloff;
            _previewer.minDistance = _simpleEventData.MinDistance;
            _previewer.maxDistance = _simpleEventData.MaxDistance;
            _previewer.spatialBlend = _simpleEventData.SpatialBlend;
            _previewer.reverbZoneMix = _simpleEventData.ReverbZoneMix;

            _previewerInspector.serializedObject.Update();

            _previewerInspector.GetType().GetMethod("Audio3DGUI", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.Invoke(_previewerInspector, null);

            _previewerInspector.serializedObject.ApplyModifiedProperties();

            serializedObject.FindProperty("_dopplerLevel").floatValue = _previewer.dopplerLevel;
            serializedObject.FindProperty("_spread").floatValue = _previewer.spread;
            serializedObject.FindProperty("_volumeRolloff").enumValueIndex = (int)_previewer.rolloffMode;
            serializedObject.FindProperty("_minDistance").floatValue = _previewer.minDistance;
            serializedObject.FindProperty("_maxDistance").floatValue = _previewer.maxDistance;
            serializedObject.FindProperty("_spatialBlend").floatValue = _previewer.spatialBlend;
            serializedObject.FindProperty("_reverbZoneMix").floatValue = _previewer.reverbZoneMix;
            _simpleEventData.Setup3DAudioCurvesOnEvent(_previewer);

            EditorUtility.SetDirty(_simpleEventData);
        }

        protected override string[] InitialFields()
        {
            return new[] { "_clips" };
        }
    }
}
#endif