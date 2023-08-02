using UnityEngine;

namespace OL.Game {
    public class LocalVisualController : MonoBehaviour {
        #region editor
        #endregion

        #region public events
        #endregion

        #region public properties

        #endregion

        #region private
        private void Awake() {
            initializeComponents();
        }

        private void initializeComponents() {
            // update materials
            updateLocalGameMaterials();
        }
        #endregion

        #region public
        [NaughtyAttributes.Button]
        public void updateLocalGameMaterials() {
            LocalController localController = LocalController.Instance;

            LocalLevelSettingsData localLevelSettingsData = null;

            if (localController != null) {
                localController.requestLocalLevelSettingsData(out localLevelSettingsData);
            }

            if (localLevelSettingsData == null) {
                return;
            }

            applyVisualData(localLevelSettingsData);
        }

        public void applyVisualData(LocalLevelSettingsData localLevelSettingsData) {
            applyEnvironmentSettings(localLevelSettingsData.EnvironmentSettings);
        }

        public void applyEnvironmentSettings(LocalEnvironmentSettings environmentSettings) {
            // Fog
            RenderSettings.fogColor = environmentSettings.FogColor;
            RenderSettings.fogStartDistance = environmentSettings.FogStart;
            RenderSettings.fogEndDistance = environmentSettings.FogEnd;
            RenderSettings.fogMode = environmentSettings.FogMode;
            RenderSettings.fog = environmentSettings.FogEnabled;

            Camera mainCamera = Camera.main;
            if (mainCamera != null) {
                mainCamera.backgroundColor = environmentSettings.FogColor;
            }

            environmentSettings.FogMaterial.color = environmentSettings.FogColor;
            environmentSettings.FogMaterial.SetFloat("_Intensity", environmentSettings.FogIntensity);

            // Skybox
            if (environmentSettings.SkyboxMaterial.HasProperty("_Intensity")) {
                environmentSettings.SkyboxMaterial.SetFloat("_Intensity",
                    environmentSettings.SkyboxIntensity);
            }
            if (environmentSettings.SkyboxMaterial.HasProperty("_TopColor")) {
                environmentSettings.SkyboxMaterial.SetColor("_TopColor",
                environmentSettings.SkyboxTopColor);
            }
            if (environmentSettings.SkyboxMaterial.HasProperty("_BottomColor")) {
                environmentSettings.SkyboxMaterial.SetColor("_BottomColor",
                environmentSettings.SkyboxBottomColor);
            }

            RenderSettings.skybox = environmentSettings.SkyboxMaterial;

            // materials
            if (environmentSettings.MaterialsWithColor != null) {
                foreach (MaterialWithColor mwc in environmentSettings.MaterialsWithColor) {
                    if (mwc.Material != null) {
                        mwc.Material.color = mwc.MaterialColor;
                    }
                }
            }

            if (environmentSettings.MaterialsWithTexture != null) {
                foreach (MaterialWithTexture mwt in environmentSettings.MaterialsWithTexture) {
                    if (mwt.Material != null) {
                        mwt.Material.mainTexture = mwt.MaterialTexture;
                        mwt.Material.mainTextureScale = mwt.MaterialTiling;
                    }
                }
            }

            if (environmentSettings.MaterialsWithNormalTexture != null) {
                foreach (MaterialWithNormalTexture mwnt in environmentSettings.MaterialsWithNormalTexture) {
                    if (mwnt.Material != null) {
                        mwnt.Material.SetTexture("_SplatTileNormalTex", mwnt.MaterialTexture);
                    }
                }
            }
        }
        #endregion
    }
}
