using System;

namespace Seven.AudioSystem.SubSystems.CustomAttributes.RangedNumber.Float
{
	public class MinMaxRangeFloatAttribute : Attribute
	{
		public readonly float Min;
		public readonly float Max;
		
		public MinMaxRangeFloatAttribute(float min, float max)
		{
			Min = min;
			Max = max;
		}
	}
}