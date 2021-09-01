void Main()
{
	var point = new Point (1, 1);
	point.DistanceFromOrigin.Dump();
	
	var point2 = point with { Y = 10 };
	point2.DistanceFromOrigin.Dump();    // Works
}

record Point (double X, double Y)
{
	double? _distance;
	public double DistanceFromOrigin => _distance ??= Math.Sqrt (X * X + Y * Y);

	protected Point (Point other) => (X, Y) = (other.X, other.Y); 
     // protected Point (Point other) => (X, Y) = other;
}
