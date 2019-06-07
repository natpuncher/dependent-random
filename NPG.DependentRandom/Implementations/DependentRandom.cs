using System.Collections.Generic;
using System.Linq;
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

		/// <summary>
		/// Creates a simple version of DependentRandom class without serialization of roll history
		/// </summary>
		/// <returns></returns>
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

		public bool Roll(string key, float chance)
		{
			var dependentChance = ApplyHistoryToChance(key, chance);
			var rollId = GetRollId(new[] {dependentChance});
			_rollHistoryContainer.UpdateHistory(key, rollId);
			return rollId == 0;
		}

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

		public int Roll(string key, IEnumerable<float> chances)
		{
			return Roll(key, chances.ToArray());
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

		private float[] ApplyHistoryToChances(string key, float[] chances)
		{
			var chancesLength = chances.Length;
			var rollHistory = _rollHistoryContainer.GetHistory(key, chancesLength);
			chances = GetDependentChances(NormalizeChances(chances));
			for (var i = 0; i < chancesLength; ++i)
			{
				chances[i] = chances[i] * (rollHistory[i] + 1);
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
			var sumChance = GetSumChance(chances);
			var chancesLength = chances.Length;
			for (var i = 0; i < chancesLength; ++i)
			{
				chances[i] = chances[i] / sumChance;
			}

			return chances;
		}

		private float GetSumChance(float[] delta, int[] rollHistory = null)
		{
			var result = 0f;
			var hasHistory = rollHistory != null;
			var deltaLength = delta.Length;
			for (var i = 0; i < deltaLength; ++i)
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
			for (var i = 0; i < chancesLength; ++i)
			{
				chances[i] = _dependentChanceProvider.GetDependentChance(chances[i]);
			}

			return chances;
		}
	}
}