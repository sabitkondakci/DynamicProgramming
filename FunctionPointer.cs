
void Main()
{
	unsafe
	{
		int outputUnsafe = CalculateUnsafe(&Sum, 3, 4); // calli IL instruction
		// output -> 7						
	}

	int outputDemo = Demo(Sum, 5, 6);
	// output -> 11
}

static int Sum(int x, int y) => x + y;

//C# provides delegate types to define safe function pointer objects.
//Invoking a delegate involves instantiating a type derived from System.Delegate
//and making a virtual method call to its Invoke method.
//This virtual call uses the callvirt IL instruction.
//In performance critical code paths, using the calli IL instruction is more efficient.
static T Demo<T>(Func<T, T, T> calculate, T x, T y) =>
	calculate(x, y);

//You can define a function pointer using the delegate* syntax.
//delegate* -> calli IL instruction , Func -> callvirt IL instruction
static unsafe T CalculateUnsafe<T>(delegate*<T, T, T> calculate, T x, T y) =>
	calculate(x, y);
