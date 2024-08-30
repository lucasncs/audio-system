using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Seven.AudioSystem.SubSystems.SoftParenting
{
    public class SoftParentingController : IDisposable
    {
        public readonly SoftParenting SoftParent;

        public SoftParentingController()
        {
            SoftParent = new GameObject($"[{nameof(AudioSystem)}] SoftParenting").AddComponent<SoftParenting>();
            Object.DontDestroyOnLoad(SoftParent);
        }

        public void Dispose()
        {
            if (SoftParent == null) return;

            SoftParent.ClearAll();
            Object.Destroy(SoftParent.gameObject);
        }
    }
}