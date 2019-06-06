using System;
using System.Collections.Generic;

namespace NPG.DependentRandom.Models
{
	public class RollHistoryContainer
	{
		public Dictionary<string, int[]> HistoryStorage = new Dictionary<string, int[]>();

		public int[] GetHistory(string key, int length)
		{
			int[] history;
			if (HistoryStorage.TryGetValue(key, out history))
			{
				if (history.Length != length)
				{
					history = InitializeHistory(key, length, history);
				}
			}
			else
			{
				history = InitializeHistory(key, length);
			}

			return history;
		}

		public void UpdateHistory(string key, int rollId)
		{
			int[] history;
			if (!HistoryStorage.TryGetValue(key, out history))
			{
				history = InitializeHistory(key, rollId + 1);
			}

			var length = history.Length;
			for (var i = 0; i < length; ++i)
			{
				if (i == rollId)
				{
					history[i] = 0;
				}
				else
				{
					history[i] += 1;
				}
			}
		}

		private int[] InitializeHistory(string key, int length, int[] history = null)
		{
			var result = new int[length];
			MergeHistory(result, history);
			HistoryStorage[key] = result;
			return result;
		}

		private static void MergeHistory(int[] result, int[] history)
		{
			if (history == null)
			{
				return;
			}

			var length = Math.Min(result.Length, history.Length);
			for (var i = 0; i < length; i++)
			{
				result[i] = history[i];
			}
		}
	}
}