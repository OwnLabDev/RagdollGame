using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace OL.Game {
    [CreateAssetMenu(
        fileName = "PlayerUISettings",
        menuName = "ScriptableObjects/GAME/Player/PlayerUISettings")]
    public class PlayerUISettings : ScriptableObject {
        #region editor
        public Gradient HealthBarGradient = default;
        public Vector3 HealthBarFollowOffset = Vector3.zero;
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
