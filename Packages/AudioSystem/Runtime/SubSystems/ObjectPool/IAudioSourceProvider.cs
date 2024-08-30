using System;
using UnityEngine;

namespace Seven.AudioSystem
{
    public interface IAudioSourceProvider : IDisposable
    {
        AudioSource Get();
        void Release(AudioSource audioSource);
    }
}