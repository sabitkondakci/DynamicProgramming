// .Net6 Console Template
string jsonString =
@"
{
  ""Date"": ""2019-08-01T00:00:00"",
  ""Temperature"": 25,
  ""Summary"": ""Hot"",
  ""DatesAvailable"": [
    ""2019-08-01T00:00:00"",
    ""2019-08-02T00:00:00""
  ],
  ""TemperatureRanges"": {
      ""Cold"": {
          ""High"": 20,
          ""Low"": -10
      },
      ""Hot"": {
          ""High"": 60,
          ""Low"": 20
      }
  }
}
";

var options = new JsonSerializerOptions 
{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
};

JsonNode? jsonNode = 
    await Task.Run<JsonNode?>( () => JsonNode.Parse(jsonString));

// Console.WriteLine(jsonNode?.ToJsonString(options));

JsonNode? temperature = jsonNode?["TemperatureRanges"];
// Console.WriteLine(temperature?.ToJsonString());

JsonNode? datesAvalilable = jsonNode?["TemperatureRanges"];
JsonNode? firstElement =
    datesAvalilable!.GetType() == typeof(JsonArray) ? datesAvalilable?[1] : default(JsonNode) ;
// Console.WriteLine(firstElement?.GetValue<DateTime>());

var jsonNodeOptions = new JsonNodeOptions()
{
    PropertyNameCaseInsensitive = true
};

var jObject = new JsonObject(jsonNodeOptions)
{
    ["Date"] = new DateTime(2022, 1, 1),
    ["RoomTemperature"] = 21,
    ["Summary"] = "Moist",
    ["DatesAvailable"] =
        new JsonArray(new DateTime(2019, 5, 5), new DateTime(2020, 4, 4)),
    ["TemperatureRanges"] = new JsonObject
    {
        ["Cold"] = new JsonObject
        {
            ["High"] = 20,
            ["Low"] = -10
        },

        ["Warm"] = new JsonObject
        {
            ["High"] = 40,
            ["Low"] = 20
        }
    },

    ["Location"] = new JsonObject
    {
        ["HighAltitude"] = new JsonArray(435,22,11),
        ["LowAltitude"] = new JsonArray(322,32,11)
    },

    ["KeyWords"] = new JsonArray("Cool", "Windy", "Moist")
};

// add a new value to TemperatureRanges
jObject["TemperatureRanges"]!["HeatWave"] = new JsonObject
{
    ["High"] = 100,
    ["Low"] = 40
};

jObject.Remove("Date");
//Console.WriteLine(jObject.ToJsonString(options));


await using (var stream = new MemoryStream())
{
    await using (var jsonWriter = new Utf8JsonWriter(stream))
    {
        JsonObject? tempRangesObject = jObject["TemperatureRanges"]?.AsObject();

        // write a small portion of jObject to stream
        tempRangesObject?.WriteTo(jsonWriter);
        await jsonWriter.FlushAsync();

        stream.Seek(0, SeekOrigin.Begin);

        TemperatureRanges temperatureRanges =
            (await JsonSerializer.
                DeserializeAsync<TemperatureRanges>(stream))!;

  
        Console.WriteLine($"Hot.High = {temperatureRanges["Warm"].High}");

        JsonArray? jArray = jObject["DatesAvailable"]?.AsArray();
        Console.WriteLine($"First Date = {jArray?[0]?.GetValue<DateTime>()}");

    }
}



public class TemperatureRanges : Dictionary<string, HighLowTemps>
{
}

public class HighLowTemps
{
    public int High { get; set; }
    public int Low { get; set; }
}
