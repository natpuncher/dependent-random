using System;
using System.Collections.Generic;

namespace NPG.DependentRandom.Infrastructure
{
	public interface IDependentRandom : IDisposable
	{
		/// <summary>
		/// Rolls an event with giving chance
		/// </summary>
		/// <param name="key"></param>
		/// <param name="chance"></param>
		/// <returns>Is event rolled</returns>
		bool Roll(string key, float chance);
		
		/// <summary>
		/// Rolls an event with giving chances
		/// </summary>
		/// <param name="key"></param>
		/// <param name="chances"></param>
		/// <returns>Id of rolled event</returns>
		int Roll(string key, IEnumerable<float> chances);
		int Roll(string key, float[] chances);
	}
}