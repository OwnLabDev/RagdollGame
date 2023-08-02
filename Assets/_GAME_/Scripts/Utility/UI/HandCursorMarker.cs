using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using DG.Tweening;

namespace OL.Game {
    public class HandCursorMarker : MonoBehaviour {
        #region editor
        [Header("UI settings"), Space(10)]
        [SerializeField] private Transform _handUI = default;

        [SerializeField] private string _clipName = "Click";
        [SerializeField] private Animator _animator = default;
        #endregion

        #region public properties
        #endregion

        private Camera _mainCamera = default;

        #region private
        private void Awake() {
            initializeComponents();
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                showMarker();

                _animator.Play(_clipName, 0, 0);
            }

            updateUIHandPosition();
        }

        private void initializeComponents() {
            _mainCamera = Camera.main;
        }

        private void updateUIHandPosition() {
            _handUI.position = Input.mousePosition;
        }

        private void showMarker() {
            _handUI.gameObject.SetActive(true);

            _handUI.localScale = Vector3.one;
        }

        private void hideMarker() {
            _handUI.localScale = Vector3.one;

            _handUI.gameObject.SetActive(false);
        }
        #endregion

        #region public
        #endregion
    }
}
