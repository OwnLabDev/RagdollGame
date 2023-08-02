using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace OL.Game {
    [RequireComponent(typeof(BoxCollider))]
    public class VCAMLevelConfiner : MonoBehaviour {
        #region editor
        [SerializeField] private BoxCollider _volume = default;
        #endregion

        #region public events
        #endregion

        #region public properties
        public BoxCollider Volume => _volume;
        #endregion

        #region private
        private void OnValidate() {
            initializeComponents();
        }

        private void Awake() {
            initializeComponents();
        }

        private void initializeComponents() {
            if (_volume == null) {
                _volume = GetComponent<BoxCollider>();
            }
        }
        #endregion

        #region protected
        #endregion

        #region public
        #endregion
    }
}
