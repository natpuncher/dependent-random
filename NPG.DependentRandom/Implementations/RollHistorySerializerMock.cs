using NPG.DependentRandom.Infrastructure;
using NPG.DependentRandom.Models;

namespace NPG.DependentRandom.Implementations
{
	public class RollHistorySerializerMock : IRollHistorySerializer
	{
		public void Serialize(RollHistoryContainer rollHistoryContainer)
		{
		}

		public RollHistoryContainer Deserialize()
		{
			return new RollHistoryContainer();
		}
	}
}