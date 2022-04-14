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
	public double MutableReadOnlyAddByNoCopy()
	{
		return add_by_type(mutableStructWithReadonlyGet);
	}

	[Benchmark]
	public double MutableReadonlyAddByDefensiveCopy()
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

//|                      Method |      Mean |     Error |    StdDev |    Median |
//| --------------------------- | ---------:| ---------:| ---------:| ---------:|
//| MutableReadOnlyAddByType    | 7.6793 ns | 0.0799 ns | 0.0747 ns | 7.7027 ns |
//| MutableReadonlyAddByRefType | 0.0039 ns | 0.0130 ns | 0.0115 ns | 0.0000 ns |
