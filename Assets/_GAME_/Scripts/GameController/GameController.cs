using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using FIMSpace;
using DG.Tweening;
using MoreMountains.Tools;

using OL.Game.UI;
using OL.Kit.Utility;

namespace OL.Game {
    public class GameController : MMPersistentSingleton<GameController> {
        #region editor
        [SerializeField] private int _levelToLoad = 0;

        [Header("Debug settings (+, -, -)"), Space(10)]
        [SerializeField] private bool _debugMode = false;
        [SerializeField] private bool _firstLaunch = false;

        [Header("Levels to skip settings"), Space(10)]
        [SerializeField] private int[] _levelsToSkip = default;
        [SerializeField] private int[] _levelsToSkipAfterLoop = default;

        [Header("Managers settings"), Space(10)]
        [SerializeField] private Transform _managersHolder = default;
        #endregion

        #region private properties
        private Manager this[string id] => _managers.FirstOrDefault(manager => manager.ID == id);
        #endregion

        #region public properties
        public GameSettings GameSettings => _settings;
        public UIManager UIManagerInstance => this["ui"] as UIManager;
        public CameraManager CameraManagerInstance => this["camera"] as CameraManager;
        #endregion

        private bool _levelSceneIsLoaded = false;

        private List<Manager> _managers = new List<Manager>();

        private GameSettings _settings = default;

        #region private
        protected override async void Awake() {
            base.Awake();

            _settings = new GameSettings(_debugMode, _firstLaunch, _levelsToSkip, _levelsToSkipAfterLoop);

            loadManagers();

            await waitingManagersLoading();
        }

        private void OnEnable() {
            SceneManager.sceneLoaded += onSceneLoaded;
        }

        private void OnDisable() {
            SceneManager.sceneLoaded -= onSceneLoaded;
        }

        private async void Start() {
            await waitingManagersLoading();

            DOTween.SetTweensCapacity(200, 150);

            int sceneBuildIndex = _levelToLoad != 0 ? _levelToLoad : _settings.nextLevelIndex();
            sceneBuildIndex = Mathf.Max(sceneBuildIndex, 1);

            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single);

            FDebug.LogColored($"All managers loaded, Loading scene: {sceneBuildIndex} ...", Color.green);
        }

        private void Update() {
            if (   !_levelSceneIsLoaded
                || GameSettings.GameIsStarted
                || !UIManagerInstance.MainMenu.IsVisible) {
                return;
            }

#if UNITY_EDITOR
            if (   Input.GetMouseButtonDown(0)
                && !UtilityMethods.IsPointerOverUIObject()) {

                startLocalGame();
            }
#elif UNITY_IOS || UNITY_ANDROID
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began 
                && !UtilityMethods.IsPointerOverUIObject()) {

                startLocalGame();
            }
#endif
        }

        private void onSceneLoaded(Scene scene, LoadSceneMode mode) {
            if (scene.buildIndex == 0 || scene.name == "Main") {
                return;
            }

            int totalLevelsPassed = _settings.TotalLevelsPassed;

            FDebug.LogColored($"Scene loaded: {scene.name}, index: {scene.buildIndex}, passed: {totalLevelsPassed}", Color.green);

            _settings.saveCurrentLevelIndex(scene.buildIndex);

            UIManagerInstance.showMenu(UIManagerInstance.FirstMenuToLoad);

            _levelSceneIsLoaded = true;
        }

        private void loadManagers() {
            _managers = _managersHolder.GetComponentsInChildren<Manager>().ToList();

            foreach (Manager manager in _managers) {
                manager.initializeVirtual();
            }
        }

        private async Task waitingManagersLoading() {
            bool managersLoaded = false;
            while (!managersLoaded) {
                managersLoaded = _managers.All(m => m.CurrentStatus == Manager.Status.Ready);

                await Task.Yield();
            }
        }
        #endregion

        #region protected
        #endregion

        #region public
        public void startLocalGame() {
            UIManagerInstance.showMenu(UIManager.UIMenuType.GameMenu);

            LocalGameController localGameController = LocalGameController.Instance;
            localGameController.startLocalGame();

            _settings.startLocalGame();
        }

        [NaughtyAttributes.Button]
        public void restartLevel() {
            _levelSceneIsLoaded = false;

            _settings.LevelNotPassed();

            UIManagerInstance.resetAllMenu(hideAfter: true);

            FDebug.LogYellow($"Restard scene: {_settings.CurrentSavedLevelIndex}");

            SceneManager.LoadScene(_settings.CurrentSavedLevelIndex, LoadSceneMode.Single);
        }

        [NaughtyAttributes.Button]
        public void nextLevel() {
            _levelSceneIsLoaded = false;

            _settings.LevelPassed();

            UIManagerInstance.resetAllMenu(hideAfter: true);

            int nextLevelIndex = _settings.nextLevelIndex();

            FDebug.LogYellow($"Next scene: {nextLevelIndex}");

            SceneManager.LoadScene(nextLevelIndex, LoadSceneMode.Single);
        }
        #endregion
    }
}