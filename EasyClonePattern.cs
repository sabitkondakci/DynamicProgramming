// .NET6 , LINQPad 7 

void Main()
{
	var model = new Model { Date = DateTime.Now, Name = "Joe" };
	
	model.Dump("model");

	var easyClone = model.With(x => { x.Date = DateTime.Today; });
	
	easyClone.Dump("easyClone");
}

public class Model : ICloneable
{
	public string Name { get; set; }
	public DateTime Date { get; set; }

	public int Id { get; set; }
	public int[] RefTest { get; set; }
	
	public Model() { }
	
	private Model(Model model)
	{
		Name = model.Name;
		Date = model.Date;
		Id = model.Id;
		RefTest = model.RefTest;
	}

	object ICloneable.Clone() => new Model(this);
}


public static class CloneHelper
{
	public static T With<T>(this T model, Action<T> action)
		where T : ICloneable
	{
		var tempT = (T)model?.Clone();
		
		if (tempT != null && action != null)
			action(tempT);

		return tempT;
	}
}
