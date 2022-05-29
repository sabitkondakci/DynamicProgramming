// LinqPad 7 , .NET6

void Main()
{
	var model = new Model { Date = DateTime.Now, Name = "Joe" };
	
	model.Dump("model");

	var immutableModelCopy = model.With(m => new Model(m)
	{
		Name = "Sabit",
		Id = 43
	});
	
	immutableModelCopy.Dump("immutableModelCopy");
}

public class Model : ICloneable
{
	public string Name { get; init; }
	public DateTime Date { get; init; }

	public int Id { get; init; }
	public int[] RefTest { get; init; }

	public Model() { }

	public Model(Model model)
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
	public static T With<T>(this T model, Func<T, T> action)
		where T : ICloneable
	{
		var tempT = (T)model?.Clone();

		if (tempT != null && action != null)
			tempT = action(tempT);

		return tempT;
	}
}
