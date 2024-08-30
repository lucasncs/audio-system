using UnityEngine;

namespace Seven.AudioSystem.AudioListenerResolver
{
    public interface IAudioListenerResolver
    {
        AudioListener ActiveListener { get; }
    }
}