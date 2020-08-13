using System;
using System.Collections.Generic;

namespace NPG.DependentRandom.Infrastructure
{
	public interface IDependentRandom : IDisposable
	{
		/// <summary>
		/// Rolls an event with the giving chance
		/// </summary>
		/// <param name="key"></param>
		/// <param name="chance"></param>
		/// <returns>Is event rolled</returns>
		bool Roll(string key, float chance);
		
		/// <summary>
		/// Rolls one of the events with giving chances
		/// </summary>
		/// <param name="key"></param>
		/// <param name="chances"></param>
		/// <returns>The Id of rolled event</returns>
		int Roll(string key, IEnumerable<float> chances);
		
		/// <summary>
		/// Rolls one of the events with giving chances
		/// </summary>
		/// <param name="key"></param>
		/// <param name="chances"></param>
		/// <returns>The Id of rolled event</returns>
		int Roll(string key, float[] chances);

		/// <summary>
		/// Clears the roll history
		/// </summary>
		void ClearHistory();
	}
}