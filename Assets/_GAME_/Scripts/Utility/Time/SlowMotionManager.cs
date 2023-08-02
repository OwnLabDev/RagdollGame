using UnityEngine;

using DG.Tweening;

using OL.Game;
using OL.Kit.Utility;

namespace OL.Utility {
    [System.Serializable]
    public class SlowMotionData {
        public RandomizedFloat Duration = 1f;
        public RandomizedFloat StartValue = 0f;
        public RandomizedFloat EndValue = 1f;
        public Ease Ease = Ease.Linear;
        public RandomizedFloat Delay = 0f;

        [Space(10)]
        public bool UseInstantValue = false;
        [NaughtyAttributes.AllowNesting]
        [NaughtyAttributes.ShowIf(nameof(UseInstantValue))]
        public RandomizedFloat InstantValue = 0f;
    }

    public class SlowMotionManager : MonoBehaviour {
        #region editor
        [SerializeField] private float _fixedDeltaTime = .02f;

        [NaughtyAttributes.HorizontalLine]

        [SerializeField] private float _slowMotionDuration = 1f;
        [SerializeField] private float _startSlowMotionValue = .5f;
        [SerializeField] private Ease _slowMotionEase = Ease.Linear;
        #endregion

        #region public events
        #endregion

        #region public properties
        [NaughtyAttributes.ShowNativeProperty]
        public float TimeScale => Time.timeScale;
        [NaughtyAttributes.ShowNativeProperty]
        public float FixedDeltaTime => Time.fixedDeltaTime;
        #endregion

        private Tween _slowMotionTween = default;

        #region private
        private void Awake() {
            setTimeScale(1f);
        }
        #endregion

        #region public
        public void setTimeScale(float timeScale) {
            Time.timeScale = timeScale;
            Time.fixedDeltaTime = Time.timeScale * _fixedDeltaTime;
        }

        public void slowMotion(float duration = 1f, float startValue = 0f, float endValue = 1f, Ease ease = Ease.Linear, float delay = 0f) {
            if (_slowMotionTween != null) {
                _slowMotionTween.Kill();

                _slowMotionTween = null;
            }

            if (duration >= 0f) {
                float slowMotionValue = startValue;

                _slowMotionTween = DOTween.To(_ => slowMotionValue = _, startValue, endValue, duration)
                    .OnUpdate(() => {
                        setTimeScale(slowMotionValue);
                    })
                    .SetEase(ease)
                    .SetDelay(delay)
                    .SetUpdate(isIndependentUpdate: true);
            } else {
                setTimeScale(endValue);
            }
        }

        public void slowMotion() {
            slowMotion(_slowMotionDuration, _startSlowMotionValue, 1f, _slowMotionEase);
        }

        public void slowMotion(SlowMotionData data) {
            if (data.UseInstantValue) {
                setTimeScale(data.InstantValue.Value);
            }

            float startValue = data.StartValue.Value;
            if (data.StartValue.Value < 0f) {
                startValue = Time.timeScale;
            }

            slowMotion(data.Duration.Value, startValue, data.EndValue.Value, data.Ease, data.Delay.Value);
        }

        public void restoreSlowMotion(float duration = 1f, Ease ease = Ease.Linear, float delay = 0f) {
            slowMotion(duration, 0f, 1f, ease, delay);
        }

        public void restoreSlowMotion_1sec(Ease ease = Ease.Linear, float delay = 0f) {
            slowMotion(1f, 0f, 1f, ease, delay);
        }

        public void restoreSlowMotion_instantly(float delay = 0f) {
            slowMotion(-1f, 0f, 1f, Ease.Linear, delay);
        }
        #endregion
    }
}