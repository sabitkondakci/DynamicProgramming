void Main()
{
	ICustomer guess = new Guess();
	ICustomer.SetLoyaltyThresholds(TimeSpan.FromDays(10),20,0.3m);
	guess.ComputeLoyaltyDiscount().Dump();
}

interface ICustomer : IOrder
{
	IEnumerable<IOrder> PreviousOrders { get;}
	
	DateTime DateJoined { get;}
	DateTime? LastOrder { get;}
	string Name { get;}
	IDictionary<DateTime,string> Reminders { get;}

	public static void SetLoyaltyThresholds(
		TimeSpan ago,
		int minimumOrders = 10,
		decimal percentageDiscount = 0.10m)
	{
		length = ago;
		orderCount = minimumOrders;
		discountPercent = percentageDiscount;
	}
	private static TimeSpan length = new TimeSpan(365 * 2, 0, 0, 0); // two years
	private static int orderCount = 10;
	private static decimal discountPercent = 0.10m;

	public decimal ComputeLoyaltyDiscount() => DefaultLoyaltyDiscount(this);
	protected static decimal DefaultLoyaltyDiscount(ICustomer c)
	{
		DateTime start = DateTime.Now - length;

		if ((c.DateJoined < start) && (c.PreviousOrders.Count() > orderCount))
		{
			return discountPercent;
		}
		return 0;
	}
}

interface IOrder
{
	DateTime Purchased {get;}
	float Cost {get;}
}

class Guess : ICustomer
{
	
	public IEnumerable<IOrder> PreviousOrders
	{
		get 
		{
			var dummyList = new List<IOrder>();
			dummyList.Add(new Cupcake());
			dummyList.Add(new Beer());
			return dummyList;
		}
	}
	
	public DateTime DateJoined => DateTime.Now.AddYears(-4);

	public DateTime? LastOrder => DateTime.UtcNow.AddDays(-6);

	public string Name => "Daniel Sutto";

	public IDictionary<DateTime, string> Reminders =>
	new Dictionary<DateTime,string>();

	public DateTime Purchased => DateTime.UtcNow;

	public float Cost => 122.4f;

	public decimal ComputeLoyaltyDiscount()
	{
		if (PreviousOrders.Any() == false)
			return 0.50m;
		else
			return ICustomer.DefaultLoyaltyDiscount(this);
	}

}

class Cupcake : IOrder
{
	public DateTime Purchased => default(DateTime);

	public float Cost => default(float);
}

class Beer : IOrder
{
	public DateTime Purchased => default(DateTime);

	public float Cost => default(float);
}
