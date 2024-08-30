using UnityEngine;

namespace Seven.AudioSystem.EventData
{
    [CreateAssetMenu(menuName = "Audio Events/Composite")]
    public class CompositeAudioEventData : AAudioEventData
    {
        [SerializeField] private AAudioEventData[] _entries;

        public override void Configure(AudioSource source)
        {
            if (_entries == null || _entries.Length == 0) return;

            int randomIndex = Random.Range(0, _entries.Length);

            _entries[randomIndex].Configure(source);
        }
    }
}