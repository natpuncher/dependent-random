using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NPG.DependentRandom.Infrastructure;
using NPG.DependentRandom.Models;

namespace NPG.DependentRandom.Implementations
{
	public class BinaryRollHistorySerializer : IRollHistorySerializer
	{
		private readonly string _filePath;

		public BinaryRollHistorySerializer(string filePath)
		{
			_filePath = filePath;
		}
		
		public void Serialize(RollHistoryContainer rollHistoryContainer)
		{
			if (File.Exists(_filePath))
			{
				File.Delete(_filePath);
			}
			
			Serialize(rollHistoryContainer.HistoryStorage, File.Open(_filePath, FileMode.Create));
		}

		public RollHistoryContainer Deserialize()
		{
			var result = new RollHistoryContainer();
			if (File.Exists(_filePath))
			{
				result.HistoryStorage = Deserialize(File.Open(_filePath, FileMode.Open));
			}
			return result;
		}

		private static void Serialize(Dictionary<string, int[]> dictionary, Stream stream)
		{
			try
			{
				using (stream)
				{
					var bin = new BinaryFormatter();
					bin.Serialize(stream, dictionary);
				}
			}
			catch (IOException)
			{
			}
		}

		private static Dictionary<string, int[]> Deserialize(Stream stream)
		{
			var result = new Dictionary<string, int[]>();
			try
			{
				using (stream)
				{
					var bin = new BinaryFormatter();
					result = (Dictionary<string, int[]>) bin.Deserialize(stream);
				}
			}
			catch (IOException)
			{
			}
			return result;
		}
	}
}