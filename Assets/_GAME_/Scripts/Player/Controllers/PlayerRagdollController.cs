using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using FIMSpace.FProceduralAnimation;

using OL.Kit.Utility;
using DG.Tweening;

namespace OL.Game {
    public class PlayerRagdollController : PlayerControllerBase {
        #region editor
        #endregion

        #region public events
        #endregion

        #region public properties
        public RagdollAnimator RagdollAnimator => _ragdollAnimator;
        #endregion

        private float _elapsedSinceFall = 0f;

        private float _limbsVelocityMagn = 0f;
        private float _limbsAngularVelocityMagn = 0f;
        private Vector3 _limbsVelocity = Vector3.zero;
        private Vector3 _limbsAngularVelocity = Vector3.zero;
        private RagdollProcessor.EGetUpType _canGetUpType = RagdollProcessor.EGetUpType.None;

        private RagdollAnimator _ragdollAnimator = default;

        private PlayerRagdollSettings _settings = default;

        #region private
        void Update() {
            if (!_isActive) {
                return;
            }

            if (_ragdollAnimator == null) {
                return;
            }

            if (_ragdollAnimator.Parameters.FreeFallRagdoll) {
                if (_canGetUpType != RagdollProcessor.EGetUpType.None) {
                    if (_limbsAngularVelocityMagn < 1f) {
                        if (_limbsVelocityMagn < 0.1f) {
                            _elapsedSinceFall += Time.deltaTime;

                            if (_elapsedSinceFall >= _settings.GetUpDelay) {
                                _elapsedSinceFall = 0f;

                                tryToGetUp();
                            }
                        }
                    }
                }
            }

            _canGetUpType = _ragdollAnimator.Parameters.User_CanGetUp(null, false);
            _limbsVelocity = _ragdollAnimator.Parameters.User_GetSpineLimbsVelocity();
            _limbsAngularVelocity = _ragdollAnimator.Parameters.User_GetSpineLimbsAngularVelocity();
            _limbsVelocityMagn = _limbsVelocity.magnitude;
            _limbsAngularVelocityMagn = _limbsAngularVelocity.magnitude;
        }

        private void initializeComponents() {
            _ragdollAnimator = _player.GetComponentInChildren<RagdollAnimator>();

            _settings = _player.Settings.RagdollSettings;
        }

        private void initializeBodyPartsMultiplier() {
            UtilityMethods.setLayerRecursively(_ragdollAnimator.gameObject, _settings.PlayerLayer, recursively: false);

            foreach (RagdollBodyPart bodyPart in _ragdollAnimator.GetComponentsInChildren<RagdollBodyPart>()) {
                if (_settings.MuscleGroupDictionary.ContainsKey(bodyPart.PartType)) {
                    float multiplier = _settings.MuscleGroupDictionary[bodyPart.PartType];

                    bodyPart.setMultiplier(multiplier);

                    UtilityMethods.setLayerRecursively(bodyPart.gameObject, _settings.RagdollLayer, recursively: false);
                }
            }
        }

        private void onRagdollBodyPartSelected(GameObject bodyPartGO) {
            string name = bodyPartGO.name;
            Transform bodyPartLink = UtilityMethods.findDeepChild(_ragdollAnimator.transform, name);

            if (bodyPartLink == null) {
                return;
            }

            if (bodyPartLink.TryGetComponent(out RagdollBodyPart bodyPart)) {
                Rigidbody bodyPartRigidbody = bodyPartGO.GetComponent<Rigidbody>();

                if (bodyPart != null && bodyPartRigidbody != null) {
                    Debug.Log($"{bodyPart}, multiplier: {bodyPart.HitMultiplier}");

                    if (_ragdollAnimator.Parameters.RagdollLimbs.Contains(bodyPartRigidbody)) {
                        _elapsedSinceFall = 0f;

                        float damage = _stateController.Settings.Damage * bodyPart.HitMultiplier;
                        _stateController.hit(damage);

                        _ragdollAnimator.StopAllCoroutines();
                        _ragdollAnimator.Parameters.SafetyResetAfterCouroutinesStop();
                        _ragdollAnimator.User_SetAllKinematic(false);
                        _ragdollAnimator.User_EnableFreeRagdoll();
                        _ragdollAnimator.User_SwitchAnimator(null, false, 0.15f);

                        float hitPower = _settings.HitPower * bodyPart.HitMultiplier;
                        Vector3 hitDirection = _inputController.InputRay.direction.normalized;
                        _ragdollAnimator.User_SetLimbImpact(bodyPartRigidbody, hitDirection * hitPower, _settings.HitDuration);

                        _ragdollAnimator.User_FadeMuscles(_settings.FadeMusclesTo, _settings.FadeMusclesDuration);

                        _slowMotionController.hitSlowMotion();
                        _FXController.spawnHitVFX(bodyPart.transform.position);
                        _FXController.spawnHitDNP(bodyPart.transform.position, damage, _UIController.HealthBarColor);
                    }
                }
            }
        }

        public void tryToGetUp() {
            if (_stateController.State == PlayerStateType.Dead) {
                return;
            }



            _ragdollAnimator.transform.rotation = _ragdollAnimator.Parameters.User_GetMappedRotationHipsToHead(Vector3.up);
            _ragdollAnimator.User_SwitchAnimator(null, true);
            _ragdollAnimator.User_GetUpStackV2(_settings.RagdollBlend, 1f, 0.7f);
            _ragdollAnimator.User_ForceRagdollToAnimatorFor();

            UtilityMethods.delayedTweenAction(.7f, () => {
                if (Physics.Raycast(_ragdollAnimator.transform.position + Vector3.up, Vector3.down, out RaycastHit info, 10f, LayerMask.NameToLayer("Default"), QueryTriggerInteraction.Ignore)) {
                    transform.position = info.point;
                }
            });

            Animator animator = _ragdollAnimator.GetComponent<Animator>();
            if (animator) {
                string animationClip = "GetUpFace";

                if (_canGetUpType == RagdollProcessor.EGetUpType.FromBack)
                    animationClip = "GetUpBack";

                animator.Play(animationClip, 0, 0f);
            }
        }
        #endregion

        #region protected
        protected override void awakeVirtual() {
            base.awakeVirtual();

            initializeComponents();
            initializeBodyPartsMultiplier();
        }

        protected override void enableVirtual() {
            base.enableVirtual();

            _inputController.OnSelect += onRagdollBodyPartSelected;
        }

        protected override void disableVirtual() {
            base.disableVirtual();

            _inputController.OnSelect -= onRagdollBodyPartSelected;
        }
        #endregion

        #region public
        #endregion
    }
}
