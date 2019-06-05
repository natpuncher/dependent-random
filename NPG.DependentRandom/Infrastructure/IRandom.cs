using System;

namespace NPG.DependentRandom.Infrastructure
{
	public interface IRandom : IDisposable
	{
		bool Roll(string key, float chance);
		int Roll(string key, float[] chances);
	}
}