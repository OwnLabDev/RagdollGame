using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using MoreMountains.Tools;
using DG.Tweening;
using System;

namespace OL.Game {
    public enum PlayerStateType {
        Active,
        Dead,
    }

    public class PlayerStateController : PlayerControllerBase {
        #region editor
        #endregion

        #region public events
        public event Action<PlayerStateType, PlayerStateType> OnStateChanged = default;
        #endregion

        #region public properties
        public float Health => _health;
        public PlayerStateType State => _state.Value;
        public PlayerStateSettings Settings => _settings;
        #endregion

        private float _health = 0f;

        private MMObservable<PlayerStateType> _state = default;

        private PlayerStateSettings _settings = default;

        #region private
        private void Update() {
            if (_DEBUG) {
                MMDebug.DebugOnScreen($"State: {_state.Value}");
            }
        }

        private void initializeComponents() {
            _settings = _player.Settings.StateSettings;            

            changeState(PlayerStateType.Active);
        }

        private void onStateChanged(PlayerStateType from, PlayerStateType to) {
            OnStateChanged?.Invoke(from, to);
        }

        private void death() {
            _observerController.deactivate();
        }
        #endregion

        #region protected
        protected override void awakeVirtual() {
            base.awakeVirtual();

            initializeComponents();
        }

        protected override void startVirtual() {
            base.startVirtual();

            heal(_settings.HealthMax);
        }

        protected override void enableVirtual() {
            base.enableVirtual();

            _state.OnValueChangedFromTo += onStateChanged;
        }

        protected override void disableVirtual() {
            base.disableVirtual();

            _state.OnValueChangedFromTo -= onStateChanged;
        }
        #endregion

        #region public
        public void changeState(PlayerStateType to) {
            PlayerStateType from = _state.Value;

            if (from == to) {
                return;
            }

            switch (to) {
                case PlayerStateType.Active: {

                    break;
                }

                case PlayerStateType.Dead: {
                    death();

                    break;
                }

                default: break;
            }

            _state.Value = to;
        }

        public void hit(float value) {
            if (_state.Value == PlayerStateType.Dead) {
                return;
            }

            if (_health >= 0f) {
                bool dieImmediatelly = value < 0f;

                float totalDamage = value;

                if (dieImmediatelly) {
                    totalDamage = _health + 1f;
                }

                _health -= totalDamage;

                _UIController.updateHealthBar(_health, 0f, _settings.HealthMax, true);

                if (_health <= 0f) {
                    changeState(PlayerStateType.Dead);
                }
            }
        }

        public void heal(float health) {
            if (_state.Value == PlayerStateType.Dead) {
                return;
            }

            _health = Mathf.Clamp(_health + health, 0f, _settings.HealthMax);

            _UIController.updateHealthBar(_health, 0f, _settings.HealthMax, true);
        }
        #endregion

        #region public DEBUG
        [NaughtyAttributes.Button]
        public void stunDEBUG() {
            changeState(PlayerStateType.Dead);
        }

        [NaughtyAttributes.Button]
        public void hit10() {
            hit(10f);
        }

        [NaughtyAttributes.Button]
        public void hit50() {
            hit(50f);
        }

        [NaughtyAttributes.Button]
        public void heal10() {
            heal(10f);
        }

        [NaughtyAttributes.Button]
        public void heal50() {
            heal(50f);
        }
        #endregion
    }
}
