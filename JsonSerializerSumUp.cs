using System.Text.Json;
using System.Text.Encodings.Web;
using System.Net.Http.Json;

async Task Main()
{
	var store = new WareHouse()
	{
		BrandName = "Muggy",
		Amount = 1200,
		PurchaseDate = DateTime.Parse("2021-10-13".AsSpan()),
		
		TemperatureRanges = new Dictionary<string, HighLowTempCelcius>
		{
			["Cold"] = new HighLowTempCelcius { High = 20, Low = -10 },
			["Hot"] = new HighLowTempCelcius { High = 35, Low = 21 },
			["Heat Wave"] = new HighLowTempCelcius { High = 60, Low = 36 },
		},
		
		ItemCategory = Category.Home_Gadget
	}; 
	
	var jsonOptions = new JsonSerializerOptions()
	{
		WriteIndented = true, IgnoreReadOnlyProperties = true,
		
		//Custom Property Naming Policy
		PropertyNamingPolicy = new LowerCaseNamingPolicy(),
		
		// PropertyNamingPolicy = JsonNamingPolicy.CamelCase: built-in policy
		
		// The camel case naming policy for dictionary keys
		// applies to serialization only.
		// If you deserialize a dictionary, the keys will match the JSON file 
		// even if you specify JsonNamingPolicy.CamelCase for the DictionaryKeyPolicy.
		DictionaryKeyPolicy = JsonNamingPolicy.CamelCase, // customized dictionary keys
		
		//Allows comments within the JSON input and ignores them.
		//The Utf8JsonReader behaves as if no comments are present.
		ReadCommentHandling = JsonCommentHandling.Skip,
		AllowTrailingCommas = true,
		
		Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
		// by default enums are serialized as numbers
		// by the help of JsonStringEnumConverter()
		// this behaviour may change.
		Converters =
		{
			new JsonStringEnumConverter() //JsonNamingPolicy is optional
		}
	};

	var filePath = @"C:\Users\fenko\Desktop\WareHouse.json";
	
	// Serialize
	await using (FileStream stream = File.Create(filePath))
		await JsonSerializer.
			SerializeAsync<WareHouse>(stream, store, jsonOptions);
	
	string jsonScript = await File.ReadAllTextAsync(filePath);
		
	//Deserialize
	WareHouse wareObject = null;
	await using (FileStream memoStream = File.OpenRead(filePath))
		wareObject = await JsonSerializer.
			DeserializeAsync<WareHouse>(memoStream,jsonOptions);
		
	
}

//Adding a Custom Property Naming Policy
public class LowerCaseNamingPolicy : JsonNamingPolicy
{
	public override string ConvertName(string name) => name.ToLower();
}

public class WareHouse : IStorage, ITempConditions
{
	public int ItemId { get; }
	public string BrandName { get; init; }
	public ulong Amount { get; init; }
	public DateTime PurchaseDate { get; init; }
	public Dictionary<string,HighLowTempCelcius> TemperatureRanges {get;init;}
	
	[JsonPropertyName("KeyWords")] // overrides JsonNamingPolicy.CamelCase
	
	// ITempConditions.DefaulttempKeyWords : default protected static readonly interface property.
	public string[] TempKeyWords => ITempConditions.DefaultTempKeyWords; 
	
	[JsonInclude] // include fields, except static, const ones
	public byte OptimalTemp = 22;
	public Category ItemCategory {get;init;}
	
}

public enum Category 
{
	None,
	Food,
	Home_Gadget,
	Tool,
	Electronic_Material,
	Car_Spare_Part,
	Bike_Spare_Part,
	Hygene
}

interface IStorage
{
	protected static readonly short SmallSize = 30_000;
	protected static readonly int MidSize = 500_000;
	protected static readonly int LargeSize = 1_000_000;

	public int ItemId { get; }
	public string BrandName { get; init; }
	public ulong Amount { get; init; }
	public DateTime PurchaseDate { get; init; }
}

interface ITempConditions
{
	public Dictionary<string, HighLowTempCelcius> TemperatureRanges { get; init; }
	protected static string[] DefaultTempKeyWords => new[] { "Cold", "Humid", "Hot"};
}

public struct HighLowTempCelcius
{
	public int High { get; init; }
	public int Low { get; init; }
}


// HttpClient GetFromJsonAsync<T>(requestUri)
namespace HttpClientExtensionMethods
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
	}

	public class Program
	{
		public static async Task Main()
		{
			using HttpClient client = new()
			{
				BaseAddress = new Uri("https://jsonplaceholder.typicode.com")
			};

			// Get the user information.
			User user = await client.GetFromJsonAsync<User>("users/7");
			Console.WriteLine($"Id: {user.Id}");
			Console.WriteLine($"Name: {user.Name}");
			Console.WriteLine($"Username: {user.Username}");
			Console.WriteLine($"Email: {user.Email}");

			// Post a new user.
			HttpResponseMessage response =
			await client.PostAsJsonAsync("users", user);
			Console.WriteLine(
				$"{(response.IsSuccessStatusCode ? "Success" : "Error")}"
				+ $"- {response.StatusCode}");
		}
	}
}
