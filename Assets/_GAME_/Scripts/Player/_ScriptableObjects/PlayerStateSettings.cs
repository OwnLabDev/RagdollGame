using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace OL.Game {
    [CreateAssetMenu(
        fileName = "PlayerStateSettings",
        menuName = "ScriptableObjects/GAME/Player/PlayerStateSettings")]
    public class PlayerStateSettings : ScriptableObject {
        #region editor
        [Header("Health settings")]
        public float Damage = 10f;
        public float HealthMax = 100f;

        [Space(10), Header("Stun settings")]
        public float StunDuration = 2f;
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
