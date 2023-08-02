using UnityEngine;

using MoreMountains.Tools;
using System.Linq;
using System.Collections.Generic;

namespace OL.Kit.Components {
    //[RequireComponent(typeof(Collider))]
    public abstract class TriggerCollisionBase : OLMonoBehaviour {
        #region editor
        [Header("Calculate collision settings")]
        [SerializeField] protected bool _calculateOnEnter = false;
        [SerializeField] protected bool _calculateOnExit = false;
        [SerializeField] protected bool _calculateOnStay = false;

        [Header("Deactivation settings"), Space(10)]
        [MMCondition(nameof(_calculateOnEnter), hideInInspector: true)]
        [SerializeField] protected bool _destroyOnEnter = false;
        [MMCondition(nameof(_calculateOnEnter), hideInInspector: true)]
        [SerializeField] protected bool _deactivateOnEnter = true;
        [MMCondition(nameof(_calculateOnEnter), hideInInspector: true)]
        [SerializeField] protected bool _disableOnEnter = true;
        [MMCondition(nameof(_calculateOnExit), hideInInspector: true)]
        [SerializeField] protected bool _destroyOnExit = false;
        [MMCondition(nameof(_calculateOnExit), hideInInspector: true)]
        [SerializeField] protected bool _deactivateOnExit = true;
        [MMCondition(nameof(_calculateOnExit), hideInInspector: true)]
        [SerializeField] protected bool _disableOnExit = true;

        [Header("Layer settings"), Space(10)]
        [SerializeField] protected LayerMask _layerMask = default;

        [Header("Gizmos settings"), Space(10)]
        [SerializeField] protected bool _drawGizmos = false;
        [MMCondition(nameof(_drawGizmos), hideInInspector: true)]
        [SerializeField] private bool _drawGizmosWire = false;
        [MMCondition(nameof(_drawGizmos), hideInInspector: true)]
        [SerializeField] private Color _gizmosColor = Color.green;
        #endregion

        #region public events
        #endregion

        #region public properties
        public abstract bool IsTrigger { get; }
        public abstract bool IsCollision { get; }
        public Collider AttachedCollider => _attachedCollider;
        public IReadOnlyList<Collider> AttachedColliders => _colliders.AsReadOnly();
        #endregion

        protected bool _deactivated = false;

        protected Collider _attachedCollider = default;
        protected List<Collider> _colliders = default;

        #region private

        private void OnDrawGizmos() {
            if (_drawGizmos) {
                if (_colliders == null) {
                    initializeComponents();
                }

                BoxCollider collider = _colliders.FirstOrDefault(c => c is BoxCollider) as BoxCollider;
                if (collider != null) {
                    Gizmos.color = _gizmosColor;

                    MMDebug.DrawGizmoCube(transform, collider.center, collider.size, _drawGizmosWire);
                }
            }
        }

        private void initializeComponents() {
            _colliders = new List<Collider>();

            _attachedCollider = GetComponent<Collider>();
            //if(attachedCollider != null) {
            //    _colliders.Add(attachedCollider);
            //}

            _colliders.AddRange(GetComponentsInChildren<Collider>());
        }
        #endregion

        #region protected
        protected override void awakeVirtual() {
            base.awakeVirtual();

            initializeComponents();
        }

        protected void deactivateOnEnter() {
            if (_deactivateOnEnter) {
                _deactivated = true;
            }

            if (_disableOnEnter) {
                foreach (Collider collider in _colliders) {
                    collider.enabled = false;
                }
            }

            if (_destroyOnEnter) {
                Destroy(gameObject);
            }
        }

        protected void deactivateOnExit() {
            if (_deactivateOnExit) {
                _deactivated = true;
            }

            if (_disableOnExit) {
                foreach (Collider collider in _colliders) {
                    collider.enabled = false;
                }
            }

            if (_destroyOnExit) {
                Destroy(gameObject);
            }
        }
        #endregion

        #region public
        public void setLayerMask(LayerMask layerMask) {
            _layerMask = layerMask;
        }

        public void activate() {
            _deactivated = false;
        }

        public void activateCollider() {
            foreach (Collider collider in _colliders) {
                collider.enabled = true;
            }
        }

        public void deactivate() {
            _deactivated = true;
        }

        public void deactivateCollider() {
            foreach (Collider collider in _colliders) {
                collider.enabled = false;
            }
        }

        public void deactivateCompletely() {
            deactivate();
            deactivateCollider();

            gameObject.SetActive(false);
        }
        #endregion
    }
}
