using UnityEngine;

namespace OL.Game {
    public abstract class Manager : MonoBehaviour {
        public enum Status { NotReady, Initialization, Ready };

        #region editor
        [SerializeField] private string _managerID = "manager";
        #endregion

        #region public properties
        public string ID => _managerID;
        public Status CurrentStatus { get; protected set; } = Status.NotReady;
        #endregion

        protected GameController _gameController = default;

        #region public
        public virtual void initializeVirtual() {
            CurrentStatus = Status.Initialization;

            _gameController = GameController.Instance;
        }
        #endregion
    }
}