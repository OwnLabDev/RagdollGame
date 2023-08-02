using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace OL.Game {
    [System.Serializable]
    public enum EnemyBodyPartType {
        Default,
        Head,
        Spine,
        Limbs,
    }

    //[RequireComponent(typeof(Collider))]
    //[RequireComponent(typeof(Rigidbody))]
    public class RagdollBodyPart : MonoBehaviour {
        #region editor
        [SerializeField] private EnemyBodyPartType _part = EnemyBodyPartType.Default;
        #endregion

        #region public events
        #endregion

        #region public properties
        public Collider Collider => _collider;
        public Rigidbody Rigidbody => _rigidbody;
        public float HitMultiplier => _multiplier;
        public EnemyBodyPartType PartType => _part;
        #endregion

        private float _multiplier = 1f;

        private Collider _collider = default;
        private Rigidbody _rigidbody = default;

        #region private
        private void Awake() {
            initializeComponents();
        }

        private void initializeComponents() {
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();            
        }
        #endregion

        #region protected
        #endregion

        #region public
        public void setMultiplier(float multiplier) {
            _multiplier = multiplier;
        }
        #endregion
    }
}
