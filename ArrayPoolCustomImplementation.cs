
void Main()
{
	GenerateLicencePlate(10); // first creation with Shared.Return
	SharedMemory(10); // rent already created array of WisconsinDriverLicenseInfo
}

public void SharedMemory(int nItemAtATime)
{
	WisconsinDLPool wisPoolObject = new();
	var pooledList = wisPoolObject.Rent(nItemAtATime); // WisconsinDriverLicenseInfo[]
	pooledList.Dump();
}

#region Enum_Hair&Eye_Color
public enum HairColor : byte
{
	None,
	Black,
	Yellow,
	White,
	Red,
	Brown,
	PaintedMi
}

public enum EyeColor : byte
{
	None,
	Black,
	Blue,
	Green,
	Brown,
	Brown_Amber,
	Gray,
	Hazel
}
#endregion

sealed class SingletonArrayPool<T>
{
	private static readonly Lazy<ArrayPool<T>> _singleton =
	new Lazy<ArrayPool<T>>(() => ArrayPool<T>.Shared);
	private SingletonArrayPool() { }
	public static ArrayPool<T> Shared => _singleton.Value;
}

public record WisconsinDriverLicenseInfo
{
	public string licenceId { get; init; }
	public char licenceClass { get; init; }
	public char sex { get; init; }
	public string address { get; init; }
	public ushort weightPound { get; init; }
	public ushort heightInch { get; init; }
	public DateTime dateOfBirth { get; init; }
	public DateTime expirationDate { get; init; }
	public HairColor hairColor { get; init; }
	public EyeColor eyeColor { get; init; }
	public bool IsDonor { get; init; }
	public DateTime ISS { get; init; }
}

public void GenerateLicencePlate(int nItemAtATime)
{
	HairColor[] hairColors = Enum.GetValues<HairColor>();
	EyeColor[] eyeColors = Enum.GetValues<EyeColor>();
	Random randomHairColor = new();
	Random randomEyeColor = new();

	WisconsinDLPool wisPoolObject = new();
	var wisconsinDLInfoArray = wisPoolObject.Rent(nItemAtATime);

	for (int i = 0; i < nItemAtATime; i++)
	{
		int randomHCNext = randomHairColor.Next(1, hairColors.Length);
		int randomECNext = randomEyeColor.Next(1, eyeColors.Length);

		wisconsinDLInfoArray[i] =
		new WisconsinDriverLicenseInfo()
		{
			licenceId = "licenseId",
			licenceClass = '#',
			sex = '#',
			address = "address",
			weightPound = 1,
			heightInch = 1,
			dateOfBirth = DateTime.UtcNow,
			expirationDate = DateTime.UtcNow,
			hairColor = hairColors[randomHCNext],
			eyeColor = eyeColors[randomECNext],
			IsDonor = true,
			ISS = DateTime.UtcNow
		};
	}

	wisconsinDLInfoArray.Dump();
	wisPoolObject.Return(wisconsinDLInfoArray, true);
}

public class WisconsinDLPool : ArrayPool<WisconsinDriverLicenseInfo>
{

	private int length;

	public override WisconsinDriverLicenseInfo[] Rent(int minimumLength)
	{
		length = minimumLength;
		return SingletonArrayPool<WisconsinDriverLicenseInfo>.
		Shared.Rent(minimumLength);
	}


	public override void Return(WisconsinDriverLicenseInfo[] array,
	bool clearArray = false)
	{
		if (clearArray)
		{
			for (int i = 0; i < length; i++)
			{
				array[i] =
				new WisconsinDriverLicenseInfo()
				{
					licenceId = "#",
					licenceClass = '#',
					sex = '#',
					address = "#",
					weightPound = 0,
					heightInch = 0,
					dateOfBirth = DateTime.MinValue,
					expirationDate = DateTime.MaxValue,
					hairColor = HairColor.None,
					eyeColor = EyeColor.None,
					IsDonor = false,
					ISS = DateTime.MinValue
				};
			}

		}

		SingletonArrayPool<WisconsinDriverLicenseInfo>.
		Shared.Return(array);
	}
}

