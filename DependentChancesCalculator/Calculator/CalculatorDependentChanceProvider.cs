using NPG.DependentRandom.Infrastructure;

namespace NPG.DependentRandom.DependentChancesCalculator.Implementations
{
	public class CalculatorDependentChanceProvider : IDependentChanceProvider
	{
		public float DependentChance { get; set; }
		
		public float GetDependentChance(float chance)
		{
			return DependentChance;
		}
	}
}