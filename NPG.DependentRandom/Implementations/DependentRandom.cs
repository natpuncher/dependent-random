using NPG.DependentRandom.Infrastructure;
using NPG.DependentRandom.Models;

namespace NPG.DependentRandom.Implementations
{
	public class DependentRandom : IDependentRandom
	{
		private readonly IRandom _random;
		private readonly IRollHistorySerializer _rollHistorySerializer;
		private readonly IDependentChanceProvider _dependentChanceProvider;
		private readonly RollHistoryContainer _rollHistoryContainer;

		public static IDependentRandom Create()
		{
			return new DependentRandom(new SystemRandom(), new RollHistorySerializerMock(),
				new CachedDependentChanceProvider());
		}

		public DependentRandom(IRandom random, IRollHistorySerializer rollHistorySerializer,
			IDependentChanceProvider dependentChanceProvider)
		{
			_random = random;
			_rollHistorySerializer = rollHistorySerializer;
			_dependentChanceProvider = dependentChanceProvider;
			_rollHistoryContainer = _rollHistorySerializer.Deserialize();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="chance"></param>
		/// <returns></returns>
		public bool Roll(string key, float chance)
		{
			var dependentChance = ApplyHistoryToChance(key, chance);
			var rollId = GetRollId(new[] {dependentChance});
			_rollHistoryContainer.UpdateHistory(key, rollId);
			return rollId == 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="chances"></param>
		/// <returns></returns>
		public int Roll(string key, float[] chances)
		{
			var dependentChances = ApplyHistoryToChances(key, chances);
			var rollId = GetRollId(dependentChances);
			if (rollId == -1)
			{
				rollId = chances.Length - 1;
			}

			_rollHistoryContainer.UpdateHistory(key, rollId);
			return rollId;
		}

		/// <summary>
		/// Serializes roll history
		/// </summary>
		public void Dispose()
		{
			_rollHistorySerializer.Serialize(_rollHistoryContainer);
		}

		private int GetRollId(float[] chances)
		{
			var result = -1;
			float sumValue = 0;
			var chancesLength = chances.Length;
			var rnd = _random.GetValue();
			for (var i = 0; i < chancesLength; ++i)
			{
				var chance = chances[i];
				if (rnd <= chance + sumValue)
				{
					result = i;
					break;
				}

				sumValue += chance;
			}

			return result;
		}

		private float[] ApplyHistoryToChances(string key, float[] sourceChances)
		{
			var rollHistory = _rollHistoryContainer.GetHistory(key, sourceChances.Length);
			var dependentChances = GetDependentChances(NormalizeChances(sourceChances));
			var chances = new float[sourceChances.Length];
			for (var i = 0; i < sourceChances.Length; ++i)
			{
				chances[i] = dependentChances[i] * (rollHistory[i] + 1);
			}

			return NormalizeChances(chances);
		}

		private float ApplyHistoryToChance(string key, float sourceChance)
		{
			var rollHistory = _rollHistoryContainer.GetHistory(key, 1);
			var dependentChance = _dependentChanceProvider.GetDependentChance(sourceChance);
			return dependentChance * (rollHistory[0] + 1);
		}

		private float[] NormalizeChances(float[] chances)
		{
			var result = new float[chances.Length];
			var sumChance = GetSumChance(chances);
			var chancesLength = chances.Length;
			for (var i = 0; i < chancesLength; ++i)
			{
				result[i] = chances[i] / sumChance;
			}

			return result;
		}

		private float GetSumChance(float[] delta, int[] rollHistory = null)
		{
			var result = 0f;
			var hasHistory = rollHistory != null;
			for (var i = 0; i < delta.Length; ++i)
			{
				var chance = delta[i];
				if (hasHistory)
				{
					chance *= (rollHistory[i] + 1);
				}

				result += chance;
			}

			return result;
		}

		private float[] GetDependentChances(float[] chances)
		{
			var chancesLength = chances.Length;
			var result = new float[chancesLength];
			for (var i = 0; i < chancesLength; ++i)
			{
				result[i] = _dependentChanceProvider.GetDependentChance(chances[i]);
			}

			return result;
		}
	}
}