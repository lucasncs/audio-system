using System;
using UnityEngine;

namespace Seven.AudioSystem.SubSystems
{
    [AddComponentMenu("")]
    public class MonoBehaviourEventListener : MonoBehaviour, IDisposable
    {
        public event Action OnBehaviourEnable;
        public event Action OnBehaviourDisable;
        public event Action OnBehaviourDestroy;

        private void OnEnable()
        {
            OnBehaviourEnable?.Invoke();
        }

        private void OnDisable()
        {
            OnBehaviourDisable?.Invoke();
        }

        private void OnDestroy()
        {
            OnBehaviourDestroy?.Invoke();
        }

        public void Dispose()
        {
            OnBehaviourEnable = null;
            OnBehaviourDisable = null;
            OnBehaviourDestroy = null;
        }
    }
}