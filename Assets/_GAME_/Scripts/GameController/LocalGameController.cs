using UnityEngine;
using UnityEngine.Events;

using MoreMountains.Tools;

namespace OL.Game {
	public class LocalGameController : MMSingleton<LocalGameController> {
	    #region editor
        [SerializeField] private bool _startByClick = true;
        [SerializeField] private bool _startAutomatically = false;

        [Space(10), Header("Events settings")]
        [SerializeField] private UnityEvent _onLocalGameStartedEvent = default;
        #endregion

        private float _gameActivationTime = 0f;

        private GameController _gameController = default;

	    #region private
	    protected override void Awake() {
            base.Awake();

	        initializeComponents();
	    }

        private void Start() {
            if (_startAutomatically) {
                startLocalGame();
            }
        }

        private void Update() {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.R)) {
                _gameController.restartLevel();
            }

            if (Input.GetKeyDown(KeyCode.N)) {
                _gameController.nextLevel();
            }
#endif
        }

        private void initializeComponents() {
            _gameController = GameController.Instance;            
        }
        #endregion

        #region public
        public void restartLocalGame() {
            _gameController.restartLevel();
        }

        public void startLocalGame() {
            _gameActivationTime = Time.time;

            LocalController.Instance.activate();

            _onLocalGameStartedEvent?.Invoke();
        }

        public int gamePlaytime(float time) {
            return (int)(time - _gameActivationTime);
        }
        #endregion
    }
}