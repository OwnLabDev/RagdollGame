using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace OL.Game {
    public class PlayerSlowMotionController : PlayerControllerBase {
        #region editor
        #endregion

        #region public events
        #endregion

        #region public properties
        public PlayerSlowMotionSettings Settings => _settings;
        #endregion

        private PlayerSlowMotionSettings _settings = default;

        #region private
        private void Update() {
            if (_DEBUG) {
                MoreMountains.Tools.MMDebug.DebugOnScreen($"TimeScale: {Time.timeScale}");
            }
        }

        private void initializeComponents() {
            _settings = _player.Settings.SlowMotionSettings;
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

            resetSlowMotion();
        }

        public override void deactivate() {
            base.deactivate();

            resetSlowMotion();
        }

        public void hitSlowMotion() {
            if (!_settings.UseSlowMotionOnHit) {
                return;
            }

            _observerController.LocalController.SlowMotionManager.slowMotion(_settings.SlowMotionOnHitData);
        }

        public void resetSlowMotion() {
            _observerController.LocalController.SlowMotionManager.slowMotion(_settings.SlowMotionResetData);
        }
        #endregion
    }
}
