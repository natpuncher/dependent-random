using System.IO;
using NPG.DependentRandom.Implementations;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class SerializationTests
	{
		private const string FileName = "testBinaryData";

		[Test]
		public void BinarySerializationTest()
		{
			var random = DependentRandom.Create(FileName);

			var eventName = "testEvent";
			random.Roll(eventName, 0.5f);
			random.Dispose();
			
			Assert.IsTrue(File.Exists(FileName));
			
			var serializer = new BinaryRollHistorySerializer(FileName);
			var history = serializer.Deserialize();
			
			Assert.IsTrue(history.HistoryStorage.ContainsKey(eventName));
		}

		[TearDown]
		public void Clear()
		{
			if (File.Exists(FileName))
			{
				File.Delete(FileName);
			}
		}
	}
}