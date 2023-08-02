using System;

using UnityEditor;
using UnityEngine;

namespace OL.Kit.Utility {
    [Serializable]
    public class RandomizedFloat {
        [SerializeField] private bool _randomize = false;
        [SerializeField] private float _customValue = 0f;
        [SerializeField] private MinMaxValue _minMaxValue = default;

        #region public properties
        public MinMaxValue MinMax => _minMaxValue;
        public float ConstantValue => _customValue;
        public float RandomizedValue => _minMaxValue.Random;
        public float Value => _randomize ? RandomizedValue : ConstantValue;
        #endregion

        public RandomizedFloat() {
            _randomize = false;
            _customValue = 0f;
            _minMaxValue = new MinMaxValue(0f, 0f);
        }

        public RandomizedFloat(RandomizedFloat rf) {
            _randomize = rf._randomize;
            _customValue = rf._customValue;
            _minMaxValue = new MinMaxValue(rf._minMaxValue.MinValue, rf._minMaxValue.MaxValue);
        }

        public RandomizedFloat(float value) {
            _randomize = false;
            _customValue = value;
            _minMaxValue = new MinMaxValue(value, value);
        }

        public RandomizedFloat(float minValue, float maxValue) {
            _randomize = true;
            _customValue = minValue;
            _minMaxValue = new MinMaxValue(minValue, maxValue);
        }

        public static implicit operator RandomizedFloat(float value) {
            RandomizedFloat rf = new RandomizedFloat();

            rf._randomize = false;
            rf._customValue = value;
            rf._minMaxValue = new MinMaxValue(-value, value);

            return rf;
        }

        public void setValue(float value) {
            _randomize = false;
            _customValue = value;
            _minMaxValue = new MinMaxValue(value, value);
        }

        public void setRandomize(bool randomize) {
            _randomize = randomize;
        }

#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(RandomizedFloat))]
        public class RandomizedFloatPropertyDrawer : PropertyDrawer {
            private const int space = 5;

            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
                EditorGUI.BeginProperty(position, label, property);
                int indentLevel = EditorGUI.indentLevel;

                position = EditorGUI.PrefixLabel(position, label);

                EditorGUI.indentLevel = 0;
                Rect toggleRect = new Rect(position.x, position.y, 24, position.height);
                EditorGUIUtility.labelWidth = toggleRect.width / 2;
                SerializedProperty randomize = property.FindPropertyRelative("_randomize");
                EditorGUI.PropertyField(toggleRect, randomize, new GUIContent("R:"));
                position.x += toggleRect.width + EditorGUIUtility.labelWidth + space;
                position.width -= toggleRect.width + toggleRect.width + EditorGUIUtility.labelWidth + space;
                if (randomize.boolValue) {
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("_minMaxValue"), GUIContent.none);
                    //if (!EditorApplication.isPlaying) { // generate random value only in editor mode
                    //    RandomizedFloat target = fieldInfo.GetValue(property.serializedObject.targetObject) as RandomizedFloat;
                    //    property.FindPropertyRelative("_randomValue").floatValue = target.RandomizedValue;
                    //} else {
                    //    position.x += position.width + space;
                    //    position.width = toggleRect.width;
                    //    EditorGUI.LabelField(position, $"{property.FindPropertyRelative("_randomValue").floatValue:F1}");
                    //}
                } else {
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("_customValue"), new GUIContent("F:"));
                }

                EditorGUI.indentLevel = indentLevel;
                EditorGUI.EndProperty();
            }
        }
#endif
    }
}