// LinqPad Environment

void Main()
{
	var itemList = new List<Tool>()
	{
		new Tool(){ ID = 1, Name = "Pump", ProductionDate = DateTime.Today },
		new Tool() { ID = 2, Name = "Toy", ProductionDate = DateTime.Today.AddDays(-63) },
		new Tool() { ID = 2, Name = "Toy", ProductionDate = DateTime.Today.AddDays(-63) },
		new Tool() { ID = 3, Name = "Faucet", ProductionDate = DateTime.Today.AddDays(-3) }
	};

	var distList = itemList.Distinct(ToolComparer.GetInstance());
	distList.Dump();
	
}

public class Tool
{
	public int ID { get; set; }
	public string Name { get; set; }
	public DateTime ProductionDate { get; set; }
}

public class ToolComparer : IEqualityComparer<Tool>
{

	public static ToolComparer GetInstance() => _comparer.Value;
	
	private ToolComparer() { }

	// Lazy<> class ensures thread safety , .Net 4.0+
	private static readonly Lazy<ToolComparer> _comparer =
		new Lazy<ToolComparer>(() => new ToolComparer(), LazyThreadSafetyMode.ExecutionAndPublication);
		
	// Second, once a similar GetHashCode returns, Eqauls() is checked.
	public bool Equals(Tool x, Tool y)
	{
		if(ReferenceEquals(x,y))
			return true;
		
		if(x is null || y is null)
			return false;
		
		return
			x.Name == y.Name &&
			x.ProductionDate == y.ProductionDate;
	}

	// First, itemList element is compared against GetHasCode of ToolComparer
	public int GetHashCode(Tool obj)
	{
		if(obj is null)
			return -1;
		
		return (obj.ID).GetHashCode();
	}
}
