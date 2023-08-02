using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using DG.Tweening;

using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

namespace OL.Kit.Utility {
    public enum UpdateModes {
        Update,
        LateUpdate,
        FixedUpdate
    }

    [Serializable]
    public class ForceData {
        [Header("Force settings")]
        public RandomizedFloat Angle = 0f;
        public RandomizedFloat Force = 100f;
        public RandomizedFloat UpForce = 10f;
        public ForceMode ForceMode = ForceMode.Acceleration;
    }

    public static class UtilityMethods {
        public static float RandomSign => Random.Range(0, 2) * 2 - 1;
        public static bool RandomFlag => Random.value < .5f ? true : false;

        public static Color TransparentColor => new Color(1f, 1f, 1f, 0f);

        public static Color RandomColor => new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f)
        );

        public static Quaternion RandomRotation => Quaternion.Euler(
            Random.Range(-360f, 360f),
            Random.Range(-360f, 360f),
            Random.Range(-360f, 360f)
        );

        public static T randomEnumValue<T>(params T[] except) {
            List<T> values = Enum.GetValues(typeof(T)).OfType<T>().ToList();
            List<T> skippedValues = values.Except(except).ToList();
            int random = Random.Range(0, skippedValues.Count);
            return skippedValues[random];
        }

        public static float percent(float percent, float target) {
            return (target * percent) / 100f;
        }

        public static float percentValueAtoValueB(float a, float b) {
            return (a * 100f) / b;
        }

        public static float percentValueAtoValueB01(float a, float b) {
            return a / b;
        }

        public static float addPercent(float target, float percent) {
            return target * (100f + percent) / 100f;
        }

        public static float find100PercentValue(float value, float percent) {
            return 100f * value / percent;
        }

        public static bool chance01(float percent) {
            return Random.value <= percent;
        }

        public static bool chance0100(float percent) {
            return Random.value.map0100(0f, 1f) <= percent.map0100(0f, 100f);
        }

        // return angle between -180 180
        public static float wrapAngle(float angle) {
            angle %= 360;
            if (angle > 180) {
                return angle - 360;
            }

            return angle;
        }

        public static float unwrapAngle(float angle) {
            if (angle >= 0) {
                return angle;
            }

            angle = -angle % 360;

            return 360 - angle;
        }

        public static float angleBetween(Vector3 a, Vector3 b) {
            return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
        }

        public static int toInt(this bool value) {
            return value == true ? 1 : 0;
        }

        public static bool toBool(this int value) {
            return value != 0 ? true : false;
        }

        public static float map(this float value, float fromMin, float fromMax, float toMin, float toMax) {
            float percent = Mathf.InverseLerp(fromMin, fromMax, value);
            return Mathf.Lerp(toMin, toMax, percent);
        }

        public static float map01(this float value, float fromMin, float fromMax) {
            return value.map(fromMin, fromMax, 0f, 1f);
        }

        public static float map0100(this float value, float fromMin, float fromMax) {
            return value.map(fromMin, fromMax, 0f, 100f);
        }

        public static int map(this int value, int fromMin, int fromMax, int toMin, int toMax) {
            float percent = Mathf.InverseLerp(fromMin, fromMax, value);
            return Mathf.RoundToInt(Mathf.Lerp(toMin, toMax, percent));
        }

        public static float inverseLerpVector(Vector3 a, Vector3 b, Vector3 value) {
            Vector3 AB = b - a;
            Vector3 AV = value - a;
            return Mathf.Clamp01(Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB));
        }

        public static bool checkoutLayer(LayerMask layerMask, int layer) {
            return (layerMask.value & (1 << layer)) != 0;
        }

        public static float? animationClipLength(Animator animator, string clipName) {
            return animator.runtimeAnimatorController.animationClips.FirstOrDefault(clip => clip.name == clipName)?.length;
        }

        public static bool hasAnimatorParameter(Animator animator, string paramName) {
            foreach (AnimatorControllerParameter param in animator.parameters) {
                if (param.name == paramName) {
                    return true;
                }
            }
            return false;
        }

        public static bool hasAnimatorParameter(Animator animator, int paramHash) {
            foreach (AnimatorControllerParameter param in animator.parameters) {
                if (param.nameHash == paramHash) {
                    return true;
                }
            }
            return false;
        }

        public static IEnumerable<T> shuffle<T>(this IEnumerable<T> source, System.Random rng) {
            T[] elements = source.ToArray();
            for (int i = elements.Length - 1; i >= 0; i--) {
                int swapIndex = rng.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }

        public static List<List<T>> chunkBy<T>(this IEnumerable<T> source, int chunkSize) {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public static void setLayerRecursively(GameObject target, int newLayer, bool recursively = true) {
            if (target == null) {
                Debug.LogError("Obj == null");
                return;
            }

            target.layer = newLayer;

            if (!recursively) {
                return;
            }

            foreach (Transform child in target.transform) {
                if (null == child) {
                    continue;
                }

                setLayerRecursively(child.gameObject, newLayer);
            }
        }

        public static Vector3 randomBoxColliderPoint(BoxCollider box) {
            Vector3 extents = box.size / 2f;
            Vector3 offset = new Vector3(
                Random.Range(-extents.x, extents.x),
                Random.Range(-extents.y, extents.y),
                Random.Range(-extents.z, extents.z));

            Vector3 boxRandomPoint = box.transform.TransformPoint(box.center)
                + box.transform.up * offset.y
                + box.transform.right * offset.x
                + box.transform.forward * offset.z;

            return boxRandomPoint;
        }

        public static Transform findDeepChild(Transform root, string childName) {
            foreach (Transform child in root) {
                if (child.name == childName) {
                    return child;
                }

                Transform result = findDeepChild(child, childName);
                if (result != null) {
                    return result;
                }
            }
            return null;
        }

        public static void destroyInside(Transform holder) {
            foreach (Transform t in holder) {
                destroy(t.gameObject);
            }
        }

        public static void destroyInsideRuntimeAndEditor(Transform transform) {
            for (int t = transform.childCount - 1; t >= 0; t--) {
                if (Application.isPlaying) {
                    UnityEngine.Object.Destroy(transform.GetChild(t).gameObject);
                } else {
                    UnityEngine.Object.DestroyImmediate(transform.GetChild(t).gameObject);
                }
            }
        }

        public static void destroyInsideImmediate(Transform holder) {
            foreach (Transform t in holder) {
                UnityEngine.Object.DestroyImmediate(t.gameObject);
            }
        }

        public static void destroy<T>(T go) where T : UnityEngine.Object {
            if (Application.isPlaying) {
                UnityEngine.Object.Destroy(go);
            } else {
                UnityEngine.Object.DestroyImmediate(go);
            }
        }

        public static void destroyComponent<T>(T componemt) where T : UnityEngine.Component {
            if (Application.isPlaying) {
                UnityEngine.Object.Destroy(componemt);
            } else {
                UnityEngine.Object.DestroyImmediate(componemt);
            }
        }

        public static IEnumerator delayedCoroutineAction(float delay, Action action) {
            yield return new WaitForSeconds(delay);

            action?.Invoke();
        }

        public static IEnumerator delayedRealtimeCoroutineAction(float delay, Action action) {
            yield return new WaitForSecondsRealtime(delay);

            action?.Invoke();
        }

        public static Tween delayedTweenAction(float delay, TweenCallback action, bool ignoreTimeScale = true) {
            return DOVirtual.DelayedCall(delay, action, ignoreTimeScale);
        }

        public static TextMeshPro createWorldText(string text, Color color = default(Color), Transform holder = null, Vector3 localPosition = default(Vector3), int fontSize = 3) {
            GameObject textMeshObject = new GameObject($"text_{text}", typeof(TextMeshPro));
            Transform transform = textMeshObject.transform;
            transform.SetParent(holder, false);
            transform.localPosition = localPosition;
            transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            TextMeshPro textMesh = textMeshObject.GetComponent<TextMeshPro>();
            textMesh.GetComponent<RectTransform>().sizeDelta = Vector3.one;
            textMesh.enableWordWrapping = false;
            textMesh.alignment = TextAlignmentOptions.Center;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            return textMesh;
        }

        public static bool IsPointerOverUIObject() {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            return results.Count > 0;
        }

        public static bool IsPointerOverGameObject() {
            //check mouse
            if (EventSystem.current.IsPointerOverGameObject()) {
                return true;
            }

            //check touch
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) {
                if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId)) {
                    return true;
                }
            }

            return false;
        }

        public static IEnumerator checkInternetConnection(Action<bool> action) {
            UnityWebRequest request = new UnityWebRequest("http://google.com");

            yield return request.SendWebRequest();

            if (request.error != null) {
                action(false);
            } else {
                action(true);
            }
        }
    }
}
