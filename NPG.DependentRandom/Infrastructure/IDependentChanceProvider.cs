namespace NPG.DependentRandom.Infrastructure
{
	public interface IDependentChanceProvider
	{
		float GetDependentChance(float chance);
	}
}