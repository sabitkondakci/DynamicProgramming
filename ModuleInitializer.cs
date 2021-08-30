class Program
{
	// This method runs automatically when the containing assembly is first loaded.
	// public or internal
	[System.Runtime.CompilerServices.ModuleInitializer]
	public static void Initializer() 
	{
		Console.WriteLine("This runs only once throughout application's lifetime");
		// Module initializers have less overhead than static constructors.
		// This feature requires .NET 5
	}
}
