

async Task Main()
{
	await using var sampleFileOpen = 
		File.OpenRead(@"C:\Users\fenko\Desktop\sampleJson.json");
		
	sampleFileOpen.Seek(0,SeekOrigin.Begin);
	
	using (JsonDocument jsonDocument = await JsonDocument.ParseAsync(sampleFileOpen))
	{
		var root = jsonDocument.RootElement;
		var releaseList = root.Pull("releases");

		var enumeratedList = releaseList?.EnumerateArray().Where(jsonElement =>
		{
			if (jsonElement.TryGetProperty("release-date", out JsonElement date))
			{
				var dateTime = date.TryGetDateTime(out DateTime outputDateTime) ?
					outputDateTime : new DateTime(0, 0, 0);
				
				// get JsonElements whose release-date year >= 2019;
				if (dateTime.Year >= 2019)
					return true;
			}
			
			return false;
		});

		foreach (var jsonElement in enumeratedList)
		{				
			var runtimeFiles = jsonElement.
				Pull("runtime:files")?.
					EnumerateArray() ?? default(JsonElement.ArrayEnumerator);
			
			foreach (JsonElement file in runtimeFiles)
			{
				file.Pull("name")?.GetString().Dump();
			}
		}
		
	}
}


public static class JsonDOMExtensions
{
	public static JsonElement? Pull(this JsonElement jsonElement, string jsonPath)
	{
		if (jsonPath is null || 
				jsonElement.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
			return default(JsonElement?);

		var filter = new char[] { '.', ':' };
		
		string[] slicedList =
			jsonPath.Split(filter, StringSplitOptions.RemoveEmptyEntries);

		for (int i = 0; i < slicedList.Length; i++)
		{
			if (jsonElement.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
				return default(JsonElement?);
			
			jsonElement = 
				jsonElement.TryGetProperty(slicedList[i], out JsonElement output) ?
					output : default(JsonElement);
		}

		return jsonElement;
	}
}
