using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using OL.Kit.Components;

namespace OL.Game {
    public class PlayerControllerBase : OLMonoBehaviour {
        #region editor
        [Header("Debug settings"), Space(10)]
        [SerializeField] protected bool _DEBUG = false;
        #endregion

        #region public events
        #endregion

        #region public properties
        public bool IsActive => _isActive;
        public Player Player => _player;
        public PlayerObserverController ObserverController => _observerController;
        #endregion

        #region protected properties
        #endregion

        protected bool _isActive = false;

        protected Player _player = default;

        protected PlayerUIController _UIController = default;
        protected PlayerFXController _FXController = default;
        protected PlayerInputController _inputController = default;
        protected PlayerStateController _stateController = default;
        protected PlayerCameraController _cameraController = default;
        protected PlayerRagdollController _ragdollController = default;
        protected PlayerSlowMotionController _slowMotionController = default;

        protected PlayerObserverController _observerController = default;

        protected List<PlayerControllerBase> _controllers = default;

        #region private
        private void initializeComponents() {
            _player = GetComponentInParent<Player>();

            _controllers = _player.GetComponentsInChildren<PlayerControllerBase>().ToList();

            _UIController = (PlayerUIController)_controllers.FirstOrDefault(c => c is PlayerUIController);
            _FXController = (PlayerFXController)_controllers.FirstOrDefault(c => c is PlayerFXController);
            _inputController = (PlayerInputController)_controllers.FirstOrDefault(c => c is PlayerInputController);
            _stateController = (PlayerStateController)_controllers.FirstOrDefault(c => c is PlayerStateController);
            _cameraController = (PlayerCameraController)_controllers.FirstOrDefault(c => c is PlayerCameraController);
            _ragdollController = (PlayerRagdollController)_controllers.FirstOrDefault(c => c is PlayerRagdollController);
            _slowMotionController = (PlayerSlowMotionController)_controllers.FirstOrDefault(c => c is PlayerSlowMotionController);

            _observerController = (PlayerObserverController)_controllers.FirstOrDefault(c => c is PlayerObserverController);
        }

        private void initializeLocalSettings() {
            
        }
        #endregion

        #region protected
        protected override void awakeVirtual() {
            base.awakeVirtual();

            initializeComponents();
        }

        protected override void startVirtual() {
            base.startVirtual();

            initializeLocalSettings();
        }
        #endregion

        #region public
        public virtual void activate() {
            _isActive = true;
        }

        public virtual void deactivate() {
            _isActive = false;
        }
        #endregion
    }
}
