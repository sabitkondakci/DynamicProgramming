namespace ValueEqualityClass
{
	class TwoDPoint : IEquatable<TwoDPoint>
	{
		public int X { get; private set; }
		public int Y { get; private set; }

		public TwoDPoint(int x, int y)
		{
			if (x is (< 1 or > 640) || y is (< 1 or > 480))
			{
				throw new ArgumentException("640x480 2D Model,  1 <= x <= 640 & 1 <= y <= 480");
			}
			this.X = x;
			this.Y = y;
		}
		
		// TwoDPoint clone ctor
		protected TwoDPoint(TwoDPoint pointObject)
		{
			X = pointObject.X;
			Y = pointObject.Y;
		}

		protected virtual Type EqualityContract => typeof(TwoDPoint);
		
		public virtual TwoDPoint Clone() => new TwoDPoint(this);

		public override bool Equals(object obj) => this.Equals(obj as TwoDPoint);
		
		// it's virtual in record so that I declared it "virtual"
		public virtual bool Equals(TwoDPoint point)
		{
			if (point is null)
				return false;

			// If run-time types are not exactly the same, return false.
			if (this.EqualityContract != point.EqualityContract)
				return false;

			// Optimization for a common success case.
			if (object.ReferenceEquals(this, point))
				return true;

			// Return true if the fields match.
			// Note that the base class is not invoked because it is
			// System.Object, which defines Equals as reference equality.
			return (X == point.X) && (Y == point.Y);
		}

		public override int GetHashCode() => (X, Y).GetHashCode();

		public static bool operator ==(TwoDPoint left, TwoDPoint right)
		{
			if (left is null)
			{
				if (right is null)
				{
					return true;
				}
				// Only the left side is null.
				return false;
			}
			// Equals handles case of null on right side.
			return left.Equals(right);
		}

		public static bool operator !=(TwoDPoint left, TwoDPoint right) => !(left == right);
	}

	// For the sake of simplicity, assume a ThreeDPoint IS a TwoDPoint.
	class ThreeDPoint : TwoDPoint, IEquatable<ThreeDPoint>
	{
		public int Z { get; private set; }

		public ThreeDPoint(int x, int y, int z) : base(x, y)
		{
			if ((z < 1) || (z > 2000))
			{
				throw new ArgumentException("Point must be in range 1 - 2000");
			}
			this.Z = z;
		}
		
		// ThreeDPoint clone ctor
		protected ThreeDPoint(ThreeDPoint pointObj):base(pointObj)
		{			
			Z = pointObj.Z;
		}
		
		public override TwoDPoint Clone() => new ThreeDPoint(this);		

		protected override Type EqualityContract => typeof(ThreeDPoint);

		public override bool Equals(object obj) => this.Equals(obj as ThreeDPoint);

		public virtual bool Equals(ThreeDPoint point)
		{
			if (point is null)
				return false;

			// If run-time types are not exactly the same, return false.
			if (this.EqualityContract != point.EqualityContract)
				return false;

			// Optimization for a common success case.
			if (Object.ReferenceEquals(this, point))
				return true;

			// Check properties that this class declares.
			if (this.Z == point.Z)
			{
				// Let base class check its own fields
				// and do the run-time type comparison.
				return base.Equals(point as TwoDPoint);
			}

			return false;

		}

		public override int GetHashCode() => (X, Y, Z).GetHashCode();

		public static bool operator ==(ThreeDPoint left, ThreeDPoint right)
		{
			if (left is null)
			{
				if (right is null)
				{
					// null == null = true.
					return true;
				}

				// Only the left side is null.
				return false;
			}
			// Equals handles the case of null on right side.
			return left.Equals(right);
		}

		public static bool operator !=(ThreeDPoint left, ThreeDPoint right) => !(left == right);
	}

	class Program
	{
		static void Main(string[] args)
		{
			TwoDPoint pointT = new TwoDPoint(3, 4);
			ThreeDPoint pointA = new ThreeDPoint(3, 4, 5);
			ThreeDPoint pointB = new ThreeDPoint(3, 4, 5);
			ThreeDPoint pointC = null;
			
			var pointTClone = pointT.Clone();
			var pointBClone = pointB.Clone();

			string i = "Comparer";

			Console.WriteLine("pointA.Equals(pointB) = {0}", pointA.Equals(pointB)); // true
			Console.WriteLine("pointA == pointB = {0}", pointA == pointB); // true
			Console.WriteLine("null comparison = {0}", pointA.Equals(pointC)); // false
			Console.WriteLine("Compare to some other type = {0}", pointA.Equals(i)); // false

			TwoDPoint pointD = null;
			TwoDPoint pointE = null;

			Console.WriteLine("Two null TwoDPoints are equal: {0}", pointD == pointE); // true

			pointE = new TwoDPoint(3, 4);
			Console.WriteLine("(pointE == pointA) = {0}", pointE == pointA); // false
			Console.WriteLine("(pointA == pointE) = {0}", pointA == pointE); // false
			Console.WriteLine("(pointA != pointE) = {0}", pointA != pointE); // true

			System.Collections.ArrayList list = new System.Collections.ArrayList();
			list.Add(new ThreeDPoint(3, 4, 5));

			Console.WriteLine("pointE.Equals(list[0]): {0}", pointE.Equals(list[0])); // false
			Console.WriteLine("pointA.Equals(list[0]): {0}", pointA.Equals(list[0])); // true

		}
	}

}
