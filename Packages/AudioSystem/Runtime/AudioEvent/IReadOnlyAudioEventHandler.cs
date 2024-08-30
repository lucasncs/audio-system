using System;
using Seven.AudioSystem.EventData;

namespace Seven.AudioSystem
{
    public interface IReadOnlyAudioEventHandler
    {
        AAudioEventData AudioEventData { get; }
        AudioEventState State { get; }
        float EventDurationInSeconds { get; }
        float ClipLengthInSeconds { get; }
        bool IsPlaying { get; }
        bool IsLooping { get; }
        event Action OnFinish;
    }
}