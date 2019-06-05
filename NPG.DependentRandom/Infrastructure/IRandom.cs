namespace NPG.DependentRandom.Infrastructure
{
	public interface IRandom
	{
		int Roll(string key, float[] chances, bool serialize = true);
		bool Roll(string key, float chance, bool serialize = true);
		void Serialize();
	}
}