using NPG.DependentRandom.Models;

namespace NPG.DependentRandom.Infrastructure
{
	public interface IRollHistorySerializer
	{
		void Serialize(RollHistory rollHistory);
		RollHistory Deserialize();
	}
}