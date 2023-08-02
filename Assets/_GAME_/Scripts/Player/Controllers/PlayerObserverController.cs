using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace OL.Game {
    public class PlayerObserverController : PlayerControllerBase {
        #region editor
        #endregion

        #region public events
        #endregion

        #region public properties
        public PlayerUIController UIController => _UIController;
        public PlayerFXController FXController => _FXController;
        public PlayerInputController InputController => _inputController;
        public PlayerStateController StateController => _stateController;
        public PlayerCameraController CameraController => _cameraController;
        public PlayerRagdollController RagdollController => _ragdollController;
        public PlayerSlowMotionController SlowMotionController => _slowMotionController;

        public GameController GameController => _gameController;
        public LocalController LocalController => _localController;
        #endregion

        private GameController _gameController = default;
        private LocalController _localController = default;

        #region protected properties
        #endregion

        #region private
        private void initializeComponents() {
            _gameController = GameController.Instance;
            _localController = LocalController.Instance;
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

            foreach (PlayerControllerBase controller in _controllers) {
                if (controller != this && controller != null) {
                    controller.activate();
                }
            }
        }

        public override void deactivate() {
            base.deactivate();

            foreach (PlayerControllerBase controller in _controllers) {
                if (controller != this && controller != null) {
                    controller.deactivate();
                }
            }
        }
        #endregion
    }
}
