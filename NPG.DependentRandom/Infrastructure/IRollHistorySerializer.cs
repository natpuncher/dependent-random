using NPG.DependentRandom.Models;

namespace NPG.DependentRandom.Infrastructure
{
	public interface IRollHistorySerializer
	{
		void Serialize(RollHistoryContainer rollHistoryContainer);
		RollHistoryContainer Deserialize();
	}
}