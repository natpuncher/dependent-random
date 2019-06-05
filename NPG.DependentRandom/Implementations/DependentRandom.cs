using NPG.DependentRandom.Infrastructure;
using NPG.DependentRandom.Models;

namespace NPG.DependentRandom.Implementations
{
	public class DependentRandom : IRandom
	{
		private readonly IRollHistorySerializer _rollHistorySerializer;
		private readonly RollHistory _rollHistory;

		public DependentRandom(IRollHistorySerializer rollHistorySerializer)
		{
			_rollHistorySerializer = rollHistorySerializer;
			_rollHistory = _rollHistorySerializer.Deserialize();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="chance"></param>
		/// <returns></returns>
		public bool Roll(string key, float chance)
		{
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="chances"></param>
		/// <returns></returns>
		public int Roll(string key, float[] chances)
		{
			return 0;
		}

		/// <summary>
		/// 
		/// </summary>
		public void Dispose()
		{
			_rollHistorySerializer.Serialize(_rollHistory);
		}
	}
}