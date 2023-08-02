using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Cinemachine;

using DG.Tweening;

using VCAM = Cinemachine.CinemachineVirtualCamera;

namespace OL.Game {
    public class CameraManager : Manager {
        public enum ZoomType { Default, ZoomOut, ZoomBack, ZoomOutBack }

        public enum VCAMType {
            InitialView,
            DefaultView,
            DeathView,
        }

        #region editor
        [SerializeField] private Camera _mainCamera = default;
        [SerializeField] private VCAMDictionary _vcams = default;
        [SerializeField] private Transform _localVcamTarget = default;
        #endregion

        #region public properties
        public VCAM Vcam => _currentVcam;
        public VCAMType VcamType => _vcams.FirstOrDefault(kpv => kpv.Value == _currentVcam).Key;
        public Camera MainCamera => _mainCamera;
        public Transform LocalVcamTarget => _localVcamTarget;
        #endregion

        private VCAM _currentVcam = default;

        private Dictionary<VCAMType, float> _vcamsFOV = new Dictionary<VCAMType, float>();

        private Sequence _zoomSequence = default;
        private Coroutine _shakeCoroutine = default;

        #region private
        private void resetVcam() {
            resetFOV();
            resetPerlin();

            _localVcamTarget.localPosition = Vector3.zero;
            _localVcamTarget.localRotation = Quaternion.identity;
        }

        private void resetFOV() {
            VCAMType vCAMType = _vcams.First(vcam => vcam.Value == _currentVcam).Key;
            _currentVcam.m_Lens.FieldOfView = _vcamsFOV[vCAMType];
        }

        private void resetPerlin() {
            if (_currentVcam == null) {
                return;
            }

            if (_currentVcam.TryGetComponent(out VCAMCameraSlot cameraSlot) && cameraSlot.ResetPerlin) {
                CinemachineBasicMultiChannelPerlin perlin = _currentVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                if (perlin != null) {
                    perlin.m_AmplitudeGain = 0f;
                    perlin.m_FrequencyGain = 0f;
                }
            }
        }

        private IEnumerator shakeCoroutine(float duration, float amplitude, float frequency, bool damping) {
            CinemachineBasicMultiChannelPerlin perlin = _currentVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = amplitude;
            perlin.m_FrequencyGain = frequency;

            if (damping) {
                float elapsed = 0f;
                float elapsedStep = Time.deltaTime / duration;
                float localAmplitude = amplitude;
                float amplitudeStep = elapsedStep * amplitude;
                float localFrequency = frequency;
                float frequencyStep = elapsedStep * frequency;

                while (elapsed < 1f) {
                    perlin.m_AmplitudeGain = localAmplitude;
                    perlin.m_FrequencyGain = localFrequency;

                    elapsed += elapsedStep;
                    localAmplitude -= amplitudeStep;
                    localFrequency -= frequencyStep;

                    yield return null;
                }
            } else {
                yield return new WaitForSeconds(duration);
            }

            resetPerlin();
        }
        #endregion

        #region public
        public override void initializeVirtual() {
            base.initializeVirtual();

            if (_vcams.Count > 0) {
                _currentVcam = _vcams.First().Value;

                foreach (VCAMType vcamType in _vcams.Keys) {
                    _vcamsFOV.Add(vcamType, _vcams[vcamType].m_Lens.FieldOfView);
                }

                resetPerlin();
            }

            CurrentStatus = Status.Ready;
        }

        public void initializeVcam(VCAMType vcamType, VCAM vcam) {
            Debug.Log(vcamType, vcam);

            if (_vcams.ContainsKey(vcamType)) {
                _vcams[vcamType] = vcam;
                _vcamsFOV[vcamType] = _vcams[vcamType].m_Lens.FieldOfView;
            } else {
                _vcams.Add(vcamType, vcam);
                _vcamsFOV.Add(vcamType, _vcams[vcamType].m_Lens.FieldOfView);
            }

            resetPerlin();
        }

        public void initializeVcams(VCAMDictionary vcams) {
            foreach (KeyValuePair<VCAMType, VCAM> keyValuePair in vcams) {
                if (_vcams.ContainsKey(keyValuePair.Key)) {
                    _vcams[keyValuePair.Key] = keyValuePair.Value;
                    _vcamsFOV[keyValuePair.Key] = _vcams[keyValuePair.Key].m_Lens.FieldOfView;
                } else {
                    _vcams.Add(keyValuePair.Key, keyValuePair.Value);
                    _vcamsFOV.Add(keyValuePair.Key, _vcams[keyValuePair.Key].m_Lens.FieldOfView);
                }

                _vcams[keyValuePair.Key].enabled = false;
            }

            resetPerlin();
        }

        public void removeVcam(VCAMType vcamType) {
            if (_vcams.ContainsKey(vcamType)) {
                _vcams.Remove(vcamType);
                _vcamsFOV.Remove(vcamType);
            }
        }

        //public void setTarget(Transform target) {
        //    _currentVcam.Follow = target;
        //    _currentVcam.LookAt = target;
        //}

        public void setTarget(Transform target) {
            foreach (VCAM vcam in _vcams.Values) {
                vcam.Follow = target;
                vcam.LookAt = target;
            }

            _currentVcam.Follow = target;
            _currentVcam.LookAt = target;
        }

        public void setFollowTarget(Transform target) {
            foreach (VCAM vcam in _vcams.Values) {
                vcam.Follow = target;
            }
        }

        public void setLookAtTarget(Transform target) {
            foreach (VCAM vcam in _vcams.Values) {
                vcam.LookAt = target;
            }
        }

        public void shake(float duration, float amplitude, float frequency, bool damping = false) {
            if (_shakeCoroutine != null) {
                StopCoroutine(_shakeCoroutine);
            }

            _shakeCoroutine = StartCoroutine(shakeCoroutine(duration, amplitude, frequency, damping));
        }

        public void shakeOnce(float amplitude, float frequency, float duration = .5f, bool damping = false) {
            if (_shakeCoroutine != null) {
                StopCoroutine(_shakeCoroutine);
            }

            _shakeCoroutine = StartCoroutine(shakeCoroutine(duration, amplitude, frequency, damping));
        }

        public void zoomOut(float delta, float duration) {
            DOTween.To(_ => _currentVcam.m_Lens.FieldOfView = _, _currentVcam.m_Lens.FieldOfView, _currentVcam.m_Lens.FieldOfView + delta, duration);
        }

        public void zoomBack(float duration) {
            VCAMType vCAMType = _vcams.First(vcam => vcam.Value == _currentVcam).Key;
            DOTween.To(_ => _currentVcam.m_Lens.FieldOfView = _, _currentVcam.m_Lens.FieldOfView, _vcamsFOV[vCAMType], duration);
        }

        public void zoomOutBack(float delta, float duration) {
            if (_zoomSequence.IsActive()) {
                return;
            }

            if (_zoomSequence != null) {
                _zoomSequence.Kill();
            }

            float localCachedFOV = _currentVcam.m_Lens.FieldOfView;

            _zoomSequence = DOTween.Sequence();
            _zoomSequence.Append(DOTween.To(() => _currentVcam.m_Lens.FieldOfView, _ => _currentVcam.m_Lens.FieldOfView = _, localCachedFOV + delta, duration / 2f).SetEase(Ease.OutQuad));
            _zoomSequence.Append(DOTween.To(() => _currentVcam.m_Lens.FieldOfView, _ => _currentVcam.m_Lens.FieldOfView = _, localCachedFOV, duration / 2f).SetEase(Ease.Linear));
        }

        public void zoom(ZoomType zoomType, float delta, float duration) {
            switch (zoomType) {
                case ZoomType.ZoomOut: {
                    zoomOut(delta, duration);
                    break;
                }
                case ZoomType.ZoomBack: {
                    zoomBack(duration);
                    break;
                }
                case ZoomType.ZoomOutBack: {
                    zoomOutBack(delta, duration);
                    break;
                }
                default:
                break;
            }
        }

        public void switchToVcam(VCAMType vcamType, bool reset = true) {
            VCAM vcam = _vcams[vcamType];
            if (_currentVcam == vcam) {
                return;
            }            

            //Debug.Log($"Switch VCAM from: {currentType}, to: {vcamType}");

            if (_zoomSequence != null && _zoomSequence.IsActive()) {
                _zoomSequence.Kill();
            }

            if (_currentVcam != null) {
                _currentVcam.enabled = false;
            }

            _currentVcam = vcam;

            if (reset) {
                resetVcam();
            }

            _currentVcam.enabled = true;
        }

        public void switchToVcam(VCAMType vcamType, float duration, bool reset = true) {
            CinemachineBrain cinemachineBrain = _mainCamera.GetComponent<CinemachineBrain>();
            cinemachineBrain.m_DefaultBlend.m_Time = duration;

            switchToVcam(vcamType, reset);
            
        }

        public VCAM vcamByType(VCAMType vcamType) {
            if (_vcams.ContainsKey(vcamType)) {
                return _vcams[vcamType];
            }

            return null;
        }
        #endregion
    }
}