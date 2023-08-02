using MoreMountains.Tools;
using OL.Kit.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OL.Game {
    public class EnvironmentGOSpawner : MonoBehaviour {
        #region editor
        [SerializeField] private Transform _holder = default;
        [SerializeField] private BoxCollider _bounds = default;
        [SerializeField] private GameObject[] _prefabs = default;

        [Space(10), Header("Spawn settings")]
        [SerializeField] private int _count = 10;
        [SerializeField] private MinMaxValue _XRotation = default;
        [SerializeField] private MinMaxValue _YRotation = default;
        [SerializeField] private MinMaxValue _ZRotation = default;
        [SerializeField] private MinMaxValue _scaleDelta = default;
        [SerializeField] private MinMaxValue _heightOffset = default;
        
        #endregion

        #region public events
        #endregion

        #region public properties
        #endregion

        #region private
        private void Awake() {
            initializeComponents();
        }

        private void initializeComponents() {

        }

        [NaughtyAttributes.Button]
        public void spawn() {
            despawn();

            Bounds bounds = _bounds.bounds;
            for (int i = 0; i < _count; i++) {
                GameObject prefab = _prefabs[Random.Range(0, _prefabs.Length)];

                Vector3 position = new Vector3(Random.Range(bounds.min.x, bounds.max.x), _heightOffset.Random, Random.Range(bounds.min.z, bounds.max.z));
                Quaternion rotation = Quaternion.Euler(_XRotation.Random, _YRotation.Random, _ZRotation.Random);

                GameObject block = Instantiate(prefab, position, rotation, _holder);
                block.gameObject.SetActive(true);

                block.transform.localScale *= _scaleDelta.Random;
            }
        }

        [NaughtyAttributes.Button]
        public void despawn() {
            _holder.MMDestroyAllChildren();
        }
        #endregion

        #region protected
        #endregion

        #region public
        #endregion
    }
}
