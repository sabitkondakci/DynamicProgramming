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
		
		// Fahrenheit or Celsius
		Temp = new Temperature(80,celsius:false),
		
		PurchaseDate = new DateTime(2021,10,29),
		TemperatureRanges = new Dictionary<string, HighLowTempCelcius>
		{
			["Cold"] = new HighLowTempCelcius { High = 20, Low = -10 },
			["Hot"] = new HighLowTempCelcius { High = 35, Low = 21 },
			["Humid"] = new HighLowTempCelcius { High = 60, Low = 36 },
		},
		
		// JsonElement issue has been solved by
		// new DictionaryStringObjectJsonConverter() implementation
		DummyElement = new Dictionary<string,object>
		{
			["Bool"] = true,
			["Null"] = null,
			["Text"] = "String",
			["Double"] = 332.44,
			["Integer"] = 342
		},
		
		ItemCategory = Category.Home_Gadget
	}; 

	var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
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
			new DictionaryStringObjectJsonConverter()
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


public class WareHouse : IStorage, ITempConditions
{
	[JsonIgnore]
	public int ItemId { get; }
	public string BrandName { get; set; }
	public double Amount { get; set; }
	
	[JsonConverter(typeof(RoundFractionConverter))]
	public Dictionary<string,double> Price {get;set;}
	
	public Dictionary<string,object> DummyElement {get;set;}
	
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
	
	public Temperature Temp {get;set;}
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
	public Temperature Temp {get;set;}
	protected static string[] DefaultTempKeyWords => new[] { "Cold", "Humid", "Hot"};
}

public struct HighLowTempCelcius
{
	public int High { get; init; }
	public int Low { get; init; }
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

// [JsonConverter] applied to a property.
// A converter added to the Converters collection.
// [JsonConverter] applied to a custom value type or POCO.
[JsonConverter(typeof(TemperatureConverter))]
public struct Temperature
{
	public Temperature(double degrees, bool celsius)
	{	
		Degrees = degrees;
		IsCelsius = celsius;
		Celcius = celsius ? degrees : Math.Round((degrees - 32)/1.8,2);
	}

	public double Degrees { get;}
	public double Celcius { get;}
	public bool IsCelsius { get; }
	public bool IsFahrenheit => !IsCelsius;

	public override string ToString() =>
		$"{Degrees}{(IsCelsius ? "C" : "F")}";

	public static Temperature Parse(string input)
	{
		double degrees = int.Parse(input[..^1]);
		bool celsius = input[^1] == 'C';

		return new Temperature(degrees, celsius);
	}
}

public class TemperatureConverter : JsonConverter<Temperature>
{
	public override Temperature Read(
		ref Utf8JsonReader reader,
		Type typeToConvert,
		JsonSerializerOptions options)
	{
		Utf8JsonReader readerClone = reader;
		reader = readerClone;
		return Temperature.Parse(readerClone.GetString());
	}
	
	public override void Write(
		Utf8JsonWriter writer,
		Temperature temperature,
		JsonSerializerOptions options) => 
			writer.WriteStringValue(temperature.ToString());
}

public class DateTimeOnlyDateConverter_Turkey : JsonConverter<DateTime>
{
	public override DateTime 
		Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		// struct can be assigned by assignment
		Utf8JsonReader readerClone = reader;
		
		CultureInfo culture = new CultureInfo("tr-TR");

		if (typeToConvert == typeof(DateTime))
		{
			reader = readerClone;
			return DateTime.Parse(readerClone.GetString(), culture);
		}
		
		reader = readerClone;
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
	public override Dictionary<string, double> 
		Read(ref Utf8JsonReader reader, 
			Type typeToConvert, JsonSerializerOptions options)
	{
		Utf8JsonReader readerClone = reader;
		
		if (readerClone.TokenType != JsonTokenType.StartObject)
		{
			throw new JsonException($"JsonTokenType was of type {readerClone.TokenType}, only objects are supported");
		}

		var dictionary = new Dictionary<string, double>();
		
		while (readerClone.Read())
		{
			if (readerClone.TokenType == JsonTokenType.EndObject)
			{
				reader = readerClone;
				return dictionary;
			}

			if (readerClone.TokenType != JsonTokenType.PropertyName)
			{
				throw new JsonException("JsonTokenType was not PropertyName");
			}

			var propertyName = readerClone.GetString();

			if (string.IsNullOrWhiteSpace(propertyName))
			{
				throw new JsonException("Failed to get property name");
			}

			readerClone.Read();
			dictionary.Add(propertyName, GetRoundDictValue(ref readerClone, options));
		}
		
		reader = readerClone;
		return dictionary;
	}

	public override void 
		Write(Utf8JsonWriter writer, 
			Dictionary<string, double> value, JsonSerializerOptions options)
	{
		writer.WriteStartObject();
			writer.WriteNumber("Max",Math.Round(value["Max"],2));
			writer.WriteNumber("Min",Math.Round(value["Min"],2));
		writer.WriteEndObject();
		
	}

	private double 
		GetRoundDictValue(ref Utf8JsonReader reader, JsonSerializerOptions options)
	{
		Utf8JsonReader readerClone = reader;
		
		if(readerClone.TokenType == JsonTokenType.Number)
		{
			double value = readerClone.GetDouble();
			return Math.Round(value,2);
		}
		
		return default(double);
	}
}

public class DictionaryStringObjectJsonConverter : JsonConverter<Dictionary<string, object>>
{
	public override Dictionary<string, object> 
		Read(ref Utf8JsonReader reader, 
			Type typeToConvert, JsonSerializerOptions options)
	{
		
		Utf8JsonReader readerClone = reader;
		
		if (readerClone.TokenType != JsonTokenType.StartObject)
		{
			throw new JsonException($"JsonTokenType was of type {readerClone.TokenType}, only objects are supported");
		}

		var dictionary = new Dictionary<string, object>();
		
		while (readerClone.Read())
		{
			if (readerClone.TokenType == JsonTokenType.EndObject)
			{
				reader = readerClone;
				return dictionary;
			}

			if (readerClone.TokenType != JsonTokenType.PropertyName)
			{
				throw new JsonException("JsonTokenType was not PropertyName");
			}

			var propertyName = readerClone.GetString();

			if (string.IsNullOrWhiteSpace(propertyName))
			{
				throw new JsonException("Failed to get property name");
			}

			readerClone.Read();
			dictionary.Add(propertyName, ExtractValue(ref readerClone, options));
		}
		
		reader = readerClone;
		return dictionary;
	}

	public override void Write(Utf8JsonWriter writer, Dictionary<string, object> value, JsonSerializerOptions options)
	{
    	writer.WriteStartObject();

		foreach (var key in value.Keys)
		{
			HandleValue(writer, key, value[key]);
		}

		writer.WriteEndObject();
	}

	private static void HandleValue(Utf8JsonWriter writer, string key, object objectValue)
	{
		if (key != null)
			writer.WritePropertyName(key);
	
		switch (objectValue)
		{
			case string stringValue:
				writer.WriteStringValue(stringValue);
				break;
			case DateTime dateTime:
				writer.WriteStringValue(dateTime);
				break;
			case long longValue:
				writer.WriteNumberValue(longValue);
				break;
			case int intValue:
				writer.WriteNumberValue(intValue);
				break;
			case float floatValue:
				writer.WriteNumberValue(floatValue);
				break;
			case double doubleValue:
				writer.WriteNumberValue(doubleValue);
				break;
			case decimal decimalValue:
				writer.WriteNumberValue(decimalValue);
				break;
			case bool boolValue:
				writer.WriteBooleanValue(boolValue);
				break;
			case Dictionary<string, object> dict:
				writer.WriteStartObject();
				foreach (var item in dict)
				{
					HandleValue(writer, item.Key, item.Value);
				}
				writer.WriteEndObject();
				break;
			case object[] array:
				writer.WriteStartArray();
				foreach (var item in array)
				{
					HandleValue(writer, item);
				}
				writer.WriteEndArray();
				break;
			default:
				writer.WriteNullValue();
				break;
		}
	}

	private static void HandleValue(Utf8JsonWriter writer, object value)
	{
		HandleValue(writer, null, value);
	}

	private object ExtractValue(ref Utf8JsonReader reader, JsonSerializerOptions options)
	{
		Utf8JsonReader readerClone = reader;
		
		switch (readerClone.TokenType)
		{
			case JsonTokenType.String:
			
				if (readerClone.TryGetDateTime(out var date))
					return date;
				
				return readerClone.GetString();
				
			case JsonTokenType.False:
				return false;
			case JsonTokenType.True:
				return true;
			case JsonTokenType.Null:
				return null;
			case JsonTokenType.Number:
			
				if (readerClone.TryGetInt64(out var result))
					return result;
				
				return readerClone.GetDecimal();
				
			case JsonTokenType.StartObject:
				return Read(ref readerClone, null, options);
			case JsonTokenType.StartArray:
			
				var list = new List<object>();
				while (readerClone.Read() && readerClone.TokenType != JsonTokenType.EndArray)
					list.Add(ExtractValue(ref readerClone, options));
				
				return list;
				
			default:
				throw new JsonException($"'{readerClone.TokenType}' is not supported");
		}
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
