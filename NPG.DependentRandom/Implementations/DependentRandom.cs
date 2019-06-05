using NPG.DependentRandom.Infrastructure;
using NPG.DependentRandom.Models;

namespace NPG.DependentRandom.Implementations
{
	public class DependentRandom : IDependentRandom
	{
		private readonly IRandom _random;
		private readonly IRollHistorySerializer _rollHistorySerializer;
		private readonly IDependentChanceProvider _dependentChanceProvider;
		private readonly RollHistory _rollHistory;

		public DependentRandom(IRandom random, IRollHistorySerializer rollHistorySerializer, IDependentChanceProvider dependentChanceProvider)
		{
			_random = random;
			_rollHistorySerializer = rollHistorySerializer;
			_dependentChanceProvider = dependentChanceProvider;
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