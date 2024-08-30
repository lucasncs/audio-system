using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Seven.AudioSystem.EventData;
using Seven.AudioSystem.SubSystems;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;

namespace Seven.AudioSystem
{
    public enum AudioEventState
    {
        Ready,
        Playing,
        Finished,
        Failed,
    }

    internal class AudioEventHandler : IReadOnlyAudioEventHandler, IDisposable
    {
        public readonly Transform ObjectToFollow;
        public readonly Transform AudioSourceTransform;
        public readonly AudioSource AudioSource;
        public readonly AAudioEventData EventData;
        internal readonly HashSet<Component> AdditionalComponents = new HashSet<Component>();

        private readonly MonoBehaviourEventListener _eventListener;
        private readonly Action<AudioEventHandler> _onAudioEndReached;
        private readonly CancellationTokenSource _cancellationToken;

        public float OriginalVolume { get; }
        public bool IsFollowing => ObjectToFollow != null;
        public AAudioEventData AudioEventData => EventData;
        public AudioEventState State { get; private set; }
        public float EventDurationInSeconds { get; }
        public float ClipLengthInSeconds { get; }
        public bool IsPlaying => State == AudioEventState.Playing;
        public bool IsLooping => AudioSource.loop;
        public event Action OnFinish;

        public AudioEventHandler()
        {
            State = AudioEventState.Finished;
            EventDurationInSeconds = ClipLengthInSeconds = 0;
        }

        public AudioEventHandler(
            AAudioEventData eventData,
            AudioSource audioSource,
            AudioMixerGroup mixerGroup,
            Action<AudioEventHandler> onAudioEndReached = null,
            Transform objectToFollow = null)
        {
            _cancellationToken = new CancellationTokenSource();
            _onAudioEndReached = onAudioEndReached;
            EventData = eventData;
            AudioSource = audioSource;
            AudioSourceTransform = AudioSource.gameObject.transform;
            AudioSource.outputAudioMixerGroup = mixerGroup;


            EventData.Configure(AudioSource);
            if (AudioSource.clip == null)
            {
                OnFailedState();
                return;
            }


            ObjectToFollow = objectToFollow;

            _eventListener = objectToFollow != null
                ? objectToFollow.gameObject.AddComponent<MonoBehaviourEventListener>()
                : null;

            if (_eventListener != null)
            {
                SetupBehaviourEventListener(_eventListener);
            }


            OriginalVolume = audioSource.volume;
            ClipLengthInSeconds = AudioSource.clip.length;
            EventDurationInSeconds = ClipLengthInSeconds + EventData.DelaySeconds;
            State = AudioEventState.Ready;
        }

        private void OnFailedState()
        {
            State = AudioEventState.Failed;
            OnFinish?.Invoke();
            FinishHandler();
        }

        private void SetupBehaviourEventListener(MonoBehaviourEventListener behaviourEventListener)
        {
            behaviourEventListener.OnBehaviourDisable += Disable;
            behaviourEventListener.OnBehaviourDestroy += Destroy;

            void Disable()
            {
                Stop();
                FinishHandler();
            }

            void Destroy()
            {
                Stop();
                FinishHandler();
            }
        }

        public void Play()
        {
            if (State != AudioEventState.Ready) return;

            if (AudioSource.clip == null) return;

            AudioSource.PlayDelayed(EventData.DelaySeconds);
            State = AudioEventState.Playing;

            if (!AudioSource.loop)
            {
                _ = WaitAudioFinish(_cancellationToken.Token);
            }
        }

        public void Stop()
        {
            if (State != AudioEventState.Playing) return;

            if (AudioSource != null) AudioSource.Stop();
            State = AudioEventState.Finished;
            OnFinish?.Invoke();
        }

        public void SetVolume(float volume)
        {
            if (State != AudioEventState.Playing) return;

            AudioSource.volume = volume;
        }

        private async Task WaitAudioFinish(CancellationToken cancellationToken)
        {
            await Task.Delay(Mathf.CeilToInt(ClipLengthInSeconds) * 1000, cancellationToken);

            Stop();
            FinishHandler();
        }

        private void FinishHandler()
        {
            _onAudioEndReached?.Invoke(this);
            Dispose();
        }

        public void Dispose()
        {
            State = AudioEventState.Finished;

            try
            {
                _cancellationToken.Cancel();
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            OnFinish = null;

            if (_eventListener != null)
            {
                _eventListener.Dispose();
                Object.Destroy(_eventListener);
            }

            foreach (Component component in AdditionalComponents)
            {
                Object.Destroy(component);
            }
            
            AdditionalComponents.Clear();
        }

        public override string ToString()
        {
            return $"Audio Event = \"{EventData.name}\"\n" +
                   $"Selected Clip = \"{AudioSource.clip.name}\"\n" +
                   $"Event Duration = {EventDurationInSeconds}\n" +
                   $"State = {State.ToString()}";
        }
    }
}