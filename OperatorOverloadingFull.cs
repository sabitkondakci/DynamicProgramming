

public class Coordinate
{
	public int X { get; private set; }
	public int Y { get; private set; }
	public int Z { get; private set; }
	
	public Coordinate(int x, int y, int z)
	{
		this.X = x;
		this.Y = y;
		this.Z = z;
	}
	
	public static Coordinate operator +(in Coordinate a, in Coordinate b) => 
	new Coordinate (a.X + b.X , a.Y + b.Y , a.Z + b.Z);

	public static Coordinate operator -(in Coordinate a, in Coordinate b) =>
	new Coordinate(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

	public static Coordinate operator *(in Coordinate a, in Coordinate b) =>
	new Coordinate(a.X * b.X, a.Y * b.Y, a.Z * b.Z);

	public static bool operator ==(in Coordinate a, in Coordinate b)
	{
		if(a.X == b.X && a.Y == b.Y && a.Z == b.Z)
			return true;
		
		return false;
	}
	
	public static bool operator !=(in Coordinate a, in Coordinate b) => !(a == b);
	
	public static Coordinate operator ++(Coordinate a) =>
	new Coordinate(++a.X,++a.Y,++a.Z);

	public static Coordinate operator --(Coordinate a) =>
	new Coordinate(--a.X, --a.Y, --a.Z);
	
	public static Coordinate operator %(in Coordinate a, in Coordinate b) 
	{
		if( b.X != 0 && b.Y != 0 && b.Z != 0)
			return new Coordinate(a.X % b.X, a.Y % b.Y, a.Z % b.Z);
		else
			throw new DivideByZeroException();
	}

	public static Coordinate operator /(in Coordinate a, in Coordinate b)
	{ 
		if( b.X != 0 && b.Y != 0 && b.Z != 0)
			return new Coordinate(a.X / b.X, a.Y / b.Y, a.Z / b.Z);	
		else
			throw new DivideByZeroException();
	}
	
	public static (double X, double Y, double Z) operator 
	^(in Coordinate a, in Coordinate b)
	{
		double x = Math.Pow(a.X, b.X);
		double y = Math.Pow(a.Y, b.Y);
		double z = Math.Pow(a.Z, b.Z);
		
		return (x,y,z);
	}

	public override bool Equals(object obj)
	{
		if(obj is not null)
		{
			Coordinate coordinate = (Coordinate)obj;
			return this == coordinate;
		}	
		return false;
	}

	public override int GetHashCode()
	{
		string hash = $"{X}+{Y}+{Z}";
		return hash.GetHashCode();
	}
}
