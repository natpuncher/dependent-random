using NPG.DependentRandom.Infrastructure;
using NPG.DependentRandom.Models;

namespace DependentChancesCalculator.Calculator
{
	public class ClearableRollHistoryMock : IRollHistorySerializer
	{
		private RollHistoryContainer _rollHistoryContainer;

		public void Serialize(RollHistoryContainer rollHistoryContainer)
		{
		}

		public RollHistoryContainer Deserialize()
		{
			_rollHistoryContainer = new RollHistoryContainer();
			return _rollHistoryContainer;
		}
		
		public void Clear()
		{
			_rollHistoryContainer.HistoryStorage.Clear();
		}
	}
}