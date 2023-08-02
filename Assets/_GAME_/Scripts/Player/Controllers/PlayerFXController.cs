using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using OL.Utility.FX.VFX;
using DamageNumbersPro;

namespace OL.Game {
    public class PlayerFXController : PlayerControllerBase {
        #region editor
        #endregion

        #region public events
        #endregion

        #region public properties
        public PlayerFXSettings Settings => _settings;
        #endregion

        private PlayerFXSettings _settings = default;

        #region private
        private void initializeComponents() {
            _settings = _player.Settings.FXSettings;
        }
        #endregion

        #region protected
        protected override void awakeVirtual() {
            base.awakeVirtual();

            initializeComponents();
        }
        #endregion

        #region public
        public void spawnHitVFX(Vector3 position) {
            if (!_settings.UseHitVFX) {
                return;
            }

            if (_settings.HitVFX == null) {
                return;
            }

            AutoDestroyVFX vfx = Instantiate(_settings.HitVFX, position + _settings.HitVFXOffset, Quaternion.identity);
        }

        public void spawnHitDNP(Vector3 position, float value, Color color) {
            if (!_settings.UseDNP) {
                return;
            }

            if (_settings.DNPPrefab == null) {
                return;
            }

            DamageNumber dnp = _settings.DNPPrefab.Spawn(position, value);

            Color dnpColor = color;
            if (_settings.OverrideDNPColor) {
                dnpColor = _settings.OverridenDNPColor;
            }

            dnp.SetColor(dnpColor);
        }
        #endregion
    }
}
