using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using OL.Game.UI;
using OL.Utility;
using OL.Kit.Utility;

namespace OL.Game {
    public class GameSettings {
        #region public properties
        public bool InDebugMode => _debug;
        public bool IsFirstLaunch => _isFirstLaunch;
        public bool GameIsStarted => _localGameIsStarted;
        public int CurrentSceneIndex => SceneManager.GetActiveScene().buildIndex;
        public int CurrentSavedLevelIndex => _currentLevel;
        public int TotalLevelsCount => _totalLevelsCount;
        public int TotalLevelsPassed => _totalLevelsPassed;
        public bool AllLevelsPassed => _totalLevelsPassed >= TotalLevelsCount;
        #endregion

        #region PlayerPrefs Tags
        private const string CURRENT_LEVEL = "CURRENT_LEVEL";
        private const string TOTAL_LEVELS_PASSED = "TOTAL_LEVELS_PASSED";
        private const string TOTAL_LEVELS_PLAYED = "TOTAL_LEVELS_PLAYED";
        private const string GLOBAL_SETTINGS_INITIALIZED = "GLOBAL_SETTINGS_INITIALIZED";
        #endregion

        private bool _debug = false;

        private int _currentLevel = 0;
        private int _totalLevelsCount = 0;
        private int _totalLevelsPassed = 0;
        private int _totalLevelsPlayed = 0;
        private int[] _levelsToSkip = default;
        private int[] _levelsToSkipAfterLoop = default;

        private bool _isFirstLaunch = false;
        private bool _localGameIsStarted = false;

        private List<int> _notPassedLevels = default;

        #region private
        private void initializeSavedSettings() {
            _currentLevel = PlayerPrefs.GetInt(CURRENT_LEVEL, 0);
            _totalLevelsPassed = PlayerPrefs.GetInt(TOTAL_LEVELS_PASSED, 0);
            _totalLevelsPlayed = PlayerPrefs.GetInt(TOTAL_LEVELS_PLAYED, 0);

            for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++) {
                if (!PlayerPrefs.GetInt($"Level_{i}", 0).toBool())
                    _notPassedLevels.Add(i); // this level is not passed
            }

            // mark boss levels as passed
            foreach (int i in _levelsToSkip) {
                _notPassedLevels.Remove(i);
            }

            _localGameIsStarted = false;

            if (_debug) {

            }
        }

        private void initializeFirstLaunchSettings() {
            PlayerPrefs.SetInt(CURRENT_LEVEL, 0);
            PlayerPrefs.SetInt(TOTAL_LEVELS_PASSED, 0);
            PlayerPrefs.SetInt(TOTAL_LEVELS_PLAYED, 0);

            for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++) {
                PlayerPrefs.SetInt($"Level_{i}", false.toInt());
            }

            // mark boss levels as passed
            foreach (int i in _levelsToSkip) {
                PlayerPrefs.SetInt($"Level_{i}", true.toInt());
            }

            PlayerPrefs.SetInt(GLOBAL_SETTINGS_INITIALIZED, true.toInt());
        }
        #endregion

        #region public
        public GameSettings(bool debugMode, bool firstLaunch, int[] skipLevels, int[] skipLevelsAfterLoop) {
            _debug = debugMode;
            _levelsToSkip = skipLevels;
            _levelsToSkipAfterLoop = skipLevelsAfterLoop;
            _notPassedLevels = new List<int>();

            _totalLevelsCount = SceneManager.sceneCountInBuildSettings - 1;

            _isFirstLaunch = !PlayerPrefs.GetInt(GLOBAL_SETTINGS_INITIALIZED).toBool() || firstLaunch;

            if (_isFirstLaunch) {
                initializeFirstLaunchSettings();
            }

            initializeSavedSettings();
        }

        public void saveCurrentLevelIndex(int value) {
            _currentLevel = value;

            _localGameIsStarted = false;

            PlayerPrefs.SetInt(CURRENT_LEVEL, _currentLevel);
        }

        public void startLocalGame() {
            _localGameIsStarted = true;

            if (_isFirstLaunch) {
                _isFirstLaunch = false;
            }
        }

        public void LevelPassed() {
            _totalLevelsPassed++;
            _totalLevelsPlayed++;

            _notPassedLevels.Remove(_currentLevel);

            PlayerPrefs.SetInt(TOTAL_LEVELS_PASSED, _totalLevelsPassed);
            PlayerPrefs.SetInt(TOTAL_LEVELS_PLAYED, _totalLevelsPlayed);

            PlayerPrefs.SetInt($"Level_{_currentLevel}", true.toInt());
        }

        public void LevelNotPassed() {
            _totalLevelsPlayed++;

            PlayerPrefs.SetInt(TOTAL_LEVELS_PLAYED, _totalLevelsPlayed);
        }

        public int nextLevelIndex() {
            int index = 1;
            if (_notPassedLevels.Count > 0) {
                index = _notPassedLevels.First();
            } else {
                // Main, Level_1
                if (SceneManager.sceneCountInBuildSettings > 2) {
                    int[] indexes = Enumerable.Range(1, _totalLevelsCount)
                        .Except(_levelsToSkip)
                        .Except(_levelsToSkipAfterLoop)
                        .Where(i => i != CurrentSavedLevelIndex).ToArray();

                    if (indexes.Length == 0) {
                        index = CurrentSavedLevelIndex;
                    } else {
                        index = indexes[Random.Range(0, indexes.Length)];
                    }
                } else {
                    index = 1; // else first scene (Level_1)
                }
            }
            return index;
        }
        #endregion
    }
}