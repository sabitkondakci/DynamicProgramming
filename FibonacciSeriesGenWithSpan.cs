public List<double> FibonacciSeries(int firstNItem)
{
	List<double> store = new();
	const int stackArrSize = 25;	
	Span<double> fibo = stackalloc double[stackArrSize];
	
	fibo[0] = fibo[1] = 1;
	store.Add(1); store.Add(1);
	
	for (int i = 2; i < firstNItem; i++)
	{
		int k = i;
		k %= stackArrSize; // loop in an array of 25 elements stored in stack
		
		if (k == 0) 
		{
			fibo[0] = fibo[^2] + fibo[^1];
			fibo[1] = fibo[^1] + fibo[0];
			store.Add(fibo[0]) ; store.Add(fibo[1]);
			k = 2; // skip first two elements
			i += 2; // skip two steps
		}
		
		fibo[k] = fibo[k - 1] + fibo[k - 2];
		store.Add(fibo[k]);
	}
	
	
	return store;
}
