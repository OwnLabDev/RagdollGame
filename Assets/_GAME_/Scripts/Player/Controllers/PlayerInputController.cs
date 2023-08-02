using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace OL.Game {
    public class PlayerInputController : PlayerControllerBase {
        #region editor
        #endregion

        #region public events
        public event Action<GameObject> OnSelect = default;
        #endregion

        #region public properties
        public Ray InputRay => _inputRay;
        public PlayerInputSettings Settings => _settings;
        #endregion

        private Camera _camera = default;

        private Ray _inputRay = default;

        private PlayerInputSettings _settings = default;

        #region private
        private void Update() {
            if (!_isActive) {
                return;
            }

            if (Input.GetMouseButtonDown(0)) {
                GameObject selectedObject = select();

                //Debug.Log($"selectedObject: {selectedObject}");

                if (selectedObject != null) {
                    OnSelect?.Invoke(selectedObject);
                }
            }
        }

        private void initializeComponents() {
            _settings = _player.Settings.InputSettings;

            _camera = _cameraController.VCAMController.CameraManager.MainCamera;
        }

        private GameObject select() {
            if (raycastCamera(out RaycastHit rayInfo, _settings.RaycastLayer)) {
                return rayInfo.collider.gameObject;
            }

            return null;
        }

        private bool raycast(Ray ray, out RaycastHit rayInfo, LayerMask mask) {
            Debug.DrawRay(ray.origin, ray.direction * Mathf.Infinity, Color.green, 2f);

            if (Physics.Raycast(ray, out rayInfo, Mathf.Infinity, mask, QueryTriggerInteraction.Ignore)) {
                return true;
            }

            return false;
        }

        private bool raycast(Vector3 origin, Vector3 direction, out RaycastHit rayInfo, LayerMask mask) {
            _inputRay = new Ray(origin, direction);

            return raycast(_inputRay, out rayInfo, mask);
        }

        private bool raycastCamera(out RaycastHit rayInfo, LayerMask mask) {
            _inputRay = _camera.ScreenPointToRay(Input.mousePosition);

            return raycast(_inputRay, out rayInfo, mask);
        }
        #endregion

        #region protected
        protected override void awakeVirtual() {
            base.awakeVirtual();

            initializeComponents();
        }
        #endregion

        #region public
        #endregion
    }
}
