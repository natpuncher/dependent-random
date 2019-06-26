using System;
using NPG.DependentRandom.Implementations;
using NUnit.Framework;

namespace NPG.DependentRandom.Tests
{
	[TestFixture]
	public class RandomComparisonTests
	{
		private const float Delta = 0.005f;
		
		[Test]
		public void RollOne([Values(0.1f, 0.35f, 0.5f, 0.6f, 0.8f, 1f)]float chance)
		{
			var dependentRandom = NPG.DependentRandom.Implementations.DependentRandom.Create();
			var systemRandom = new SystemRandom(12323542);

			var dependentRandomLongestRow = 0;
			var systemRandomLongestRow = 0;
			
			var iterationCount = 1000000;

			var key = "someEventKey" + chance;

			var dependentInfo = new RollInfo();
			var systemInfo = new RollInfo();
			for (var i = 0; i < iterationCount; i++)
			{
				dependentInfo.Update(dependentRandom.Roll(key, chance));
				systemInfo.Update(systemRandom.GetValue() < chance);
			}
			
			Assert.AreEqual(chance, dependentInfo.GetChance(), Delta);
			Assert.AreEqual(chance, systemInfo.GetChance(), Delta);
		
			Console.WriteLine(string.Format("Dependent random chance = {0}, max row = {1}", dependentInfo.GetChance(), dependentInfo.MaxRow));
			Console.WriteLine(string.Format("System random chance = {0}, max row = {1}", systemInfo.GetChance(), systemInfo.MaxRow));
		}
	}
}