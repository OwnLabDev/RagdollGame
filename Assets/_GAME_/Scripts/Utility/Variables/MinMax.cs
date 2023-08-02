using System;

using UnityEditor;
using UnityEngine;

using Random = UnityEngine.Random;

namespace OL.Kit.Utility {
    [Serializable]
    public class MinMaxValue {
        [SerializeField] private float _minValue = 0f;
        [SerializeField] private float _maxValue = 1f;

        #region public properties
        public float Min {
            get {
                //validate();

                return Mathf.Min(_minValue, _maxValue);
            }
        }

        public float Max {
            get {
                //validate();

                return Mathf.Max(_minValue, _maxValue);
            }
        }

        public float MinValue => _minValue;
        public float MaxValue => _maxValue;
        public float Middle => (_minValue + _maxValue) / 2f;
        public float Random => UnityEngine.Random.Range(Min, Max);
        #endregion

        #region private
        [NaughtyAttributes.Button]
        private void validate() {
            if (_maxValue < _minValue) {
                float tmp = _minValue;
                _minValue = _maxValue;
                _maxValue = tmp;
            }
        }
        #endregion

        #region public
        public MinMaxValue(MinMaxValue minMax) {
            setMin(minMax.MinValue);
            setMax(minMax.MaxValue);
        }

        public MinMaxValue(float minValue, float maxValue) {
            setMin(minValue);
            setMax(maxValue);
        }

        public void setMin(float minValue) {
            _minValue = minValue;
        }

        public void setMax(float maxValue) {
            _maxValue = maxValue;
        }

        public bool checkout(float value) {
            return value >= _minValue && value <= _maxValue;
        }

        public float clamped(float value) {
            return Mathf.Clamp(value, Min, Max);
        }

        public float mapped(float delta) {
            delta = Mathf.Clamp01(delta);

            return delta.map(0f, 1f, Min, Max);
        }
        #endregion

#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(MinMaxValue))]
        public class MinMaxPropertyDrawer : PropertyDrawer {
            private const int space = 5;

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
                EditorGUI.BeginProperty(position, label, property);
                int indentLevel = EditorGUI.indentLevel;

                position = EditorGUI.PrefixLabel(position, label);

                EditorGUIUtility.labelWidth = 32;
                position.width = (position.width - space) / 2;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("_minValue"), new GUIContent("min: "));
                position.x += position.width + space;
                EditorGUI.PropertyField(position, property.FindPropertyRelative("_maxValue"), new GUIContent("max: "));

                EditorGUI.indentLevel = indentLevel;
                EditorGUI.EndProperty();
            }
        }
#endif
    }
}