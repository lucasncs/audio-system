using Seven.AudioSystem.SubSystems.CustomAttributes.RangedNumber.Float;
using UnityEngine;

namespace Seven.AudioSystem.EventData
{
    [CreateAssetMenu(menuName = "Audio Events/Simple")]
    public class SimpleAudioEventData : AAudioEventData
    {
        [SerializeField] protected AudioClip[] _clips;
        [SerializeField] protected bool _byPassEffects;
        [SerializeField] protected bool _byPassListenerEffects;
        [SerializeField] protected bool _byPassReverbZones;
        [SerializeField] protected bool _loop;
        [Range(0, 255)] [SerializeField] protected int _priority;
        [Range(0, 1)] [SerializeField] protected float _volume = 1;
        [MinMaxRangeFloat(0, 2)] [SerializeField] protected RangedFloat _pitch = new RangedFloat(.95f, 1.05f);
        [Range(-1, 1)] [SerializeField] protected float _stereoPan;
        [Range(0, 1f)] [SerializeField] protected float _spatialBlend;
        [Range(0, 1.1f)] [SerializeField] protected float _reverbZoneMix;
        [HideInInspector] [SerializeField] protected float _dopplerLevel;
        [HideInInspector] [SerializeField] protected float _spread;
        [HideInInspector] [SerializeField] protected AudioRolloffMode _volumeRolloff;
        [HideInInspector] [SerializeField] protected float _minDistance;
        [HideInInspector] [SerializeField] protected float _maxDistance;
        [HideInInspector] [SerializeField] private AnimationCurve _spreadAnimationCurve;
        [HideInInspector] [SerializeField] private AnimationCurve _customRolloffAnimationCurve;
        [HideInInspector] [SerializeField] private AnimationCurve _spatialBlendAnimationCurve;
        [HideInInspector] [SerializeField] private AnimationCurve _reverbZoneMixAnimationCurve;

        public bool ByPassEffects => _byPassEffects;
        public bool ByPassListenerEffects => _byPassListenerEffects;
        public bool ByPassReverbZones => _byPassReverbZones;
        public bool Loop => _loop;
        public int Priority => _priority;
        public float Volume => _volume;
        public RangedFloat Pitch => _pitch;
        public float StereoPan => _stereoPan;
        public float SpatialBlend => _spatialBlend;
        public float ReverbZoneMix => _reverbZoneMix;
        public float DopplerLevel => _dopplerLevel;
        public float Spread => _spread;
        public AudioRolloffMode VolumeRolloff => _volumeRolloff;
        public float MinDistance => _minDistance;
        public float MaxDistance => _maxDistance;
        public AnimationCurve SpreadAnimationCurve => _spreadAnimationCurve;
        public AnimationCurve CustomRolloffAnimationCurve => _customRolloffAnimationCurve;
        public AnimationCurve SpatialBlendAnimationCurve => _spatialBlendAnimationCurve;
        public AnimationCurve ReverbZoneMixAnimationCurve => _reverbZoneMixAnimationCurve;

        public override void Configure(AudioSource source)
        {
            if (_clips == null || _clips.Length < 1)
            {
                Debug.LogError(
                    $"[{nameof(AudioSystem)}] The audio event \"{name}\" is missing the audio clip reference", this);
                return;
            }

            int randomIndex = Random.Range(0, _clips.Length);

            source.clip = _clips[randomIndex];

            source.volume = Volume;
            source.pitch = Pitch.RandomValue();
            source.loop = Loop;
            source.minDistance = MinDistance;
            source.maxDistance = MaxDistance;
            source.dopplerLevel = DopplerLevel;
            source.mute = false;
            source.bypassEffects = ByPassEffects;
            source.bypassListenerEffects = ByPassListenerEffects;
            source.bypassReverbZones = ByPassReverbZones;
            source.playOnAwake = false;
            source.priority = Priority;
            source.spatialBlend = SpatialBlend;
            source.reverbZoneMix = ReverbZoneMix;
            source.rolloffMode = VolumeRolloff;

            Setup3DAudioCurvesOnSource(source);
        }

        internal void Setup3DAudioCurvesOnSource(AudioSource source)
        {
            if (SpreadAnimationCurve == null
                || CustomRolloffAnimationCurve == null
                || SpatialBlendAnimationCurve == null
                || ReverbZoneMixAnimationCurve == null)
            {
                return;
            }

            if (CurveHasKeys(SpreadAnimationCurve))
                source.SetCustomCurve(AudioSourceCurveType.Spread, SpreadAnimationCurve);
            if (CurveHasKeys(CustomRolloffAnimationCurve))
                source.SetCustomCurve(AudioSourceCurveType.CustomRolloff, CustomRolloffAnimationCurve);
            if (CurveHasKeys(SpatialBlendAnimationCurve))
                source.SetCustomCurve(AudioSourceCurveType.SpatialBlend, SpatialBlendAnimationCurve);
            if (CurveHasKeys(ReverbZoneMixAnimationCurve))
                source.SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, ReverbZoneMixAnimationCurve);
        }

        private bool CurveHasKeys(AnimationCurve curve)
        {
            return curve.keys.Length > 0;
        }

        internal void Setup3DAudioCurvesOnEvent(AudioSource source)
        {
            _spreadAnimationCurve = source.GetCustomCurve(AudioSourceCurveType.Spread);
            _customRolloffAnimationCurve = source.GetCustomCurve(AudioSourceCurveType.CustomRolloff);
            _spatialBlendAnimationCurve = source.GetCustomCurve(AudioSourceCurveType.SpatialBlend);
            _reverbZoneMixAnimationCurve = source.GetCustomCurve(AudioSourceCurveType.ReverbZoneMix);
        }
    }
}