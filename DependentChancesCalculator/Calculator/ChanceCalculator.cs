using System;
using System.Threading;
using DependentChancesCalculator.Calculator;
using NPG.DependentRandom.DependentChancesCalculator.Implementations;
using NPG.DependentRandom.Implementations;
using NPG.DependentRandom.Tests;

namespace NPG.DependentRandom.DependentChancesCalculator.Calculator
{
	public class ChanceCalculator
	{
		private const int IterationCount = 100000;
		private const float Delta = 0.000000001f;

		private readonly float _sourceChance;
		private readonly Action<float> _onComplete;

		public ChanceCalculator(float sourceChance, Action<float> onComplete)
		{
			_sourceChance = sourceChance;
			_onComplete = onComplete;

			new Thread(Calculate).Start();
		}

		private void Calculate()
		{
			var chanceProvider = new CalculatorDependentChanceProvider();
			chanceProvider.DependentChance = _sourceChance;
			var historyProvider = new ClearableRollHistoryMock();
			
			var random = new DependentRandom.Implementations.DependentRandom(new SystemRandom(), historyProvider, chanceProvider);
			var eventName = "calculations";

			var upper = _sourceChance;
			var lower = 0f;

			var sumChance = 1f;
			float lastChance;
			do
			{
				chanceProvider.DependentChance = (upper + lower) / 2;
				var rollInfo = new RollInfo();
				historyProvider.Clear();
				for (var i = 0; i < IterationCount; i++)
				{
					rollInfo.Update(random.Roll(eventName, _sourceChance));
				}
				
				lastChance = sumChance;
				sumChance = rollInfo.GetChance();
				if (sumChance > _sourceChance)
				{
					upper = chanceProvider.DependentChance;
				}
				else
				{
					lower = chanceProvider.DependentChance;
				}
			} while (Math.Abs(lastChance - sumChance) > Delta);

			_onComplete(chanceProvider.DependentChance);
		}
	}
}