using NPG.DependentRandom.Models;

namespace NPG.DependentRandom.Infrastructure
{
	public interface IRollHistorySerializer
	{
		/// <summary>
		/// Serializes the RollHistoryContainer
		/// </summary>
		/// <param name="rollHistoryContainer"></param>
		void Serialize(RollHistoryContainer rollHistoryContainer);
		
		/// <summary>
		/// Deserializes the RollHistoryContainer
		/// </summary>
		/// <returns></returns>
		RollHistoryContainer Deserialize();
	}
}