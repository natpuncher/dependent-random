namespace NPG.DependentRandom.Infrastructure
{
	public interface IRandom
	{
		/// <summary>
		/// Gets a float between 0f and 1f
		/// </summary>
		/// <returns></returns>
		float GetValue();
	}
}