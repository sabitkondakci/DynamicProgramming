void Main()
{
	InvokeLicencePool();
	ReinvokeLicencePool();
}

sealed class SingletonArrayPool<T>
{
	private static readonly Lazy<ArrayPool<T>> _singleton = 
	new Lazy<ArrayPool<T>>(() => ArrayPool<T>.Shared);
	private SingletonArrayPool() { }
	public static ArrayPool<T> Shared => _singleton.Value;
}

public record Licence
{
	public int Id { get; init; }
	public string HairColor { get; init; }
	public string EyeColor { get; init; }
	public ushort WeightInPound { get; init; }
	public byte Age { get; init; }
	// 20 more properties ...
}

public void InvokeLicencePool()
{
	var licencePool = SingletonArrayPool<Licence>.Shared;
	var licenceArr = licencePool.Rent(10);
	for (int i = 0; i < 10; i++)
	{
		licenceArr[i] = new()
		{
			Id = 0,
			EyeColor = "Black",
			HairColor = "Black",
			WeightInPound = 155,
			Age = 33
		};
	}
	
	licenceArr.Dump();
	licencePool.Return(licenceArr);
}

public void ReinvokeLicencePool()
{
	var licencePool = SingletonArrayPool<Licence>.Shared;
	var licenceArr = licencePool.Rent(10);
	licenceArr.Dump();
}
