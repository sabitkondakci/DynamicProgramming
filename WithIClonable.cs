// LinqPad 7 , .NET6

void Main()
{
	var model = new Model { Date = DateTime.Now, Id = 1, Name = "Joe" };
	
	model.Dump("model");
	
	// model might be null! 
	var immutableModelCopy = model?.With(m => { });

	immutableModelCopy.Dump("immutableModelCopy");
}

public class Model : ICloneable
{
	public string Name { get; set; }
	public DateTime Date { get; set; }

	public int Id { get; set; }
	public int[] RefTest { get; set; }

	public Model() => RefTest = new int[2];

	private Model(Model model)
	{
		Name = model.Name;
		Date = model.Date;
		Id = model.Id;
		RefTest = model.RefTest;
	}

	object ICloneable.Clone() => new Model(this);
}


public static class ImmutableHelper
{
	public static T With<T>(this T model, Action<T> action)
		where T : ICloneable
	{
		var tempT = (T) model.Clone();
		
		// handle model != null by model?.With()
		if (action != null)
			action(tempT);

		return tempT;
	}
}
