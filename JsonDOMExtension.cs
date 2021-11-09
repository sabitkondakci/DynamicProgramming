

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

//sampleJson.json content:
/* {
  "channel-version": "2.2",
  "latest-release": "2.2.8",
  "latest-release-date": "2019-11-19",
  "latest-runtime": "2.2.8",
  "latest-sdk": "2.2.207",
  "support-phase": "eol",
  "eol-date": "2019-12-23",
  "lifecycle-policy": "https://www.microsoft.com/net/support/policy",
  "releases": [
    {
      "release-date": "2019-11-19",
      "release-version": "2.2.8",
      "security": false,
      "release-notes": "https://github.com/dotnet/core/blob/master/release-notes/2.2/2.2.8/2.2.8.md",
      "runtime": {
        "version": "2.2.8",
        "version-display": "2.2.8",
        "vs-version": "",
        "files": [
          {
            "name": "1dotnet-runtime-linux-arm.tar.gz",
            "rid": "linux-arm",
            "url": "https://download.visualstudio.microsoft.com/download/pr/97595553-470b-45bc-842d-aff8da46d4c4/46ee25ac85e4844df0e7f0fb9229755c/dotnet-runtime-2.2.8-linux-arm.tar.gz",
            "hash": "a3fb720504821eca64ec507e4ae2e321b3119c90f7b14844db85026d386047e1cfdf6f24b07f5fae6f19af9ed7ccbe49e46a39ad16d0c3838d9e9589bf2d5ef9"
          },
          {
            "name": "1dotnet-runtime-linux-arm64.tar.gz",
            "rid": "linux-arm64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/8595cc08-1588-4e28-b765-1201b447c99b/342cf07ff5e3adb396d17da2de0d359b/dotnet-runtime-2.2.8-linux-arm64.tar.gz",
            "hash": "097e94db6a7cf2d78588825aea663ebbe6fdd51275bcb9cee4d6d00c8274532a3474f95d506267e38b1b45bf1fa3fa2d255ba532afffe9f5bce17c8092c24766"
          },
          {
            "name": "1dotnet-runtime-linux-musl-x64.tar.gz",
            "rid": "linux-musl-x64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/f5e25e07-9934-4323-9f8b-164e2a829063/d95bd8e5f1dd52168ebf4fb9594507b1/dotnet-runtime-2.2.8-linux-musl-x64.tar.gz",
            "hash": "d0f8e7ac385e7fcaca2a70b1081625be88289e06f031ce12955f0d6df0b6ff2f13e6d93287e30439bb19932b2a06a9d1162577579c9c85da435c4036c609659a"
          },
          {
            "name": "1dotnet-runtime-linux-x64.tar.gz",
            "rid": "linux-x64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/3fbca771-e7d3-45bf-8e77-cfc1c5c41810/e118d44f5a6df21714abd8316e2e042b/dotnet-runtime-2.2.8-linux-x64.tar.gz",
            "hash": "b818557b0090ec047be0fb2e5ffee212e23e8417e1b0164f455e3a880bf5b94967dc4c86d6ed82397af9acc1f7415674904f6225a1abff85d28d2a6d5de8073b"
          },
          {
            "name": "1dotnet-runtime-osx-x64.pkg",
            "rid": "osx-x64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/fcec560f-0ae9-4d60-8528-13a11150805a/97c10e386a0cb1a7c2312fcf7bf87823/dotnet-runtime-2.2.8-osx-x64.pkg",
            "hash": "ae5d06c54fb0126d87abfb30cc372f8b56f6ed2ddb3d0762fb05b39714c42535ea47298f3ed85f687dfea42d670acdb19bc3009dc79fab39d1a339d5708ca360"
          },
          {
            "name": "1dotnet-runtime-osx-x64.tar.gz",
            "rid": "osx-x64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/bbd4e493-6eed-45e8-90ed-7be0f1270c7a/2d19adb63887d3b02301361117bbe4f5/dotnet-runtime-2.2.8-osx-x64.tar.gz",
            "hash": "dcd38ac8c6093eacd6b649b6416dc6af0053003441d1182ccf0b9584e04805b82c51381139ee76de1788ffa3ff576e7310dd4bc24318412a0fe47a0982cfc0fb"
          },
          {
            "name": "1dotnet-runtime-rhel.6-x64.tar.gz",
            "rid": "rhel.6-x64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/fcefad8a-38da-4f06-8039-8b6053cd5d84/4548d460aac1744ad6ddd253bbb4422d/dotnet-runtime-2.2.8-rhel.6-x64.tar.gz",
            "hash": "5016f53948514bee22f4d7646ebc03784e86eb45d1d19bcec0f2ef2957f3f9374c512b9fff34686c597d4c0640cd208ed07df9298c0ff0f50a44a2c4b774beeb"
          },
          {
            "name": "1dotnet-runtime-win-arm.zip",
            "rid": "win-arm",
            "url": "https://download.visualstudio.microsoft.com/download/pr/584be079-dde5-465d-9f9b-04183458dd07/a666047a3ae292cb97d74e466320e600/dotnet-runtime-2.2.8-win-arm.zip",
            "hash": "21f34cafd5e3661017f1bc92f39d250f162f9d3832404125c73473250157d25cd74d7afe959749c4cc2fc418439d20f521626f4505e254f42ef857c5c10904c5"
          },
          {
            "name": "1dotnet-runtime-win-x64.exe",
            "rid": "win-x64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/4e14a32d-cf57-42ce-964f-fa40c7d11dde/95cf2d91312fc495bc25ad9137d42698/dotnet-runtime-2.2.8-win-x64.exe",
            "hash": "963742eb79d51807444d871cd57acc1c2b37a199eeecc97f3d47715fe73dcc7d2b7015bb2b7e6f7497726de67a44f936c1ebbc2c9ea728cf47e66aa4cbacc191"
          },
          {
            "name": "1dotnet-runtime-win-x64.zip",
            "rid": "win-x64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/79365951-b51b-487e-a03c-6ffeb3a5f3ad/ce9eb59ba8a76621d5e76614b0c9e97d/dotnet-runtime-2.2.8-win-x64.zip",
            "hash": "5864a40f662388761bc108510df9540b0b6672ae0f5a04cac71112ef0d1aa5781ffa3c856e919eed68eba6161f21a38c52f0e8850e8ecdf22609c42d2387d848"
          },
          {
            "name": "1dotnet-runtime-win-x86.exe",
            "rid": "win-x86",
            "url": "https://download.visualstudio.microsoft.com/download/pr/930685bc-ac92-4149-b4f0-b0b26d480418/c03bbed24f87e66281b5ff99ceecbb0b/dotnet-runtime-2.2.8-win-x86.exe",
            "hash": "75de2107f0798add2f67f773451a779b051bf6898936f8fbd82ce7347e7471bb5310831844e3868610bc1fb8cc6ef780c5b4fa7ec4419b81b574fb5085881de2"
          },
          {
            "name": "1dotnet-runtime-win-x86.zip",
            "rid": "win-x86",
            "url": "https://download.visualstudio.microsoft.com/download/pr/33751b42-f854-4d55-b2ff-3f0d09a88cf7/0c268c32f7730e90bd0a370be6699bf6/dotnet-runtime-2.2.8-win-x86.zip",
            "hash": "66fca0a5b9b801fea1972b87d99ecf9f9904edf948139df495e7696c6a02a417371c4632a9232c76e3832f6161a3dd172a0d799acf66db0dc43a7719776c90d7"
          }
        ]
      },
      "sdk": {
        "version": "2.2.207",
        "version-display": "2.2.207",
        "runtime-version": "2.2.8",
        "vs-version": "",
        "vs-support": "Visual Studio 2019 (v16.0)",
        "csharp-version": "7.3",
        "fsharp-version": "4.5",
        "files": [
          {
            "name": "dotnet-sdk-linux-arm.tar.gz",
            "rid": "linux-arm",
            "url": "https://download.visualstudio.microsoft.com/download/pr/fca1c415-b70c-4134-8844-ea947f410aad/901a86c12be90a67ec37cd0cc59d5070/dotnet-sdk-2.2.207-linux-arm.tar.gz",
            "hash": "a922b87fc1e433d489d6863fa3faca5a5eeb33f68104c5c4733ca8fbd187230715f6ce384ddbdaca501b1f42c87f590a9299b525be405214803bb1da3c4bbd1c"
          },
          {
            "name": "dotnet-sdk-linux-arm64.tar.gz",
            "rid": "linux-arm64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/18738093-b024-4353-96c2-4e1d2285a5e4/5e86ebbca79e71486aa2b18af0214ae9/dotnet-sdk-2.2.207-linux-arm64.tar.gz",
            "hash": "565fe5cbc2c388e54b3ee548d5b98e1fd85d920ceeeb5475a2bf2daa7f090fc925d8afef19b2b76973af439fbb749c6996711790287eafd588e4d916a016e84c"
          },
          {
            "name": "dotnet-sdk-linux-musl-x64.tar.gz",
            "rid": "linux-musl-x64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/c72122bd-38f5-4c98-b585-b8aaf57ecc6e/c89d7774a430e163d801753654f33972/dotnet-sdk-2.2.207-linux-musl-x64.tar.gz",
            "hash": "231118ba205b5d609263fc790851c65900aabf5830d492425849de89b7103f02012a302ce21960cb062426c5b8fd480e1316176a927bd287b08b7d19445f7224"
          },
          {
            "name": "dotnet-sdk-linux-x64.tar.gz",
            "rid": "linux-x64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/022d9abf-35f0-4fd5-8d1c-86056df76e89/477f1ebb70f314054129a9f51e9ec8ec/dotnet-sdk-2.2.207-linux-x64.tar.gz",
            "hash": "9d70b4a8a63b66da90544087199a0f681d135bf90d43ca53b12ea97cc600a768b0a3d2f824cfe27bd3228e058b060c63319cd86033be8b8d27925283f99de958"
          },
          {
            "name": "dotnet-sdk-osx-gs-x64.pkg",
            "rid": "osx-x64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/837aa87a-8160-4297-b6b7-eceb56b3ce48/74c42db19f2784ab172e27598eae7f4f/dotnet-sdk-2.2.207-osx-gs-x64.pkg",
            "hash": "3cf91804f2d0b7beb0830450f98cbd18125d1df72354b6a57668cca11a871a68d234f2d8a8a5fe86215b1f71584c22b9f75fc057365c55026da2979195894278"
          },
          {
            "name": "dotnet-sdk-osx-x64.pkg",
            "rid": "osx-x64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/cb2d65e1-ad90-4416-8e6a-3755f92ba39f/f498aca4950a038d6fc55cca75eca630/dotnet-sdk-2.2.207-osx-x64.pkg",
            "hash": "3cf91804f2d0b7beb0830450f98cbd18125d1df72354b6a57668cca11a871a68d234f2d8a8a5fe86215b1f71584c22b9f75fc057365c55026da2979195894278"
          },
          {
            "name": "dotnet-sdk-osx-x64.tar.gz",
            "rid": "osx-x64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/5b8d25c1-85e1-4b18-8d96-b14115586319/78ff638656c3a90324e810f8dd157422/dotnet-sdk-2.2.207-osx-x64.tar.gz",
            "hash": "d60d683851ba08a8f30acac8c635219223a6f11e1efe5ec7e64c4b1dca44f7e3d6122ecc0a4e97b8b57c2035e22be5e09f5f1642db6227bb8898654da057a7ae"
          },
          {
            "name": "dotnet-sdk-rhel.6-x64.tar.gz",
            "rid": "rhel.6-x64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/2b9ec838-2e6e-40cd-a57a-885e56904329/959135d11fd608afea316c01f73e9490/dotnet-sdk-2.2.207-rhel.6-x64.tar.gz",
            "hash": "64decac610d7fdda90e7a1236e155ddcc8bb35ee5fda8f1ebd7c97380eddff9638e08cf8d439bbc52bdedb223c70049441b448bda0b687b744b34b159630ef4b"
          },
          {
            "name": "dotnet-sdk-win-arm.zip",
            "rid": "win-arm",
            "url": "https://download.visualstudio.microsoft.com/download/pr/4ce7496a-fa96-4fbd-9259-f5ad6f9fbcd4/7a4176c05032d8b28cb3a7e830876c22/dotnet-sdk-2.2.207-win-arm.zip",
            "hash": "263aa3de231de97268d75dadee94031f26ee0c3ed0da18ee87c53eba42138cf1384ff0869caee13f8a57441c4c5d415d8abe388bb3dee3294f5af2a9e7620ecb"
          },
          {
            "name": "dotnet-sdk-win-gs-x64.exe",
            "rid": "",
            "url": "https://download.visualstudio.microsoft.com/download/pr/dfa5fe58-1542-4b4c-84bf-2aa44743c925/170740c73015a8c621bedab256fd8431/dotnet-sdk-2.2.207-win-gs-x64.exe",
            "hash": "721882a80632fb113dcd3b82a80f4be968a08b6f09a9c0513cef7464e5fae836b60b601e570289fc6a31d3765f6f66d81ec32d6e98e58098acb74d0a714eabb6"
          },
          {
            "name": "dotnet-sdk-win-gs-x86.exe",
            "rid": "",
            "url": "https://download.visualstudio.microsoft.com/download/pr/d23f0125-64e3-4132-97c0-5beb671228f9/e68a5a74a1dbf73059efe007ae56a456/dotnet-sdk-2.2.207-win-gs-x86.exe",
            "hash": "ce0a50585881d0345a232a3f40d99d4248c455157472525ade558bb93f222358ee79dde0786dcdf75b4923f55935d9d6aa8785c0129f44d713c8dee3f97c4195"
          },
          {
            "name": "dotnet-sdk-win-x64.exe",
            "rid": "win-x64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/279de74e-f7e3-426b-94d8-7f31d32a129c/e83e8c4c49bcb720def67a5c8fe0d8df/dotnet-sdk-2.2.207-win-x64.exe",
            "hash": "721882a80632fb113dcd3b82a80f4be968a08b6f09a9c0513cef7464e5fae836b60b601e570289fc6a31d3765f6f66d81ec32d6e98e58098acb74d0a714eabb6"
          },
          {
            "name": "dotnet-sdk-win-x64.zip",
            "rid": "win-x64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/e0d4bd70-9dd2-40a3-9e6e-64af9721f3e3/2324e93d2152efd009f242a1723685c3/dotnet-sdk-2.2.207-win-x64.zip",
            "hash": "726f60e2cf82b7fbea97066dda318d468774bcd830c7244aac16610f4a2bbd879cfb89a93dd7983a8b424babe8201d62845e2b904ed698455f1082655dd00286"
          },
          {
            "name": "dotnet-sdk-win-x86.exe",
            "rid": "win-x86",
            "url": "https://download.visualstudio.microsoft.com/download/pr/982e7a87-d652-4db0-b64b-cb14eaf17564/f920534ef0bfac0f0e2553b0428e45fd/dotnet-sdk-2.2.207-win-x86.exe",
            "hash": "ce0a50585881d0345a232a3f40d99d4248c455157472525ade558bb93f222358ee79dde0786dcdf75b4923f55935d9d6aa8785c0129f44d713c8dee3f97c4195"
          },
          {
            "name": "dotnet-sdk-win-x86.zip",
            "rid": "win-x86",
            "url": "https://download.visualstudio.microsoft.com/download/pr/1d1cc3a2-efb5-4810-8fcf-e6413945b4ad/e335d27e9ab47de259aa2f22db7a4e60/dotnet-sdk-2.2.207-win-x86.zip",
            "hash": "3f8d76e44a4f236d2c38b79620d4c4ab8e98f768774bf00ce3b3fad32762991f9c65bd16b5811218605b7f959a7fc7d492e17879370f4a58e0f7c15e0e603a56"
          }
        ]
      },
      "sdks": [
        {
          "version": "2.2.207",
          "version-display": "2.2.207",
          "runtime-version": "2.2.8",
          "vs-version": "",
          "vs-support": "Visual Studio 2019 (v16.0)",
          "csharp-version": "7.3",
          "fsharp-version": "4.5",
          "files": [
            {
              "name": "dotnet-sdk-linux-arm.tar.gz",
              "rid": "linux-arm",
              "url": "https://download.visualstudio.microsoft.com/download/pr/fca1c415-b70c-4134-8844-ea947f410aad/901a86c12be90a67ec37cd0cc59d5070/dotnet-sdk-2.2.207-linux-arm.tar.gz",
              "hash": "a922b87fc1e433d489d6863fa3faca5a5eeb33f68104c5c4733ca8fbd187230715f6ce384ddbdaca501b1f42c87f590a9299b525be405214803bb1da3c4bbd1c"
            },
            {
              "name": "dotnet-sdk-linux-arm64.tar.gz",
              "rid": "linux-arm64",
              "url": "https://download.visualstudio.microsoft.com/download/pr/18738093-b024-4353-96c2-4e1d2285a5e4/5e86ebbca79e71486aa2b18af0214ae9/dotnet-sdk-2.2.207-linux-arm64.tar.gz",
              "hash": "565fe5cbc2c388e54b3ee548d5b98e1fd85d920ceeeb5475a2bf2daa7f090fc925d8afef19b2b76973af439fbb749c6996711790287eafd588e4d916a016e84c"
            },
            {
              "name": "dotnet-sdk-linux-musl-x64.tar.gz",
              "rid": "linux-musl-x64",
              "url": "https://download.visualstudio.microsoft.com/download/pr/c72122bd-38f5-4c98-b585-b8aaf57ecc6e/c89d7774a430e163d801753654f33972/dotnet-sdk-2.2.207-linux-musl-x64.tar.gz",
              "hash": "231118ba205b5d609263fc790851c65900aabf5830d492425849de89b7103f02012a302ce21960cb062426c5b8fd480e1316176a927bd287b08b7d19445f7224"
            },
            {
              "name": "dotnet-sdk-linux-x64.tar.gz",
              "rid": "linux-x64",
              "url": "https://download.visualstudio.microsoft.com/download/pr/022d9abf-35f0-4fd5-8d1c-86056df76e89/477f1ebb70f314054129a9f51e9ec8ec/dotnet-sdk-2.2.207-linux-x64.tar.gz",
              "hash": "9d70b4a8a63b66da90544087199a0f681d135bf90d43ca53b12ea97cc600a768b0a3d2f824cfe27bd3228e058b060c63319cd86033be8b8d27925283f99de958"
            },
            {
              "name": "dotnet-sdk-osx-gs-x64.pkg",
              "rid": "osx-x64",
              "url": "https://download.visualstudio.microsoft.com/download/pr/837aa87a-8160-4297-b6b7-eceb56b3ce48/74c42db19f2784ab172e27598eae7f4f/dotnet-sdk-2.2.207-osx-gs-x64.pkg",
              "hash": "3cf91804f2d0b7beb0830450f98cbd18125d1df72354b6a57668cca11a871a68d234f2d8a8a5fe86215b1f71584c22b9f75fc057365c55026da2979195894278"
            },
            {
              "name": "dotnet-sdk-osx-x64.pkg",
              "rid": "osx-x64",
              "url": "https://download.visualstudio.microsoft.com/download/pr/cb2d65e1-ad90-4416-8e6a-3755f92ba39f/f498aca4950a038d6fc55cca75eca630/dotnet-sdk-2.2.207-osx-x64.pkg",
              "hash": "3cf91804f2d0b7beb0830450f98cbd18125d1df72354b6a57668cca11a871a68d234f2d8a8a5fe86215b1f71584c22b9f75fc057365c55026da2979195894278"
            },
            {
              "name": "dotnet-sdk-osx-x64.tar.gz",
              "rid": "osx-x64",
              "url": "https://download.visualstudio.microsoft.com/download/pr/5b8d25c1-85e1-4b18-8d96-b14115586319/78ff638656c3a90324e810f8dd157422/dotnet-sdk-2.2.207-osx-x64.tar.gz",
              "hash": "d60d683851ba08a8f30acac8c635219223a6f11e1efe5ec7e64c4b1dca44f7e3d6122ecc0a4e97b8b57c2035e22be5e09f5f1642db6227bb8898654da057a7ae"
            },
            {
              "name": "dotnet-sdk-rhel.6-x64.tar.gz",
              "rid": "rhel.6-x64",
              "url": "https://download.visualstudio.microsoft.com/download/pr/2b9ec838-2e6e-40cd-a57a-885e56904329/959135d11fd608afea316c01f73e9490/dotnet-sdk-2.2.207-rhel.6-x64.tar.gz",
              "hash": "64decac610d7fdda90e7a1236e155ddcc8bb35ee5fda8f1ebd7c97380eddff9638e08cf8d439bbc52bdedb223c70049441b448bda0b687b744b34b159630ef4b"
            },
            {
              "name": "dotnet-sdk-win-arm.zip",
              "rid": "win-arm",
              "url": "https://download.visualstudio.microsoft.com/download/pr/4ce7496a-fa96-4fbd-9259-f5ad6f9fbcd4/7a4176c05032d8b28cb3a7e830876c22/dotnet-sdk-2.2.207-win-arm.zip",
              "hash": "263aa3de231de97268d75dadee94031f26ee0c3ed0da18ee87c53eba42138cf1384ff0869caee13f8a57441c4c5d415d8abe388bb3dee3294f5af2a9e7620ecb"
            },
            {
              "name": "dotnet-sdk-win-gs-x64.exe",
              "rid": "",
              "url": "https://download.visualstudio.microsoft.com/download/pr/dfa5fe58-1542-4b4c-84bf-2aa44743c925/170740c73015a8c621bedab256fd8431/dotnet-sdk-2.2.207-win-gs-x64.exe",
              "hash": "721882a80632fb113dcd3b82a80f4be968a08b6f09a9c0513cef7464e5fae836b60b601e570289fc6a31d3765f6f66d81ec32d6e98e58098acb74d0a714eabb6"
            },
            {
              "name": "dotnet-sdk-win-gs-x86.exe",
              "rid": "",
              "url": "https://download.visualstudio.microsoft.com/download/pr/d23f0125-64e3-4132-97c0-5beb671228f9/e68a5a74a1dbf73059efe007ae56a456/dotnet-sdk-2.2.207-win-gs-x86.exe",
              "hash": "ce0a50585881d0345a232a3f40d99d4248c455157472525ade558bb93f222358ee79dde0786dcdf75b4923f55935d9d6aa8785c0129f44d713c8dee3f97c4195"
            },
            {
              "name": "dotnet-sdk-win-x64.exe",
              "rid": "win-x64",
              "url": "https://download.visualstudio.microsoft.com/download/pr/279de74e-f7e3-426b-94d8-7f31d32a129c/e83e8c4c49bcb720def67a5c8fe0d8df/dotnet-sdk-2.2.207-win-x64.exe",
              "hash": "721882a80632fb113dcd3b82a80f4be968a08b6f09a9c0513cef7464e5fae836b60b601e570289fc6a31d3765f6f66d81ec32d6e98e58098acb74d0a714eabb6"
            },
            {
              "name": "dotnet-sdk-win-x64.zip",
              "rid": "win-x64",
              "url": "https://download.visualstudio.microsoft.com/download/pr/e0d4bd70-9dd2-40a3-9e6e-64af9721f3e3/2324e93d2152efd009f242a1723685c3/dotnet-sdk-2.2.207-win-x64.zip",
              "hash": "726f60e2cf82b7fbea97066dda318d468774bcd830c7244aac16610f4a2bbd879cfb89a93dd7983a8b424babe8201d62845e2b904ed698455f1082655dd00286"
            },
            {
              "name": "dotnet-sdk-win-x86.exe",
              "rid": "win-x86",
              "url": "https://download.visualstudio.microsoft.com/download/pr/982e7a87-d652-4db0-b64b-cb14eaf17564/f920534ef0bfac0f0e2553b0428e45fd/dotnet-sdk-2.2.207-win-x86.exe",
              "hash": "ce0a50585881d0345a232a3f40d99d4248c455157472525ade558bb93f222358ee79dde0786dcdf75b4923f55935d9d6aa8785c0129f44d713c8dee3f97c4195"
            },
            {
              "name": "dotnet-sdk-win-x86.zip",
              "rid": "win-x86",
              "url": "https://download.visualstudio.microsoft.com/download/pr/1d1cc3a2-efb5-4810-8fcf-e6413945b4ad/e335d27e9ab47de259aa2f22db7a4e60/dotnet-sdk-2.2.207-win-x86.zip",
              "hash": "3f8d76e44a4f236d2c38b79620d4c4ab8e98f768774bf00ce3b3fad32762991f9c65bd16b5811218605b7f959a7fc7d492e17879370f4a58e0f7c15e0e603a56"
            }
          ]
        },
        {
          "version": "2.2.110",
          "version-display": "2.2.110",
          "runtime-version": "2.2.8",
          "vs-version": "",
          "vs-support": "Visual Studio 2017 (v15.9)",
          "csharp-version": "7.3",
          "fsharp-version": "4.5",
          "files": [
            {
              "name": "dotnet-sdk-linux-arm.tar.gz",
              "rid": "linux-arm",
              "url": "https://download.visualstudio.microsoft.com/download/pr/8cbe9c20-2e88-43dc-8d9a-27da95e5a1e7/d580d095fc8d236d7db15336668d9173/dotnet-sdk-2.2.110-linux-arm.tar.gz",
              "hash": "7a4c26448216d8e4e1433c4070972f5314fe69c8f7b8f66993b0a60465282fbd6b6a9cd8de9da251982f55f24a5853bd400c6cbf5e4ed40213b80b62e541d8c5"
            },
            {
              "name": "dotnet-sdk-linux-arm64.tar.gz",
              "rid": "linux-arm64",
              "url": "https://download.visualstudio.microsoft.com/download/pr/06413d6a-e12b-41fc-91cf-d88a6f97a5c1/5a32f67fe5ad0457309cf8e0fa52f2b8/dotnet-sdk-2.2.110-linux-arm64.tar.gz",
              "hash": "921ee8b9409a36ccc0d49fa90af68aa387bb0a7fbe7eea06c10b76cb2c53b81e08ce7767f4b18afdd4ce46194ca5e0de787b105a906f4da6c03dd5b284518063"
            },
            {
              "name": "dotnet-sdk-linux-musl-x64.tar.gz",
              "rid": "linux-musl-x64",
              "url": "https://download.visualstudio.microsoft.com/download/pr/17b53621-992d-4805-9feb-93cc34662c5f/c83ef9c56200b4d333b18c48f9054437/dotnet-sdk-2.2.110-linux-musl-x64.tar.gz",
              "hash": "1dd6bedfa2151bb518eaaba8621035ddae94fc69a1053c3247de3aab044252e2d0979984520bd11dee4922cd58a03f6ba99b652fb1602b5cff9a6d3d22034fa5"
            },
            {
              "name": "dotnet-sdk-linux-x64.tar.gz",
              "rid": "linux-x64",
              "url": "https://download.visualstudio.microsoft.com/download/pr/42f39f2f-3f24-4340-8c57-0a3133620c21/0a353696275b00cbddc9f60069867cfc/dotnet-sdk-2.2.110-linux-x64.tar.gz",
              "hash": "cd3bc601ccc45edf38cdcc254831b88539dd51f26bdafa2d74eebb09d20d19d745fe319a93c4290e3b74a7a5d8fe851773a748ef0f23f7997c76b26e74d0d94f"
            },
            {
              "name": "dotnet-sdk-osx-gs-x64.pkg",
              "rid": "osx-x64",
              "url": "https://download.visualstudio.microsoft.com/download/pr/701d2e3d-4e06-44e1-a364-88772d946ddb/fe84d8921516ac6c7c2f6b039611ac29/dotnet-sdk-2.2.110-osx-gs-x64.pkg",
              "hash": "4cd219e117c642955a553fd3c2bd78b6e1163d4bd773bfdc2bd2e8f1fc9a0b90b6243edb2d9c13298e18900394522284961d9e9204a4a9913c91bd15cf6de206"
            },
            {
              "name": "dotnet-sdk-osx-x64.pkg",
              "rid": "osx-x64",
              "url": "https://download.visualstudio.microsoft.com/download/pr/7db9e4c2-7118-4c13-8689-4193e4c91aed/a783f5cad3c017097bc123b478eee2a3/dotnet-sdk-2.2.110-osx-x64.pkg",
              "hash": "4cd219e117c642955a553fd3c2bd78b6e1163d4bd773bfdc2bd2e8f1fc9a0b90b6243edb2d9c13298e18900394522284961d9e9204a4a9913c91bd15cf6de206"
            },
            {
              "name": "dotnet-sdk-osx-x64.tar.gz",
              "rid": "osx-x64",
              "url": "https://download.visualstudio.microsoft.com/download/pr/f2d70b94-7b76-49c7-917f-758e71135305/24dc05dad28e067500762516d4a8d514/dotnet-sdk-2.2.110-osx-x64.tar.gz",
              "hash": "866512de8a387d66b9518620ca1449bef61fcd8ca4978f2286d3d44de09670ba418bdb9d0a6d821f61e3f753996db66841e8ddaf53e5859ed0b767b6451534d6"
            },
            {
              "name": "dotnet-sdk-rhel.6-x64.tar.gz",
              "rid": "rhel.6-x64",
              "url": "https://download.visualstudio.microsoft.com/download/pr/c7a67baf-9c6b-4fdf-8b58-c3e554c6802f/87fbc4a569b2c5ddca8c4933346ff56d/dotnet-sdk-2.2.110-rhel.6-x64.tar.gz",
              "hash": "25d68910b75aec0ba8fd038264aa641a8b8b89c1c6cb8871a69081d40ebfe9f79473e3d3efe64c75cdab7b50a4518da131a04e685c1c91b95f1c7ceac48216dd"
            },
            {
              "name": "dotnet-sdk-win-arm.zip",
              "rid": "win-arm",
              "url": "https://download.visualstudio.microsoft.com/download/pr/c466f96a-d612-4f1c-9b4f-5bb3f658d5a7/38f300421101aa06bb58de9f8651de7e/dotnet-sdk-2.2.110-win-arm.zip",
              "hash": "3b938d4b46807fd84e62f1d8b20ecd3e89280e2f7867a4a510ef298b9eb29cfd524f332525ccd442a9d40c9bac438291e2601b305ac23f9f8fdcc2a023652009"
            },
            {
              "name": "dotnet-sdk-win-gs-x64.exe",
              "rid": "",
              "url": "https://download.visualstudio.microsoft.com/download/pr/3e380085-9346-4710-9079-a7292981b1fd/3aeba734cf4d9d44e0fd8eea4af0673f/dotnet-sdk-2.2.110-win-gs-x64.exe",
              "hash": "d36edc2cc36e3f1a673cfddc4c5ccfd70806f56604995015678e17ab3ece7cc5a530b4f1dbb9e03f916c5cd0eabd13005219c25259d29528cb4efc3a03425623"
            },
            {
              "name": "dotnet-sdk-win-gs-x86.exe",
              "rid": "",
              "url": "https://download.visualstudio.microsoft.com/download/pr/005b25fa-1626-4ea0-9605-c72d0a1786c7/6735c99bc12178f2db721b883582986c/dotnet-sdk-2.2.110-win-gs-x86.exe",
              "hash": "365de8c85f22977d3fda98fe02d15fc3c847b43ce1b447fb9028c062a86c541fd668a48d50633b1ce8e3469b7d219ac68cff33d8b3b064325c66d021d30f4b3e"
            },
            {
              "name": "dotnet-sdk-win-x64.exe",
              "rid": "win-x64",
              "url": "https://download.visualstudio.microsoft.com/download/pr/78969d24-673f-4515-9544-1dd5bcda5411/beda84891a9a085cecd9bff855fdd082/dotnet-sdk-2.2.110-win-x64.exe",
              "hash": "d36edc2cc36e3f1a673cfddc4c5ccfd70806f56604995015678e17ab3ece7cc5a530b4f1dbb9e03f916c5cd0eabd13005219c25259d29528cb4efc3a03425623"
            },
            {
              "name": "dotnet-sdk-win-x64.zip",
              "rid": "win-x64",
              "url": "https://download.visualstudio.microsoft.com/download/pr/246c4b65-5a51-4294-8ce3-181aefd60e94/5169a50a00d4c56abe20ef1c1325ceff/dotnet-sdk-2.2.110-win-x64.zip",
              "hash": "4b702194aa19a3e9659733cae3e32ae1db595924ddeb4d7fb81736242b30c91ed2444b7bc2b588ff4e6a79ec184fd476e0c9a49b37b09bc93085a5b5dcdadeef"
            },
            {
              "name": "dotnet-sdk-win-x86.exe",
              "rid": "win-x86",
              "url": "https://download.visualstudio.microsoft.com/download/pr/1af1c7ed-74dc-4772-8e8c-146e54a47b2f/162c9a7e45ea5080a3f4085d8684b7b9/dotnet-sdk-2.2.110-win-x86.exe",
              "hash": "365de8c85f22977d3fda98fe02d15fc3c847b43ce1b447fb9028c062a86c541fd668a48d50633b1ce8e3469b7d219ac68cff33d8b3b064325c66d021d30f4b3e"
            },
            {
              "name": "dotnet-sdk-win-x86.zip",
              "rid": "win-x86",
              "url": "https://download.visualstudio.microsoft.com/download/pr/513e3f1e-2ff8-48d9-bc2a-0e60b19eca72/4a780ab4d4fe4ce5e7777d25d973e1b7/dotnet-sdk-2.2.110-win-x86.zip",
              "hash": "77072d6eabe0181e7f1ad2dbf95b4d37ffdb8189049785df24d2842e28ce950c9ab52f08b45aa0443c553cb67aa0ef1ed139ba0c4a8364671574a8166d6af482"
            }
          ]
        }
      ],
      "aspnetcore-runtime": {
        "version": "2.2.8",
        "version-display": "2.2.8",
        "version-aspnetcoremodule": [
          "12.2.19109.5"
        ],
        "vs-version": "",
        "files": [
          {
            "name": "aspnetcore-runtime-linux-arm.tar.gz",
            "rid": "linux-arm",
            "url": "https://download.visualstudio.microsoft.com/download/pr/9fcb0171-11d7-40e6-a2e8-2357813bf6bd/becdd52523d5a6782ded8febd2c487a0/aspnetcore-runtime-2.2.8-linux-arm.tar.gz",
            "hash": "fab9a1d9d101716337bb153af2ac36429fc387230c0c0bf2d639b31fb7f787bc8dbaaa31f28f9cbe69f117ffc78d8ddb5a5968da0e77785d3c12c6814ef50f7b"
          },
          {
            "name": "aspnetcore-runtime-linux-musl-x64.tar.gz",
            "rid": "linux-musl-x64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/981063ac-98de-4622-9da7-c9df5a2547b5/ebc5edcac0759ad87f478c92f36a9a0c/aspnetcore-runtime-2.2.8-linux-musl-x64.tar.gz",
            "hash": "139d13a067d91b13f90f488cbb36517a0c629e803e15edbb4fb85443641184c4efd8c83110e32c1a1cc578b95f25e38056e680830288665491b568ea3944db3f"
          },
          {
            "name": "aspnetcore-runtime-linux-x64.tar.gz",
            "rid": "linux-x64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/e716faa4-345c-45a7-bd1f-860cdf422b75/fa8e57167f3bd4bf20b8b60992cf184f/aspnetcore-runtime-2.2.8-linux-x64.tar.gz",
            "hash": "954072376698be69acb7e277df2c243f931e10529def21dcbf9ce277609b30d462126bf8b8b3cab36476bec3d63a927b8e44e59e4d4cade23eef45956fba1ffd"
          },
          {
            "name": "aspnetcore-runtime-osx-x64.tar.gz",
            "rid": "osx-x64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/e73aa371-90fd-488c-805a-649a324ea853/611a4a5bd4da4a950387eea27e0b588a/aspnetcore-runtime-2.2.8-osx-x64.tar.gz",
            "hash": "1c969c6cbad8276ae19a64448105d627a43b97f26d4bc65165afecbea684f9f370969be00070fda065e0cd88842f4280e54b181bb32c608f307e68507fd4607c"
          },
          {
            "name": "aspnetcore-runtime-win-arm.zip",
            "rid": "win-arm",
            "url": "https://download.visualstudio.microsoft.com/download/pr/344af0cd-5fd8-427b-a438-b94d1973fdcc/54291ccaa6049a63a811bb52d0eb94e6/aspnetcore-runtime-2.2.8-win-arm.zip",
            "hash": "ed0152644d9270010c0470c32e5774c8f542f70bdf09f66665c4c1640c379b3cc4ba38d33ef170e16f606257faa5b696562e3575eb6f372865780b851b39e59f"
          },
          {
            "name": "aspnetcore-runtime-win-x64.exe",
            "rid": "win-x64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/068d05e8-a0cf-4584-9422-b77f34f1e98e/de70e92721a05c6148619993cbf1376b/aspnetcore-runtime-2.2.8-win-x64.exe",
            "hash": "18e3b7fcb645aa5d476d9b06491013b533ba1653015d8dbf90001c917ce48a8a6e93b3d5cea25e38965f5a024f836ef8b99e04892b043b4da850316111d60514"
          },
          {
            "name": "aspnetcore-runtime-win-x64.zip",
            "rid": "win-x64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/acf18dce-9e6a-4a39-a1c7-e503c09e4086/f2c6e01ef9bb44c4beb905d82bb7ebac/aspnetcore-runtime-2.2.8-win-x64.zip",
            "hash": "990c1c0e1db6f59b213f4d94c81c212dc39cbbe6a9ae2a0183c1f7947b447e614ac266d78785d0f0fc0451d32e3d3f0b3e5f7415a52d5c7f2e58db38aedda1d0"
          },
          {
            "name": "aspnetcore-runtime-win-x86.exe",
            "rid": "win-x86",
            "url": "https://download.visualstudio.microsoft.com/download/pr/53eefcbe-83a9-42ce-b529-9ef7672c5508/b3e9c4afc183b447044703dbc8edf71d/aspnetcore-runtime-2.2.8-win-x86.exe",
            "hash": "34c648e841ec8a016990d9dab30eac3bf26b0bca1ba2f16b807cb15abd028d951db61be8b1a5f1278ad6a63469908aa7e37dc75717556b660e2fe07c5d4d6cc7"
          },
          {
            "name": "aspnetcore-runtime-win-x86.zip",
            "rid": "win-x86",
            "url": "https://download.visualstudio.microsoft.com/download/pr/295249c5-35e1-4688-a9f4-9096989d70c1/c6cd5d342e754d2cff6f61645c4e84ae/aspnetcore-runtime-2.2.8-win-x86.zip",
            "hash": "a82014bc9ec924668115351d96c1c64754ffdd31ae3bc080ab7b18fd072dc4c127256cb2442b7977cc13014208793bcb2340e575d4af5ac9d12f0c12fe275892"
          },
          {
            "name": "dotnet-hosting-win.exe",
            "rid": "win-x86_x64",
            "url": "https://download.visualstudio.microsoft.com/download/pr/ba001109-03c6-45ef-832c-c4dbfdb36e00/e3413f9e47e13f1e4b1b9cf2998bc613/dotnet-hosting-2.2.8-win.exe",
            "hash": "1b3177fc65ec343f641b8ffdc2a0e925e322e90ed44dcb5c6d3982a370dd7b56f7fcc362dad3a4b7e2db4f0fe6878b7e7448fc7f41dfe01302c7484434691f6b"
          }
        ]
      },
      "windowsdesktop": {
        "version": "2.2.8",
        "version-display": "2.2.8",
        "files": []
      },
      "symbols": {
        "version": "2.2.8",
        "files": []
      }
    }
  }
} */
