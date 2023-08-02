using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using OL.Kit.Utility;

namespace OL.Game {
    public class PlayerCameraController : PlayerControllerBase {
        #region editor
        [NaughtyAttributes.HorizontalLine]

        [Space(10), Header("PlayerCameraController settings")]
        [SerializeField] private CameraManager.VCAMType _activeVCAM = CameraManager.VCAMType.DefaultView;
        [SerializeField] private CameraManager.VCAMType _inactiveVCAM = CameraManager.VCAMType.DeathView;
        #endregion

        #region public events
        #endregion

        #region public properties
        public PlayerCameraSettings Settings => _settings;
        public VCAMController VCAMController => _vcam;
        #endregion

        private PlayerCameraSettings _settings = default;

        private VCAMController _vcam = default;

        #region private
        private void initializeComponents() {
            _settings = _player.Settings.CameraSettings;

            _vcam = FindObjectOfType<VCAMController>();            
        }
        #endregion

        #region protected
        protected override void awakeVirtual() {
            base.awakeVirtual();

            initializeComponents();
        }
        #endregion

        #region public
        public override void activate() {
            base.activate();

            switchToActiveVCAM();
        }

        public override void deactivate() {
            base.deactivate();

            switchToInactiveVCAM();
        }

        public void switchToVCAM(CameraManager.VCAMType vcam) {
            _vcam.switchToVcam(vcam);

            _vcam.CameraManager.setTarget(_ragdollController.RagdollAnimator.transform);
        }

        public void switchToActiveVCAM() {
            switchToVCAM(_activeVCAM);
        }

        public void switchToInactiveVCAM() {
            switchToVCAM(_inactiveVCAM);
        }

        public void zoom() {
            _vcam.CameraManager.zoomOutBack(_settings.CameraZoom, _settings.CameraZoomDuration);
        }
        #endregion
    }
}
