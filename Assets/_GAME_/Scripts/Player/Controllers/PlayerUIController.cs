using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using DG.Tweening;
using MoreMountains.Tools;
using UnityEngine.UI;

namespace OL.Game {
    public class PlayerUIController : PlayerControllerBase {
        #region editor
        [SerializeField] protected MMHealthBar _healthBar = default;
        [SerializeField] private MMFollowTarget _healthBarFollower = default;
        #endregion

        #region public events
        #endregion

        #region public properties
        public Color HealthBarColor => _healthBarForeground.color;
        public PlayerUISettings Settings => _settings;
        #endregion

        private Image _healthBarForeground = default;

        private PlayerUISettings _settings = default;

        #region private
        private void initializeComponents() {
            _settings = _player.Settings.UISettings;

            _healthBar.Initialization();

            _healthBarForeground = _healthBar.TargetProgressBar.ForegroundBar.GetComponent<Image>();
        }
        #endregion

        #region protected
        protected override void awakeVirtual() {
            base.awakeVirtual();

            initializeComponents();
        }

        protected override void startVirtual() {
            base.startVirtual();

            _healthBarFollower.Offset = _settings.HealthBarFollowOffset;
        }
        #endregion

        #region public   
        public void showHealthBar() {
            _healthBar.TargetProgressBar.ShowBar();
        }

        public void hideHelthBar() {
            _healthBar.TargetProgressBar.HideBar(0f);
        }

        public void updateHealthBar(float current, float min, float max, bool show = true) {
            _healthBar.UpdateBar(Mathf.RoundToInt(current), min, max, show);

            float delta = _healthBar.TargetProgressBar.BarTarget;
            Color color = _settings.HealthBarGradient.Evaluate(delta);

            _healthBarForeground.color = color;
        }

        public void hideUI() {
            _healthBar.TargetProgressBar.HideBar(0f);
        }
        #endregion
    }
}
