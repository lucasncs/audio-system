using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Seven.AudioSystem.SubSystems.ObjectPool
{
    public class AudioPoolController : IAudioSourceProvider, IDisposable
    {
        private readonly IObjectPool<AudioSource> _pool;
        private readonly Transform _poolParent;

        public AudioPoolController(IAudioPoolSettings audioPoolSettings)
        {
            _poolParent = new GameObject($"[{nameof(AudioSystem)}] AudioSourcePool").transform;
            Object.DontDestroyOnLoad(_poolParent);

            _pool = new ObjectPool<AudioSource>(
                OnCreate,
                OnGet,
                OnRelease,
                OnDestroy,
                true,
                audioPoolSettings.InitialObjectPoolSize,
                audioPoolSettings.WarmUpPoolElements ? audioPoolSettings.InitialObjectPoolSize : 0,
                audioPoolSettings.MaxObjectPoolSize);
        }

        private AudioSource OnCreate()
        {
            var obj = new GameObject("AudioSource")
            {
                transform =
                {
                    parent = _poolParent
                }
            };
            obj.SetActive(false);
            return obj.AddComponent<AudioSource>();
        }

        private void OnGet(AudioSource audioSource)
        {
            audioSource.gameObject.SetActive(true);
        }

        private void OnRelease(AudioSource audioSource)
        {
            if (audioSource == null) return;

            audioSource.Stop();
            audioSource.clip = null;
            audioSource.gameObject.SetActive(false);
        }

        private void OnDestroy(AudioSource audioSource)
        {
            if (audioSource == null) return;

            Object.Destroy(audioSource.gameObject);
        }

        public AudioSource Get()
        {
            return _pool.Get();
        }

        public void Release(AudioSource audioSource)
        {
            _pool.Release(audioSource);
        }

        public void Dispose()
        {
            _pool.Clear();

            if (_poolParent != null)
            {
                Object.Destroy(_poolParent.gameObject);
            }
        }
    }
}