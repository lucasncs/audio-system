#if UNITY_EDITOR
using UnityEditor;

namespace Seven.AudioSystem.EventData.Editor
{
    [CustomEditor(typeof(CompositeAudioEventData), true)]
    internal class CompositeAudioEventEditor : AAudioEventEditor
    {
        protected override string[] InitialFields()
        {
            return new[] { "_entries" };
        }
    }
}
#endif