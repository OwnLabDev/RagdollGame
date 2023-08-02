using OL.Game;
using System.Collections;
using UnityEngine;

namespace OL.Utility.FX.VFX {
    [RequireComponent(typeof(ParticleSystem))]
    public class AutoDestroyVFX : MonoBehaviour {
        #region editor
        [SerializeField] private bool _disableLoop = false;
        #endregion

        private ParticleSystem _ps = default;

        private Coroutine _destroyCoroutine = default;

        #region private
        private void Awake() {
            _ps = GetComponent<ParticleSystem>();

            if (_disableLoop) {
                ParticleSystem.MainModule main = _ps.main;

                main.loop = false;
            }
        }

        private void OnEnable() {
            if (_destroyCoroutine != null) {
                StopCoroutine(_destroyCoroutine);
            }

            _destroyCoroutine = StartCoroutine(destroyCoroutine());
        }

        private void OnDisable() {
            if (_destroyCoroutine != null) {
                StopCoroutine(_destroyCoroutine);

                _destroyCoroutine = null;
            }
        }

        private void OnDestroy() {
            if (_destroyCoroutine != null) {
                StopCoroutine(_destroyCoroutine);

                _destroyCoroutine = null;
            }
        }

        private IEnumerator destroyCoroutine() {
            yield return new WaitWhile(() => _ps.IsAlive(true));

            Destroy(gameObject);
        }
        #endregion

        #region public
        public void setColor(Color color, bool withChildren = true) {
            ParticleSystem.MainModule psMainModule = GetComponent<ParticleSystem>().main;
            psMainModule.startColor = color;

            if (withChildren) {
                foreach (ParticleSystem ps in transform.GetComponentsInChildren<ParticleSystem>()) {
                    ParticleSystem.MainModule mainModule = ps.main;
                    mainModule.startColor = color;
                }
            }
        }

        public void setColor(Gradient color, bool withChildren = true) {
            ParticleSystem.MainModule psMainModule = GetComponent<ParticleSystem>().main;
            psMainModule.startColor = color;

            if (withChildren) {
                foreach (ParticleSystem ps in transform.GetComponentsInChildren<ParticleSystem>()) {
                    ParticleSystem.MainModule mainModule = ps.main;
                    mainModule.startColor = color;
                }
            }
        }

        public void setMaterialColor(Color color) {
            ParticleSystemRenderer renderer = GetComponent<ParticleSystemRenderer>();
            renderer.material.color = color;
        }

        public static void setColor(ParticleSystem ps, Color color, bool withChildren = true) {
            ParticleSystem.MainModule psMainModule = ps.main;
            psMainModule.startColor = color;

            if (withChildren) {
                foreach (ParticleSystem localPS in ps.transform.GetComponentsInChildren<ParticleSystem>()) {
                    ParticleSystem.MainModule mainModule = localPS.main;
                    mainModule.startColor = color;
                }
            }
        }

        public static void setColor(ParticleSystem ps, Gradient color, bool withChildren = true) {
            ParticleSystem.MainModule psMainModule = ps.main;
            psMainModule.startColor = color;

            if (withChildren) {
                foreach (ParticleSystem localPS in ps.transform.GetComponentsInChildren<ParticleSystem>()) {
                    ParticleSystem.MainModule mainModule = localPS.main;
                    mainModule.startColor = color;
                }
            }
        }

        public static AutoDestroyVFX spawnVFX(AutoDestroyVFX[] variants, Transform parent) {
            if (variants == null || variants.Length == 0) {
                return null;
            }

            AutoDestroyVFX variant = variants[Random.Range(0, variants.Length)];
            if (variant != null) {
                return spawnVFX(variant, parent);
            }

            return null;
        }

        public static AutoDestroyVFX spawnVFX(AutoDestroyVFX[] variants, Vector3 position, Quaternion rotation) {
            if (variants == null || variants.Length == 0) {
                return null;
            }

            AutoDestroyVFX variant = variants[Random.Range(0, variants.Length)];
            if (variant != null) {
                return spawnVFX(variant, position, rotation);
            }

            return null;
        }

        public static AutoDestroyVFX spawnVFX(AutoDestroyVFX[] variants, Vector3 position, Quaternion rotation, Transform parent) {
            if (variants == null || variants.Length == 0) {
                return null;
            }

            AutoDestroyVFX variant = variants[Random.Range(0, variants.Length)];
            if (variant != null) {
                return spawnVFX(variant, position, rotation, parent);
            }

            return null;
        }

        public static AutoDestroyVFX spawnVFX(AutoDestroyVFX variant, Transform parent) {
            if (variant == null) {
                return null;
            }

            return Instantiate(variant, parent);
        }

        public static AutoDestroyVFX spawnVFX(AutoDestroyVFX variant, Vector3 position, Quaternion rotation) {
            if (variant == null) {
                return null;
            }

            return Instantiate(variant, position, rotation);
        }

        public static AutoDestroyVFX spawnVFX(AutoDestroyVFX variant, Vector3 position, Quaternion rotation, Transform parent) {
            if (variant == null) {
                return null;
            }

            return Instantiate(variant, position, rotation, parent);
        }
        #endregion
    }
}
