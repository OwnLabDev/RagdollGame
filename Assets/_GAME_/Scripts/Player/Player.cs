using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using OL.Kit.Components;

namespace OL.Game {
    public class Player : OLMonoBehaviour {
        #region editor
        [Header("Settings")]
        [NaughtyAttributes.Expandable]
        [SerializeField] protected PlayerSettings _settings = default;
        #endregion

        #region public events
        #endregion

        #region public properties
        public PlayerSettings Settings => _settings;
        public PlayerObserverController ObserverController => _observerController;        
        #endregion

        protected PlayerObserverController _observerController = default;

        #region private
        private void initializeComponents() {
            _observerController = GetComponentInChildren<PlayerObserverController>();
        }
        #endregion

        #region protected
        protected override void awakeVirtual() {
            base.awakeVirtual();

            initializeComponents();
        }
        #endregion

        #region public
        public virtual void activate() {
            _observerController.activate();
        }

        public virtual void deactivate() {
            _observerController.deactivate();
        }
        #endregion

        #region public abstract
        #endregion

        #region public virtual
        #endregion
    }
}
