using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using OL.Utility.FX.VFX;
using DamageNumbersPro;

namespace OL.Game {
    [CreateAssetMenu(
        fileName = "PlayerFXSettings",
        menuName = "ScriptableObjects/GAME/Player/PlayerFXSettings")]
    public class PlayerFXSettings : ScriptableObject {
        #region editor
        [Space(10), Header("VFX settings")]
        public bool UseHitVFX = true;
        [NaughtyAttributes.ShowIf(nameof(UseHitVFX))]
        public AutoDestroyVFX HitVFX = default;
        [NaughtyAttributes.ShowIf(nameof(UseHitVFX))]
        public Vector3 HitVFXOffset = Vector3.zero;

        [Space(10), Header("DNP settings")]
        public bool UseDNP = true;
        public bool OverrideDNPColor = true;
        [NaughtyAttributes.ShowIf(NaughtyAttributes.EConditionOperator.And, nameof(UseDNP), nameof(OverrideDNPColor))]
        public Color OverridenDNPColor = Color.green;
        [NaughtyAttributes.ShowIf(nameof(UseDNP))]
        public DamageNumber DNPPrefab = default;
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
