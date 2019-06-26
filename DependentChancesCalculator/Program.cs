using System;
using System.Diagnostics;
using NPG.DependentRandom.DependentChancesCalculator.Calculator;

namespace NPG.DependentRandom.DependentChancesCalculator
{
	internal class Program
	{
		public static void Main(string[] args)
		{
			var delta = 0.01f;
			var from = int.Parse(args[0]);
			var to = int.Parse(args[1]);
			var sw = Stopwatch.StartNew();
			var count = 0;
			for (var index = from; index < to; index++)
			{
				var chance = delta * index;
				new ChanceCalculator(chance, val =>
				{
					count++;
					Console.WriteLine(string.Format("[{3}/{4}] {2}ms | {{{0}, {1}}}", chance, val, sw.ElapsedMilliseconds, count, to - from));
				});
			}
		}
	}
}