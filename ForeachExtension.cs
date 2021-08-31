using System.Collections.Generic;

void Main()
{
	foreach (var element in (1.4,10.4,0.5))
	{
		Console.WriteLine(element);
	}
}

static class Extensions
{
	public static IEnumerator<int> GetEnumerator (this int x) =>
		Enumerable.Range (0, x).GetEnumerator();

	public static IEnumerator<double> GetEnumerator(this (double start,double end, double step) loop)
	{
		List<double> list = new();	
		for (double i = loop.start; i <= loop.end; i+=loop.step)
			list.Add(i);
						
		return list.GetEnumerator();
	}	
}
