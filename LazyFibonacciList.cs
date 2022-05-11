void Main()
{
	var fiboList = new LazyFibonacciList();
	
	var list = new List<int>();
  
  // if you don't use Take(), it'll loop infinitely.
	foreach (var element in fiboList.Take(10))
	{
		Console.WriteLine(element);
	}
}

public class LazyFibonacciList : IEnumerable<long>
{

	public IEnumerator<long> GetEnumerator() => new Enumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	struct Enumerator : IEnumerator<long>
	{
		public long Current { get; private set; }

		private long Last { get; set; }
		
		object IEnumerator.Current => Current;

		public void Dispose() { }

		public bool MoveNext()
		{
			if (Current == -1)
				Current = 0;
			else if (Current == 0)
				Current = 1;
			else
			{
				long next = Current + Last;
				Last = Current;
				Current = next;
			}
			
			return true;
		}

		public void Reset()
		{
			Current = -1;
		}
	}
}
