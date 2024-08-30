namespace Seven.AudioSystem.SubSystems.CustomAttributes.RangedNumber.Float
{
	[System.Serializable]
	public struct RangedFloat
	{
		public float minValue;
		public float maxValue;

		public RangedFloat(float minValue, float maxValue)
		{
			this.minValue = minValue;
			this.maxValue = maxValue;
		}

		/// <summary>
		/// Return a random float number between minValue and maxValue.
		/// </summary>
		public float RandomValue()
		{
			return UnityEngine.Random.Range(minValue, maxValue);
		}
	}
}