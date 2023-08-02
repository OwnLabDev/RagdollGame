using UnityEngine;
using UnityEngine.Events;

using FIMSpace;
using MoreMountains.Tools;

using OL.Utility;

namespace OL.Game {
    public class LocalController : MMSingleton<LocalController> {
        #region editor
        [NaughtyAttributes.Expandable]
        [SerializeField] private LocalSettings _localSettings = default;

        [Header("Controllers settings"), Space(10)]
        [SerializeField] private SlowMotionManager _slowMotionManager = default;
        [SerializeField] private LocalVisualController _localVisualController = default;

        [Space(10), Header("Events settings")]
        public UnityEvent OnGameStarted = default;

        [Header("DEBUG settings"), Space(10)]
        [SerializeField] private bool _useDebugSettings = false;
        [NaughtyAttributes.ShowIf(nameof(_useDebugSettings))]
        [SerializeField] private int _debugLevelsPassedCount = 0;
        [NaughtyAttributes.ShowIf(nameof(_useDebugSettings)), NaughtyAttributes.ReadOnly]
        [SerializeField] private int _debugLevelSettingsIndexDBG = 0;
        [NaughtyAttributes.ShowIf(nameof(_useDebugSettings))]
        [SerializeField] private LocalLevelSettingsData _localLevelSettingsData_DEBUG = default;
        #endregion

        #region public properties
        public SlowMotionManager SlowMotionManager => _slowMotionManager;
        public LocalVisualController LocalVisualController => _localVisualController;
        public LocalSettings LocalSettings => _localSettings;
        public LocalLevelSettingsData LocalLevelSettingsData => _localLevelSettingsData;
        #endregion

        private int _settingsIndex = 0;

        private LocalLevelSettingsData _localLevelSettingsData = default;

        private GameController _gameController = default;

        #region private
        private void initializeComponents() {
            _gameController = GameController.Instance;

            if (_gameController == null) {
                return;
            }

            if (requestLocalLevelSettingsData(out _localLevelSettingsData)) {
                // update gameplay stuff
                updateDynamicReadonlySettings();
            }

            FDebug.LogColored($"Load level settings: index: {_settingsIndex}", Color.cyan);
        }

        private LocalLevelSettingsData selectLocalLevelSettings() {
            if (_gameController == null || _gameController.GameSettings == null) {
                return _localSettings.LevelSettings.DefaultLocalLevelSettingsData;
            }

            int index = 0;
            int levelIndex = 0;

            if (Application.isPlaying) {
                if (_gameController != null) {
                    levelIndex = _gameController.GameSettings.CurrentSceneIndex - 1;
                }
            }

            if (_useDebugSettings
                && _localSettings.LevelSettings.LocalLevelSettingsData != null
                && _localSettings.LevelSettings.LocalLevelSettingsData.Length > 0) {
                levelIndex = _debugLevelsPassedCount;
            }

            if (_localSettings.LevelSettings.LocalLevelSettingsData != null
                && _localSettings.LevelSettings.LocalLevelSettingsData.Length > 0) {
                index = (levelIndex) / (_localSettings.LevelSettings.ChangeSettingsDataByLevelCount);
                index = (index) % _localSettings.LevelSettings.LocalLevelSettingsData.Length;
                index = Mathf.Clamp(index, 0, _localSettings.LevelSettings.LocalLevelSettingsData.Length - 1);

                // If level index exist in list of level settings
                if (index < _localSettings.LevelSettings.LocalLevelSettingsData.Length) {
                    index = Mathf.Clamp(index, 0, _localSettings.LevelSettings.LocalLevelSettingsData.Length - 1);

                    FDebug.LogYellow($"Loading level settings -> index: {index}");

                    _settingsIndex = index;

                    _debugLevelSettingsIndexDBG = _settingsIndex;
                    _localSettings.LevelSettings.SelectedSettingsIndex = _debugLevelSettingsIndexDBG;

                    return _localSettings.LevelSettings.LocalLevelSettingsData[_settingsIndex];
                }
            }

            FDebug.LogYellow($"Loading DEFAULT level settings!");

            return _localSettings.LevelSettings.DefaultLocalLevelSettingsData;
        }

        private LocalLevelSettingsData selectLocalLevelSettingsOLD() {
            int levelIndex = 0;

            if (Application.isPlaying) {
                GameController gameController = GameController.Instance;
                if (gameController != null) {
                    levelIndex = gameController.GameSettings.TotalLevelsPassed;
                }
            }

            if (_useDebugSettings) {
                levelIndex = _debugLevelsPassedCount;
            }

            //Debug.Log($"Load level settings: index: {levelIndex}");

            int index = (levelIndex) / (_localSettings.LevelSettings.ChangeSettingsDataByLevelCount);
            index = (index) % _localSettings.LevelSettings.LocalLevelSettingsData.Length;
            index = Mathf.Clamp(index, 0, _localSettings.LevelSettings.LocalLevelSettingsData.Length - 1);

            _debugLevelSettingsIndexDBG = index;
            _localSettings.LevelSettings.SelectedSettingsIndex = _debugLevelSettingsIndexDBG;

            return _localSettings.LevelSettings.LocalLevelSettingsData[index];
        }

        private void updateDynamicReadonlySettings() {
            _localSettings.EnvironmentSettings = _localLevelSettingsData.EnvironmentSettings;
        }
        #endregion

        #region protected
        private void OnValidate() {
#if UNITY_EDITOR
            //_localVisualController.updateLocalGameMaterials();
#endif
        }

        protected override void Awake() {
            base.Awake();

            initializeComponents();
        }
        #endregion

        #region public
        public bool requestLocalLevelSettingsData(out LocalLevelSettingsData localLevelSettingsData) {
            _localLevelSettingsData = selectLocalLevelSettings();

            // DEBUG STUFF
            if (_useDebugSettings
                && _localLevelSettingsData_DEBUG != null
                && _localLevelSettingsData_DEBUG.EnvironmentSettings != null) {
                _localLevelSettingsData = _localLevelSettingsData_DEBUG;

                FDebug.LogOrange($"!!! USING DEBUG SETTINGS !!!: {_localLevelSettingsData}");
            }

            localLevelSettingsData = _localLevelSettingsData;

            return _localLevelSettingsData != null;
        }

        public void activate() {
            Player player = FindObjectOfType<Player>();

            if (player != null) {
                player.activate();
            }

            OnGameStarted?.Invoke();
        }

        [NaughtyAttributes.Button]
        public void validate() {
            _localVisualController.updateLocalGameMaterials();
        }
        #endregion
    }
}
