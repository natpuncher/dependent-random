namespace NPG.DependentRandom.Infrastructure
{
	public interface IDependentChanceProvider
	{
		/// <summary>
		/// Gets dependent chance delta for giving chance 
		/// </summary>
		/// <param name="chance"></param>
		/// <returns></returns>
		float GetDependentChance(float chance);
	}
}