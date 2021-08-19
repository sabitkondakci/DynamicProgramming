
void Main()
{
	string result = EnumToString(Color.Blue);
	Console.WriteLine(result);
}

enum Color {
	Blue,
	Green,
	Yellow
}

private static string EnumToString(Color enumerator) 
{
	switch (enumerator)
	{
		case Color.Blue:
			return nameof(Color.Blue);
		case Color.Green:
			return nameof(Color.Green);
		case Color.Yellow:
			return nameof(Color.Yellow);
		default:
			throw new ArgumentOutOfRangeException(nameof(enumerator),enumerator,null);
	}
}
