using UnityEngine;

namespace OL.Kit.Components {
    public abstract class OLMonoBehaviour : MonoBehaviour {
        #region private
        private void Awake() {
            awakeVirtual();
        }

        private void Start() {
            startVirtual();
        }

        private void OnEnable() {
            enableVirtual();
        }

        private void OnDisable() {
            disableVirtual();
        }

        private void OnDestroy() {
            destroyVirtual();
        }
        #endregion

        #region protected
        protected virtual void awakeVirtual() { }
        protected virtual void startVirtual() { }
        protected virtual void destroyVirtual() { }
        protected virtual void enableVirtual() { }
        protected virtual void disableVirtual() { }
        #endregion
    }
}
