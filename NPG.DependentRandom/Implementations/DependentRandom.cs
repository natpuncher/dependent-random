using System.Collections.Generic;
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
		private readonly List<float> _chancesBuffer = new List<float>();

		/// <summary>
		/// Creates an instance of IDependentRandom without a serialization of roll history
		/// </summary>
		/// <returns></returns>
		public static IDependentRandom Create()
		{
			return new DependentRandom(new SystemRandom(), new RollHistorySerializerMock(), new CachedDependentChanceProvider());
		}

		/// <summary>
		/// Creates an instance of IDependentRandom with a binary serialization into the giving file
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static IDependentRandom Create(string filePath)
		{
			return new DependentRandom(new SystemRandom(), new BinaryRollHistorySerializer(filePath), new CachedDependentChanceProvider());
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
			_chancesBuffer.Clear();
			_chancesBuffer.Add(chance);
			ApplyHistoryToSingleChance(key);
			var rollId = GetRollId();
			_rollHistoryContainer.UpdateHistory(key, rollId);
			return rollId == 0;
		}

		public int Roll(string key, IEnumerable<float> chances)
		{
			_chancesBuffer.Clear();
			_chancesBuffer.AddRange(chances);
			ApplyHistoryToChances(key);
			var rollId = GetRollId();
			if (rollId == -1)
			{
				rollId = _chancesBuffer.Count - 1;
			}

			_rollHistoryContainer.UpdateHistory(key, rollId);
			return rollId;
		}
		
		public void ClearHistory()
		{
			_rollHistoryContainer.HistoryStorage.Clear();
		}

		/// <summary>
		/// Serializes roll history
		/// </summary>
		public void Dispose()
		{
			_rollHistorySerializer.Serialize(_rollHistoryContainer);
		}

		private int GetRollId()
		{
			var result = -1;
			float sumValue = 0;
			var chancesLength = _chancesBuffer.Count;
			var rnd = _random.GetValue();
			for (var i = 0; i < chancesLength; ++i)
			{
				var chance = _chancesBuffer[i];
				if (rnd <= chance + sumValue)
				{
					result = i;
					break;
				}

				sumValue += chance;
			}

			return result;
		}

		private void ApplyHistoryToChances(string key)
		{
			var chancesLength = _chancesBuffer.Count;
			var rollHistory = _rollHistoryContainer.GetHistory(key, chancesLength);
			NormalizeBuffer();
			ApplyDependencyToBuffer();
			for (var i = 0; i < chancesLength; ++i)
			{
				_chancesBuffer[i] = _chancesBuffer[i] * (rollHistory[i] + 1);
			}

			NormalizeBuffer();
		}

		private void ApplyHistoryToSingleChance(string key)
		{
			var rollHistory = _rollHistoryContainer.GetHistory(key, 1);
			var dependentChance = _dependentChanceProvider.GetDependentChance(_chancesBuffer[0]);
			_chancesBuffer[0] = dependentChance * (rollHistory[0] + 1);
		}

		private void NormalizeBuffer()
		{
			var sumChance = GetSumChance();
			var chancesLength = _chancesBuffer.Count;
			for (var i = 0; i < chancesLength; ++i)
			{
				_chancesBuffer[i] = _chancesBuffer[i] / sumChance;
			}
		}

		private float GetSumChance(int[] rollHistory = null)
		{
			var result = 0f;
			var hasHistory = rollHistory != null;
			var deltaLength = _chancesBuffer.Count;
			for (var i = 0; i < deltaLength; ++i)
			{
				var chance = _chancesBuffer[i];
				if (hasHistory)
				{
					chance *= rollHistory[i] + 1;
				}

				result += chance;
			}

			return result;
		}

		private void ApplyDependencyToBuffer()
		{
			var chancesLength = _chancesBuffer.Count;
			for (var i = 0; i < chancesLength; ++i)
			{
				_chancesBuffer[i] = _dependentChanceProvider.GetDependentChance(_chancesBuffer[i]);
			}
		}
	}
}