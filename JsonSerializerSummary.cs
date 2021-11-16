using System.Text.Json;
using System.Text.Encodings.Web;
using System.Net.Http.Json;

async Task Main()
{
	var store = new WareHouse()
	{
		BrandName = @"Muggy\",
		Amount = 1200,
		
		Price = new Dictionary<string,double>
		{
			["Max"] = 1244.33442,
			["Min"] = 988.4323
		},
		
		PurchaseDate = new DateTime(2021,10,29),
		
		TemperatureRanges = new Dictionary<string, HighLowTempCelcius>
		{
			["Cold"] = new HighLowTempCelcius { High = 20, Low = -10 },
			["Hot"] = new HighLowTempCelcius { High = 35, Low = 21 },
			["Humid"] = new HighLowTempCelcius { High = 60, Low = 36 },
		},
		
		ItemCategory = Category.Home_Gadget
	}; 

	var jsonOptions = new JsonSerializerOptions()
	{
		WriteIndented = true,
		IgnoreReadOnlyProperties = true,
		
		//Custom Property Naming Policy
		PropertyNamingPolicy = new LowerCaseNamingPolicy(),
		
		// PropertyNamingPolicy = JsonNamingPolicy.CamelCase: built-in policy
		
		// The camel case naming policy for dictionary keys
		// applies to serialization only.
		// If you deserialize a dictionary, the keys will match the JSON file 
		// even if you specify JsonNamingPolicy.CamelCase for the DictionaryKeyPolicy.
		DictionaryKeyPolicy = new UpperCaseNamingPolicy(),
		
		//Allows comments within the JSON input and ignores them.
		//The Utf8JsonReader behaves as if no comments are present.
		ReadCommentHandling = JsonCommentHandling.Skip,
		AllowTrailingCommas = true,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
		
		// Writes numbers in string notation as "12"
		NumberHandling =
			JsonNumberHandling.AllowReadingFromString |
					JsonNumberHandling.WriteAsString,

		Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
		// by default enums are serialized as numbers
		// by the help of JsonStringEnumConverter()
		// this behaviour may change.
		Converters =
		{
			new JsonStringEnumConverter(), //JsonNamingPolicy is optional
			new DateTimeOnlyDateConverter_Turkey(),	
		}
	};
	
	await using (var stream = new MemoryStream())
	{
		
		// Serialize
		await JsonSerializer.
			SerializeAsync<WareHouse>(stream, store, jsonOptions);
		
		stream.Seek(0,SeekOrigin.Begin);

		using var streamReader = new StreamReader(stream,Encoding.UTF8,
			false,stream.Capacity);
					
		var script = await streamReader.ReadToEndAsync();
		script.Dump();
				
		//Deserialize
		stream.Seek(0,SeekOrigin.Begin);
		
		WareHouse wareObject = await JsonSerializer.
			DeserializeAsync<WareHouse>(stream,jsonOptions);

		wareObject.Dump();
	}
	
}

//Adding a Custom Property Naming Policy
public class LowerCaseNamingPolicy : JsonNamingPolicy
{
	public override string ConvertName(string name) => name.ToLower();
}

// Dictionay Key Policy
public class UpperCaseNamingPolicy : JsonNamingPolicy
{
	public override string ConvertName(string name) => name.ToUpper();
}

public class WareHouse : IStorage, ITempConditions
{
	[JsonIgnore]
	public int ItemId { get; }
	public string BrandName { get; set; }
	public double Amount { get; set; }
	
	[JsonConverter(typeof(RoundFractionConverter))]
	public Dictionary<string,double> Price {get;set;}
	
	public DateTime PurchaseDate { get; set; }
	public Dictionary<string,HighLowTempCelcius> TemperatureRanges {get;set;}
	
	[JsonPropertyName("KeyWords")] // overrides JsonNamingPolicy.CamelCase
	public string[] TempKeyWords => ITempConditions.DefaultTempKeyWords;
	
	[JsonInclude] // include fields, except static, const ones
	public byte OptimalTemp = 22;
	public Category ItemCategory {get;set;}
	public string ItemCategoryString(Category category)
	{
		return category switch 
		{
			Category.Tool => nameof(Category.Tool),
			Category.Hygene => nameof(Category.Hygene),
			Category.Home_Gadget => nameof(Category.Home_Gadget),
			Category.Food => nameof(Category.Food),
			Category.Electronic_Material => nameof(Category.Electronic_Material),
			Category.Car_Spare_Part => nameof(Category.Car_Spare_Part),
			Category.Bike_Spare_Part => nameof(Category.Bike_Spare_Part),
			_ => nameof(Category.None)
		};
	}
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
	public string BrandName { get; set; }
	public double Amount { get; set; }
	public DateTime PurchaseDate { get; set; }
}

interface ITempConditions
{
	public Dictionary<string, HighLowTempCelcius> TemperatureRanges { get; set; }
	protected static string[] DefaultTempKeyWords => new[] { "Cold", "Humid", "Hot"};
}

public struct HighLowTempCelcius
{
	public int High { get; init; }
	public int Low { get; init; }
}

public class DateTimeOnlyDateConverter_Turkey : JsonConverter<DateTime>
{
	public override DateTime 
		Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		CultureInfo culture = new CultureInfo("tr-TR");
		
		if(typeToConvert == typeof(DateTime))
			return DateTime.Parse(reader.GetString(),culture);
		
		return new DateTime();
	}

	public override void
		Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
	{
		var year = value.Year;
		var month = value.Month;
		var day = value.Day;

		var date = $"{day}/{month}/{year}"; // DateTime format in Turkey
		writer.WriteStringValue(date);
	}
}



public class RoundFractionConverter : JsonConverter<Dictionary<string, double>>
{
	public override Dictionary<string, double> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType != JsonTokenType.StartObject)
		{
			throw new JsonException($"JsonTokenType was of type {reader.TokenType}, only objects are supported");
		}

		var dictionary = new Dictionary<string, double>();
		
		while (reader.Read())
		{
			if (reader.TokenType == JsonTokenType.EndObject)
			{
				return dictionary;
			}

			if (reader.TokenType != JsonTokenType.PropertyName)
			{
				throw new JsonException("JsonTokenType was not PropertyName");
			}

			var propertyName = reader.GetString();

			if (string.IsNullOrWhiteSpace(propertyName))
			{
				throw new JsonException("Failed to get property name");
			}

			reader.Read();
			dictionary.Add(propertyName, GetDictValue(ref reader, options));
		}

		return dictionary;
	}

	public override void Write(Utf8JsonWriter writer, Dictionary<string, double> value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
			writer.WriteNumber("Max",Math.Round(value["Max"],2));
			writer.WriteNumber("Min",Math.Round(value["Min"],2));
		writer.WriteEndObject();
		writer.Flush();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private double GetDictValue(ref Utf8JsonReader reader, JsonSerializerOptions options)
	{
		if(reader.TokenType == JsonTokenType.Number)
		{
			double value = reader.GetDouble();
			return Math.Round(value,2);
		}
		
		return default(double);
	}
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
