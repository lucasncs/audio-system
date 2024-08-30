using System.Collections.Generic;
using UnityEngine;

namespace Seven.AudioSystem.SubSystems.SoftParenting
{
    public class SoftParenting : MonoBehaviour
    {
        private readonly Dictionary<Transform, Transform> _parentingMap = new Dictionary<Transform, Transform>();
        private readonly List<Transform> _markedForCleanup = new List<Transform>();

        public void Register(Transform child, Transform parent)
        {
            _parentingMap[child] = parent;
        }

        public void Unregister(Transform child)
        {
            _parentingMap.Remove(child);
        }

        public void ClearAll()
        {
            _parentingMap.Clear();
            _markedForCleanup.Clear();
        }

        private void UpdateTransform()
        {
            foreach (KeyValuePair<Transform, Transform> parentingPair in _parentingMap)
            {
                if (parentingPair.Key == null || parentingPair.Value == null)
                {
                    _markedForCleanup.Add(parentingPair.Key);
                    continue;
                }

                parentingPair.Key.position = parentingPair.Value.position;
                parentingPair.Key.rotation = parentingPair.Value.rotation;
            }

            ExecuteCleanup();
        }

        private void ExecuteCleanup()
        {
            if (_markedForCleanup.Count < 1) return;

            foreach (Transform item in _markedForCleanup)
            {
                _parentingMap.Remove(item);
            }

            _markedForCleanup.Clear();
        }

        private void LateUpdate()
        {
            if (_parentingMap.Count < 1) return;

            UpdateTransform();
        }
    }
}