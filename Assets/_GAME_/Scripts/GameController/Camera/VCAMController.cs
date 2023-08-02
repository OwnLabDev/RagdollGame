using System.Linq;

using UnityEngine;

using Cinemachine;

using MoreMountains.Tools;

using OL.Kit.Utility;

namespace OL.Game {
    public class VCAMController : MonoBehaviour {
        #region editor
        [Header("Settings")]
        [NaughtyAttributes.Expandable]
        [SerializeField] private CameraSettings _settings = default;

        [Space(10), Header("Initial camera settings")]
        [SerializeField] private CameraManager.VCAMType _initialVCAM = default;
        #endregion

        #region public properties
        public CameraManager CameraManager => _cameraManager;
        public CameraSettings Settings => _settings;
        #endregion

        private VCAMDictionary _vcams = default;

        private CameraManager _cameraManager = default;

        #region private
        private void Awake() {
            initializeComponents();
        }

        private void initializeComponents() {
            _vcams = new VCAMDictionary();

            foreach (VCAMCameraSlot cameraSlot in FindObjectsOfType<VCAMCameraSlot>()) {
                _vcams.Add(cameraSlot.VCAMType, cameraSlot.VCAM);
            }

            if (GameController.Instance != null) {
                _cameraManager = GameController.Instance.CameraManagerInstance;
                _cameraManager.initializeVcams(_vcams);
            }

            switchToVcam(_initialVCAM);
        }
        #endregion

        #region public
        public void switchToVcam(CameraManager.VCAMType vcamType) {
            if (_cameraManager != null) {
                _cameraManager.switchToVcam(vcamType);
            }
        }

        public void switchToVcam(CameraManager.VCAMType vcamType, float duration) {
            if (_cameraManager != null) {
                _cameraManager.switchToVcam(vcamType, duration);
            }
        }
        #endregion
    }
}
