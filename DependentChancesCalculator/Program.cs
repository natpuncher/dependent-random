using System;
using NPG.DependentRandom.DependentChancesCalculator.Calculator;

namespace NPG.DependentRandom.DependentChancesCalculator
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			var delta = 0.01f;
			for (var i = 0; i < 100; i++)
			{
				var chance = delta * i;
				var calculator = new ChanceCalculator(chance, val => Console.WriteLine(string.Format("{0}, {1}", chance, val)));
			}
			Console.WriteLine("Done");
		}
	}
}