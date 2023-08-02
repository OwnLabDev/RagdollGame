using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace OL.Game {
    [CreateAssetMenu(
        fileName = "PlayerInputSettings",
        menuName = "ScriptableObjects/GAME/Player/PlayerInputSettings")]
    public class PlayerInputSettings : ScriptableObject {
        #region editor
        public LayerMask RaycastLayer = default;
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
