using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace OL.Game {
    [CreateAssetMenu(
        fileName = "PlayerRagdollSettings",
        menuName = "ScriptableObjects/GAME/Player/PlayerRagdollSettings")]
    public class PlayerRagdollSettings : ScriptableObject {
        #region editor
        [Space(10), Header("Ragdoll muscle settings")]
        [NaughtyAttributes.Layer]
        public int PlayerLayer = 0;
        [NaughtyAttributes.Layer]
        public int RagdollLayer = 0;
        public MuscleGroupFloatDictionary MuscleGroupDictionary = default;

        [Space(10), Header("Ragdoll hit settings")]
        public float GetUpDelay = 1f;
        public float HitPower = 10f;
        public float HitDuration = .5f;
        public float FadeMusclesTo = 0.1f;
        public float FadeMusclesDuration = 0.75f;

        public float RagdollBlend = 1f;
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
