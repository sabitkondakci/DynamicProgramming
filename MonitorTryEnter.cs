void Main()
{
	object locker1 = new object();
	object locker2 = new object();

	new Thread(() =>
	{	
        bool lockTaken = false;
		try
		{
			Monitor.Enter(locker1, ref lockTaken);
			Console.WriteLine("1");
			Thread.Sleep(1000);	
			bool lockTaken2 = false;
			try
			{
				Monitor.TryEnter(locker2,10000, ref lockTaken2);
				Console.WriteLine("2");
			}
			finally
			{
				if (lockTaken2)
				{
					Monitor.Exit(locker2);
				}
			}
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(locker1);
			}
		}
	}).Start();


	bool lockTaken = false;
	try
	{
		Monitor.Enter(locker2, ref lockTaken);
		Console.WriteLine("3");
		Thread.Sleep(1000);
		bool lockTaken2 = false;
		try
		{
			Monitor.Enter(locker1, ref lockTaken2);
			Console.WriteLine("4");
		}
		finally
		{
			if (lockTaken2)
			{
				Monitor.Exit(locker1);
			}
		}
	}
	finally
	{
		if (lockTaken)
		{
			Monitor.Exit(locker2);
		}
	}
}
