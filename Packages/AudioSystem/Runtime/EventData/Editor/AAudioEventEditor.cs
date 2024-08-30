#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Seven.AudioSystem.EventData.Editor
{
    [CustomEditor(typeof(AAudioEventData), true)]
    internal abstract class AAudioEventEditor : UnityEditor.Editor
    {
        [SerializeField] protected AudioSource _previewer;

        protected AAudioEventData _eventData;
        protected UnityEditor.Editor _previewerInspector;

        private string[] _ignoredFields;

        protected abstract string[] InitialFields();

        protected virtual void OnEnable()
        {
            _eventData = (AAudioEventData)target;
            _previewer = EditorUtility.CreateGameObjectWithHideFlags("[Audio Preview]", HideFlags.HideInHierarchy)
                .AddComponent<AudioSource>();
            _previewerInspector = CreateEditor(_previewer);

            var list = new List<string> { "m_Script" };
            list.AddRange(InitialFields());
            _ignoredFields = list.ToArray();
        }

        protected void OnDisable()
        {
            if (_previewer != null)
            {
                _previewer.Stop();
                DestroyImmediate(_previewer.gameObject);
            }

            DestroyImmediate(_previewerInspector);
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
            EditorGUILayout.BeginHorizontal("box");

            GUI.enabled = !_previewer.isPlaying;

            if (GUILayout.Button("Preview"))
            {
                _previewer.Stop();
                _eventData.Configure(_previewer);
                _previewer.Play();
            }

            if (GUILayout.Button("Preview w/ Delay"))
            {
                _previewer.Stop();
                _eventData.Configure(_previewer);
                _previewer.PlayDelayed(_eventData.DelaySeconds);
            }

            _previewer.spatialBlend = 0;

            GUI.enabled = _previewer.isPlaying;

            if (GUILayout.Button("Stop"))
            {
                _previewer.Stop();
            }

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();

            serializedObject.UpdateIfRequiredOrScript();

            DrawInspector();

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawInspector()
        {
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"), true);
            }

            DrawInitialFields();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Configurations", EditorStyles.boldLabel);

            DrawPropertiesExcluding(serializedObject, _ignoredFields);
        }

        private void DrawInitialFields()
        {
            string[] fields = InitialFields();
            for (var i = 0; i < fields.Length; i++)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(fields[i]), true);
            }
        }
    }
}
#endif