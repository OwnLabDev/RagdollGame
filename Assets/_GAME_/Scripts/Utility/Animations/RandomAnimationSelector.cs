using UnityEngine;

namespace OL.Kit.Components {
    public class RandomAnimationSelector : StateMachineBehaviour {
        #region editor
        [SerializeField] private bool _crossFade = true;
        [SerializeField] private int _animationsCount = 1;
        [SerializeField] private string _animationNamePrefix = "animation";

        [Header("Debug settings"), Space(10)]
        [Tooltip("Default value == -1")]
        [SerializeField] private int[] _debugAnimationIndexes = default;
        #endregion

        #region public
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            randomAnimation(animator, stateInfo, layerIndex, 
                _animationNamePrefix, _animationsCount, _debugAnimationIndexes, _crossFade);
        }

        public static string randomAnimation(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
                                           string animationPrefix, int animationsCount, int[] debugIndexes, bool crossFade) {
            int randomAnimationIndex = Random.Range(0, animationsCount);
            if (debugIndexes != null && debugIndexes.Length != 0) {
                randomAnimationIndex = debugIndexes[Random.Range(0, debugIndexes.Length)];
            }

            string randomAnimationTag = $"{animationPrefix}_{randomAnimationIndex}";

            if (crossFade) {
                animator.CrossFade(randomAnimationTag, .25f, layerIndex);
            } else {
                animator.Play(randomAnimationTag, layerIndex);
            }

            return randomAnimationTag;
        }
        #endregion
    }
}
