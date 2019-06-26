namespace NPG.DependentRandom.Tests
{
	public class RollInfo
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