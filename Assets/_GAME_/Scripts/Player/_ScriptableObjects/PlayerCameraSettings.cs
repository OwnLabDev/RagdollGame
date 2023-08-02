using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using OL.Kit.Utility;

namespace OL.Game {
    [CreateAssetMenu(
        fileName = "PlayerCameraSettings",
        menuName = "ScriptableObjects/GAME/Player/PlayerCameraSettings")]
    public class PlayerCameraSettings : ScriptableObject {
        #region editor
        [Space(10), Header("Camera settings")]
        public bool UseCameraShake = true;
        [NaughtyAttributes.ShowIf(nameof(UseCameraShake))]
        public RandomizedFloat SkakeAmplitude = 5f;
        [NaughtyAttributes.ShowIf(nameof(UseCameraShake))]
        public RandomizedFloat ShakeFrequence = 1f;
        [NaughtyAttributes.ShowIf(nameof(UseCameraShake))]
        public RandomizedFloat ShakeDuration = .5f;

        [Space(10), Header("Camera zoom settings")]
        public bool UseCameraZoom = true;
        [NaughtyAttributes.ShowIf(nameof(UseCameraZoom))]
        public float CameraZoom = 5f;
        [NaughtyAttributes.ShowIf(nameof(UseCameraZoom))]
        public float CameraZoomDuration = 1f;
        #endregion

        #region public events
        #endregion

        #region public properties
        #endregion

        #region private
        #endregion

        #region protected
        #endregion

        #region public
        #endregion
    }
}
