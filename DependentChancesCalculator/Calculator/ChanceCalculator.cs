using System;
using System.Threading;
using NPG.DependentRandom.DependentChancesCalculator.Implementations;
using NPG.DependentRandom.Implementations;
using NPG.DependentRandom.Tests;

namespace NPG.DependentRandom.DependentChancesCalculator.Calculator
{
	public class ChanceCalculator
	{
		private const int IterationCount = 100000;
		private const float Delta = 0.000000001f;
		private const float SmallValueBorder = 0.1f;

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

			var random = new DependentRandom.Implementations.DependentRandom(new SystemRandom(), new RollHistorySerializerMock(), chanceProvider);
			var eventName = "calculations";

			RollInfo rollInfo;
			do
			{
				rollInfo = new RollInfo();
				for (var i = 0; i < IterationCount; i++)
				{
					rollInfo.Update(random.Roll(eventName, _sourceChance));
				}

				chanceProvider.DependentChance = UpdateDependentChance(chanceProvider.DependentChance, rollInfo.GetChance());
			} while (Math.Abs(rollInfo.GetChance() - _sourceChance) > Delta);

			_onComplete(chanceProvider.DependentChance);
		}

		private float UpdateDependentChance(float dependentChance, float sumChance)
		{
			var delta = (_sourceChance - sumChance) / 2;
			if (dependentChance < SmallValueBorder)
			{
				delta /= 10;
			}
			return dependentChance + delta;
		}
	}
}