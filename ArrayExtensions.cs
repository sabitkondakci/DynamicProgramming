// LinqPad 6

void Main()
{
	int[] arr = { 1, 2, 3, 4, 2};

	/*
		var sameArr = arr.Fill(9,3);
		sameArr.Dump(); // 1, 2, 3, 9, 9
		arr.Dump(); // 1, 2, 3, 9, 9
	*/

	/*
		var copyArr = arr.FillAndCopy(9,3);
		copyArr.Dump(); // 1, 2, 3, 9, 9
		arr.Dump(); // 1, 2, 3, 4, 2
	*/

	/*
		var sameMappedArr = arr.Map(a => (int)Math.Pow(a,3),2);
		sameMappedArr.Dump(); // 1, 2, 27, 64, 8
		arr.Dump(); // 1, 2, 27, 64, 8
	*/
	
	/*
		var copyMappedArr = arr.MapAndCopy(a => (int)Math.Pow(a, 3), 2);
		copyMappedArr.Dump(); // 1, 2, 27, 64, 8
		arr.Dump(); // 1, 2, 3, 4, 2
	*/
}


public static class ArrayExtensions
{
	public static int[] FillAndCopy(this int[] array,int value,int startIndex = 0)
	{
		var tempArray = array.ToArray();
		foreach (ref int item in tempArray.AsSpan(startIndex))
			item = value;	
		
		return tempArray;
	}

	public static int[] Fill(this int[] array, int value, int startIndex = 0)
	{
		foreach (ref int item in array.AsSpan(startIndex))
			item = value;

		return array;
	}
	
	public static int[] Map(this int[] array, Func<int,int> func, int startIndex = 0)
	{
		foreach (ref int item in array.AsSpan(startIndex))
			item = func(item);

		return array;
	}

	public static int[] MapAndCopy(this int[] array, Func<int, int> func, int startIndex = 0)
	{
		var tempArray = array.ToArray();
		foreach (ref int item in tempArray.AsSpan(startIndex))
			item = func(item);

		return tempArray;
	}
}


