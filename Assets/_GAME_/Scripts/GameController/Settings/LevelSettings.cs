using UnityEngine;

using OL.Kit.Utility;

namespace OL.Game {
    [System.Serializable]
    public class LocalLevelSettingsData {
        [Space(10), Header("Environment settings")]
        [NaughtyAttributes.Expandable]
        public LocalEnvironmentSettings EnvironmentSettings = default;
    }

    [CreateAssetMenu(
        fileName = "LevelSettings",
        menuName = "ScriptableObjects/LevelSettings")]
    public class LevelSettings : ScriptableObject {
        public int ChangeSettingsDataByLevelCount = 1;
        public LocalLevelSettingsData DefaultLocalLevelSettingsData = default;
        public LocalLevelSettingsData[] LocalLevelSettingsData = default;

        [MoreMountains.Tools.MMReadOnly]
        public int SelectedSettingsIndex = 0;

        #region private
        private void OnValidate() {
#if UNITY_EDITOR
            FindObjectOfType<LocalVisualController>()?.updateLocalGameMaterials();
#endif
        }
        #endregion
    }
}