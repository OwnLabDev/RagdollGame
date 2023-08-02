using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using OL.Utility;
using OL.Kit.Utility;

namespace OL.Game {
    [CreateAssetMenu(
        fileName = "PlayerSettings",
        menuName = "ScriptableObjects/GAME/Player/PlayerSettings")]
    public class PlayerSettings : ScriptableObject {
        #region editor
        [Header("Settings")]
        

        [Space(10), Header("UI settings")]
        [NaughtyAttributes.Expandable]
        public PlayerUISettings UISettings = default;

        [Space(10), Header("FX settings")]
        [NaughtyAttributes.Expandable]
        public PlayerFXSettings FXSettings = default;

        [Space(10), Header("Input settings")]
        [NaughtyAttributes.Expandable]
        public PlayerInputSettings InputSettings = default;

        [Space(10), Header("State settings")]
        [NaughtyAttributes.Expandable]
        public PlayerStateSettings StateSettings = default;

        [Space(10), Header("Camera settings")]
        [NaughtyAttributes.Expandable]
        public PlayerCameraSettings CameraSettings = default;

        [Space(10), Header("Ragdoll settings")]
        [NaughtyAttributes.Expandable]
        public PlayerRagdollSettings RagdollSettings = default;

        [Space(10), Header("Slow Motion settings")]
        [NaughtyAttributes.Expandable]
        public PlayerSlowMotionSettings SlowMotionSettings = default;                
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
