using System;

namespace NPG.DependentRandom.Infrastructure
{
	public interface IDependentRandom : IDisposable
	{
		bool Roll(string key, float chance);
		int Roll(string key, float[] chances);
	}
}