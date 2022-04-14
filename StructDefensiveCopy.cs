// LinqPad6 

class Program
{
	static void Main(string[] args)
	{
		var Summary = BenchmarkRunner.Run<Benchmarks>();
	}
}

public struct MutableStructReadonlyGet
{
	public double X { readonly get => x; set => x = value; }
	public double Y { readonly get => y; set => y = value; }
	public double Z { readonly get => z; set => z = value; }

	private double z;
	private double y;
	private double x;
	
	// mocking a big struct by adding dummy parameters
	private double a;
	private double b;
	private double c;
	private double d;
	private double e;
	private double f;
	private double g;
	private double h;
	
	public MutableStructReadonlyGet(double x = 0, double y = 0, double z = 0)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		
		this.a = 1;
		this.b = 2;
		this.c = 3;
		this.d = 4;
		this.e = 5;
		this.f = 6;
		this.g = 7;
		this.h = 8;
	}
}

public class Benchmarks
{
	MutableStructReadonlyGet mutableStructWithReadonlyGet;

	[GlobalSetup]
	public void Setup()
	{
		mutableStructWithReadonlyGet = new MutableStructReadonlyGet(1.9, 9.3);
	}

	[Benchmark]
	public double MutableReadOnlyAddByDefensiveCopy()
	{
		return add_by_type(mutableStructWithReadonlyGet);
	}

	[Benchmark]
	public double MutableReadonlyAddByNoCopy()
	{
		return add_by_in(in mutableStructWithReadonlyGet);
	}

	public double add_by_type(MutableStructReadonlyGet s)
	{
		return s.X + s.Y;
	}
	public double add_by_in(in MutableStructReadonlyGet s)
	{
		return s.X + s.Y;
	}
}

//|                               Method |      Mean |     Error |    StdDev |    Median |
//| ------------------------------------ | ---------:| ---------:| ---------:| ---------:|
//| MutableReadOnlyAddByDefensiveCopy    | 7.6793 ns | 0.0799 ns | 0.0747 ns | 7.7027 ns |
//| MutableReadonlyAddByNoCopy           | 0.0039 ns | 0.0130 ns | 0.0115 ns | 0.0000 ns |


/* 
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.1586 (21H2)
Intel Core i5-3210M CPU 2.50GHz (Ivy Bridge), 1 CPU, 4 logical and 2 physical cores .NET SDK=6.0.200 
[Host] : .NET 5.0.15 (5.0.1522.11506), X64 RyuJIT 
*/
