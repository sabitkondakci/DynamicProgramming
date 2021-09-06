

void Main()
{
	pint64 a = 12; // --> implicit operator pint64(long number) => new pint64(number);
	pint64 b = 2; // --> implicit operator pint64(long number) => new pint64(number);
	nuint s =3;
	double t = a^b; // t = 144 --> double operator ^(pint64 first, pint64 second) => Math.Pow(first._x,second._x);	
	
	long k = a; // k = 12 --> implicit operator long(pint64 p) => p._x;

}

struct pint64
{
	private long _x;
	public pint64(long x)
	{
		this._x = x;
	}	
	public static double operator ^(pint64 first, pint64 second) => Math.Pow(first._x,second._x);	
	public static implicit operator long(pint64 p) => p._x;
	public static implicit operator pint64(long number) => new pint64(number);
}
