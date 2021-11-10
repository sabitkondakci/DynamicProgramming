async Task Main()
{
	
	var encodeSettings = new TextEncoderSettings();
	encodeSettings.AllowCharacters('\u0436', '\u0430');
	encodeSettings.AllowRange(UnicodeRanges.BasicLatin);
	
	var options = new JsonWriterOptions
	{
		Indented = true,
		Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin,UnicodeRanges.Arabic)
			
		// An alternative is to specify individual characters that
		// you want to allow through without being escaped.
		// The following example serializes only the first two characters of жарко:
		//Encoder = JavaScriptEncoder.Create(encodeSettings)
	};
	
	// Provides methods to transform UTF-8
	// or UTF-16 encoded text into a form that is suitable for JSON.
	var jsonEncodedText = JsonEncodedText.
		Encode("sampleArray",JavaScriptEncoder.UnsafeRelaxedJsonEscaping);
		
	
	await using (var stream = new MemoryStream())
	{
		await using (var writer = new Utf8JsonWriter(stream, options))
		{
			writer.WriteStartObject();
			writer.WriteString("start-date", DateTimeOffset.UtcNow);
			writer.WriteNumber("array-length", 2);
			writer.WriteBoolean("is-still-valid",true);
			
			writer.WriteCommentValue("sample arrays are great!");
			writer.WriteStartArray(jsonEncodedText.EncodedUtf8Bytes);
				writer.WriteStartObject();
				writer.WriteString("name", "Ahmet");
				writer.WriteNumber("age", 14);
				writer.WriteEndObject();

				writer.WriteStartObject();
				writer.WriteString("name", "Elif");
				writer.WriteNumber("age", 11);
				writer.WriteEndObject();
				
				writer.WriteCommentValue("Array Values");
				// boolean in a json array
				writer.WriteBooleanValue(false); 
				// null in a json array
				writer.WriteNullValue();
				// string value in a json array
				writer.WriteStringValue("test string value"); 
				// number value in a json array
				writer.WriteNumberValue(999); 
				
			writer.WriteEndArray();
			
			writer.WriteNull("nullProperty");
			
			writer.WriteEndObject();
			await writer.FlushAsync();
		}
		
		// store it to a file
		using var fileStream = File.Create(@"C:\Users\fenko\Desktop\sample.json");
		await fileStream.WriteAsync(stream.ToArray());
	}
}

// OUTPUT:

//{
//  "start-date": "2021-11-10T12:38:21.2030153+00:00",
//  "array-length": 2,
//  "is-still-valid": true
//  /*sample arrays are great!*/,
//  "sampleArray": [
//    {
//      "name": "Ahmet",
//      "age": 14
//    },
//    {
//	"name": "Elif",
//      "age": 11
//
//	}
//    /*Array Values*/,
//    false,
//    null,
//    "test string value",
//    999
//  ],
//  "nullProperty": null
//}



