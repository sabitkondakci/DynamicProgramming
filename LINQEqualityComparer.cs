void Main()
{
	var itemList = new List<Tool>()
	{
		new Tool(){ },
		null,
		null,
		new Tool() { }
	};

	var distList = itemList.Distinct(new ToolComparer());
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
