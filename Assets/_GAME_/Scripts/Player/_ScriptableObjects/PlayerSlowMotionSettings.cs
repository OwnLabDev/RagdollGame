using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using DG.Tweening;

using OL.Utility;

namespace OL.Game {
    [CreateAssetMenu(
        fileName = "PlayerSlowMotionSettings",
        menuName = "ScriptableObjects/GAME/Player/PlayerSlowMotionSettings")]
    public class PlayerSlowMotionSettings : ScriptableObject {
        #region editor
        [Space(10), Header("Reset slow motion settings")]
        public SlowMotionData SlowMotionResetData = default;

        [Space(10), Header("Hit slow motion settings")]
        public bool UseSlowMotionOnHit = true;
        [NaughtyAttributes.ShowIf(nameof(UseSlowMotionOnHit))]
        public SlowMotionData SlowMotionOnHitData = default;
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
