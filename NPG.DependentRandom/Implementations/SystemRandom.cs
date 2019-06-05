using System;
using NPG.DependentRandom.Infrastructure;

namespace NPG.DependentRandom.Implementations
{
	public class SystemRandom : IRandom
	{
		private readonly Random _rnd;

		public SystemRandom()
		{
			_rnd = new Random();
		}
		
		public SystemRandom(int seed)
		{
			_rnd = new Random(seed);
		}
		
		public float GetValue()
		{
			return (float)_rnd.NextDouble();
		}
	}
}