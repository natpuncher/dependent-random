using System;
using NPG.DependentRandom.Implementations;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class Tests
	{
		private const float Delta = 0.01f;
		
		[Test]
		public void RollOne([Values(0.1f, 0.35f, 0.5f, 0.9f, 1f)]float chance)
		{
			var dependentRandom = DependentRandom.Create();
			var systemRandom = new SystemRandom();

			var dependentRandomLongestRow = 0;
			var systemRandomLongestRow = 0;
			
			var iterationCount = 1000000;

			var key = "someEventKey" + chance;

			var dependentInfo = new RollInfo();
			var systemInfo = new RollInfo();
			for (var i = 0; i < iterationCount; i++)
			{
				dependentInfo.Update(dependentRandom.Roll(key, chance));
				systemInfo.Update(systemRandom.GetValue() < chance);
			}
			
			Assert.AreEqual(chance, dependentInfo.GetChance(), Delta);
			Assert.AreEqual(chance, systemInfo.GetChance(), Delta);
		
			Console.WriteLine(string.Format("Dependent max row = {0}", dependentInfo.MaxRow));
			Console.WriteLine(string.Format("System max row = {0}", systemInfo.MaxRow));
		}

		private class RollInfo
		{
			public int MaxRow { get; private set; }
			private int _curRow;
			private bool _lastValue;
			private int _count;
			private int _successCount;

			public void Update(bool curValue)
			{
				_count++;
				if (curValue)
				{
					_successCount++;
				}
				
				if (curValue == _lastValue)
				{
					_curRow++;
				}
				else
				{
					_lastValue = curValue;
					if (_curRow > MaxRow)
					{
						MaxRow = _curRow;
					}
					_curRow = 0;
				}
			}

			public float GetChance()
			{
				return 1f * _successCount / _count;
			}
		}
	}
}