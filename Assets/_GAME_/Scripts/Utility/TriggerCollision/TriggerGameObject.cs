using UnityEngine;
using UnityEngine.Events;

using OL.Kit.Utility;

namespace OL.Kit.Components {
    public class TriggerGameObject : TriggerCollisionBase {
        #region editor
        [Header("Trigger settings"), Space(10)]
        [SerializeField] private bool _markNestedCollidersAsTrigger = false;

        [Header("Event settings"), Space(10)]
        public UnityEvent<TriggerGameObject, GameObject> OnEnterEvent = default;
        public UnityEvent<TriggerGameObject, GameObject> OnExitEvent = default;
        public UnityEvent<TriggerGameObject, GameObject> OnStayEvent = default;
        #endregion

        #region public events        
        #endregion

        #region public properties
        public override bool IsTrigger => true;
        public override bool IsCollision => false;
        #endregion

        #region private
        private void OnTriggerEnter(Collider collider) {
            if (!_calculateOnEnter || _deactivated) {
                return;
            }

            if (UtilityMethods.checkoutLayer(_layerMask, collider.gameObject.layer)) {
                OnEnterEvent?.Invoke(this, collider.gameObject);

                deactivateOnEnter();
            }
        }

        private void OnTriggerExit(Collider collider) {
            if (!_calculateOnExit || _deactivated) {
                return;
            }

            if (UtilityMethods.checkoutLayer(_layerMask, collider.gameObject.layer)) {
                OnExitEvent?.Invoke(this, collider.gameObject);

                deactivateOnExit();
            }
        }

        private void OnTriggerStay(Collider collider) {
            if (!_calculateOnStay || _deactivated) {
                return;
            }

            if (UtilityMethods.checkoutLayer(_layerMask, collider.gameObject.layer)) {
                OnStayEvent?.Invoke(this, collider.gameObject);
            }
        }
        #endregion

        #region protected
        protected override void awakeVirtual() {
            base.awakeVirtual();

            Collider collider = GetComponent<Collider>();
            if (collider != null) {
                collider.isTrigger = true;
            }

            if (_markNestedCollidersAsTrigger) {
                foreach (Collider nestedCollider in _colliders) {
                    nestedCollider.isTrigger = true;
                }
            }
        }
        #endregion

        #region public
        #endregion
    }
}
