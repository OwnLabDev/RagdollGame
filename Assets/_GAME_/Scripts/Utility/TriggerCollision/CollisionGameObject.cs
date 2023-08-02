using UnityEngine;
using UnityEngine.Events;

using OL.Kit.Utility;

namespace OL.Kit.Components {
    public class CollisionGameObject : TriggerCollisionBase {
        #region editor
        [Header("Event settings"), Space(10)]
        public UnityEvent<CollisionGameObject, GameObject> OnEnterEvent = default;
        public UnityEvent<CollisionGameObject, GameObject> OnExitEvent = default;
        public UnityEvent<CollisionGameObject, GameObject> OnStayEvent = default;
        #endregion

        #region public events        
        #endregion

        #region public properties
        public override bool IsTrigger => false;
        public override bool IsCollision => true;
        public Collision CollisionInfo => _lastCollisionInfo;
        #endregion

        private Collision _lastCollisionInfo = default;

        #region private

        private void OnCollisionEnter(Collision collision) {
            if (!_calculateOnEnter || _deactivated) {
                return;
            }

            if (UtilityMethods.checkoutLayer(_layerMask, collision.gameObject.layer)) {
                //if (_drawGizmos) {
                //    Debug.Log($"OnCollisionEnter: {collision.gameObject}", gameObject);
                //}

                _lastCollisionInfo = collision;

                OnEnterEvent?.Invoke(this, collision.gameObject);

                deactivateOnEnter();
            }
        }

        private void OnCollisionExit(Collision collision) {
            if (!_calculateOnExit || _deactivated) {
                return;
            }

            if (UtilityMethods.checkoutLayer(_layerMask, collision.gameObject.layer)) {
                _lastCollisionInfo = collision;

                OnExitEvent?.Invoke(this, collision.gameObject);

                deactivateOnExit();
            }
        }

        private void OnCollisionStay(Collision collision) {
            if (!_calculateOnStay || _deactivated) {
                return;
            }

            if (UtilityMethods.checkoutLayer(_layerMask, collision.gameObject.layer)) {
                _lastCollisionInfo = collision;

                OnStayEvent?.Invoke(this, collision.gameObject);
            }
        }
        #endregion

        #region protected
        protected override void awakeVirtual() {
            base.awakeVirtual();

            foreach (Collider collider in _colliders) {
                collider.isTrigger = false;
            }
        }
        #endregion

        #region public
        #endregion
    }
}
