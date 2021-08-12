public static class ExtentionCollection
{
	public static bool HasDuplicate<T>(this ICollection<T> collection, out int indexOfDuplicateValue) 
	where T : struct
	{		
		var localCollection = collection;
		
		if(localCollection is null)
			throw new NullReferenceException();
		if(localCollection.Count == 0)
			throw new Exception("Collection is empty");
			
		HashSet<T> list = new();			 	
		var result = !localCollection.All(c => list.Add(c) );
		indexOfDuplicateValue = result ? list.Count : -1;
		
		return result;
	}

	public static bool HasDuplicate(this ICollection<string> collection,
		out int indexOfDuplicateValue, bool isCaseSensitive = false)
	{
		var localCollection = collection;
		
		if (localCollection is null)
			throw new NullReferenceException();
		if (localCollection.Count == 0)
			throw new Exception("Collection is empty");
			
		localCollection = !isCaseSensitive ?
		collection.Select(c => c?.ToLower() ).ToArray() : localCollection ;

		HashSet<string> list = new();
		var result = !localCollection.All(c => list.Add(c));
		indexOfDuplicateValue = result ? list.Count : -1;	
		
		return result;
	}
}
