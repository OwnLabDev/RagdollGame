using UnityEngine;

namespace OL.Game {
    [System.Serializable]
    public class MaterialWithColor {
        public Material Material = default;
        public Color MaterialColor = Color.white;
    }

    [System.Serializable]
    public class MaterialWithTexture {
        public Material Material = default;
        public Texture MaterialTexture = default;
        public Vector2 MaterialTiling = Vector3.one;
    }

    [System.Serializable]
    public class MaterialWithNormalTexture {
        public Material Material = default;
        public Texture MaterialTexture = default;
    }

    [CreateAssetMenu(
        fileName = "LocalEnvironmentSettings",
        menuName = "ScriptableObjects/LocalEnvironmentSettings")]
    public class LocalEnvironmentSettings : ScriptableObject {
        [Space(10), Header("Skybox settings")]
        public float SkyboxIntensity = 1f;
        public Color SkyboxTopColor = Color.white;
        public Color SkyboxBottomColor = Color.white;
        public Material SkyboxMaterial = default;

        [Space(10), Header("Fog settings")]
        public bool FogEnabled = true;
        [NaughtyAttributes.ShowIf(nameof(FogEnabled))]
        public float FogStart = 75f;
        [NaughtyAttributes.ShowIf(nameof(FogEnabled))]
        public float FogEnd = 150f;
        [NaughtyAttributes.ShowIf(nameof(FogEnabled))]
        public FogMode FogMode = FogMode.Linear;
        [NaughtyAttributes.ShowIf(nameof(FogEnabled))]
        public Color FogColor = Color.white;
        [NaughtyAttributes.ShowIf(nameof(FogEnabled))]
        public float FogIntensity = .015f;
        [NaughtyAttributes.ShowIf(nameof(FogEnabled))]
        public Material FogMaterial = default;

        [Space(10), Header("Materials settings")]
        public MaterialWithColor[] MaterialsWithColor = default;
        public MaterialWithTexture[] MaterialsWithTexture = default;
        public MaterialWithNormalTexture[] MaterialsWithNormalTexture = default;

        #region private
#if UNITY_EDITOR
        private void OnValidate() {
            FindObjectOfType<LocalVisualController>()?.updateLocalGameMaterials();
        }
#endif
        #endregion
    }
}