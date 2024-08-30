#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Seven.AudioSystem.SubSystems.CustomAttributes.RangedNumber.Float.Editor
{
	[CustomPropertyDrawer(typeof(RangedFloat), true)]
	public class RangedFloatDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			label = EditorGUI.BeginProperty(position, label, property);
			position = EditorGUI.PrefixLabel(position, label);

			SerializedProperty minProp = property.FindPropertyRelative("minValue");
			SerializedProperty maxProp = property.FindPropertyRelative("maxValue");

			float minValue = minProp.floatValue;
			float maxValue = maxProp.floatValue;

			float rangeMin = 0;
			float rangeMax = 1;

			var ranges = (MinMaxRangeFloatAttribute[])fieldInfo.GetCustomAttributes(typeof(MinMaxRangeFloatAttribute), true);
			if (ranges.Length > 0)
			{
				rangeMin = ranges[0].Min;
				rangeMax = ranges[0].Max;
			}

			const float rangeBoundsLabelWidth = 40f;

			var rangeBoundsLabel1Rect = new Rect(position);
			rangeBoundsLabel1Rect.width = rangeBoundsLabelWidth;
			position.xMin += rangeBoundsLabelWidth;

			var rangeBoundsLabel2Rect = new Rect(position);
			rangeBoundsLabel2Rect.xMin = rangeBoundsLabel2Rect.xMax - rangeBoundsLabelWidth;
			position.xMax -= rangeBoundsLabelWidth;

			EditorGUI.BeginChangeCheck();
			minValue = EditorGUI.FloatField(rangeBoundsLabel1Rect, minValue, EditorStyles.helpBox);
			maxValue = EditorGUI.FloatField(rangeBoundsLabel2Rect, maxValue, EditorStyles.helpBox);
			EditorGUI.MinMaxSlider(position, ref minValue, ref maxValue, rangeMin, rangeMax);
			if (EditorGUI.EndChangeCheck())
			{
				minProp.floatValue = minValue;
				maxProp.floatValue = maxValue;
			}

			EditorGUI.EndProperty();
		}
	}
}
#endif