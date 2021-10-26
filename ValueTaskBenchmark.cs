// LINQPad6 .Net5
void Main()
{
	BenchmarkRunner.Run<VTaskBench>();
}

[MemoryDiagnoser]
public class VTaskBench
{
	Dictionary<string,string> _cache;
	string[] names;
	Random random;
	
	[GlobalSetup]
	public void Setup()
	{
		random = new();
		_cache = new();
		names = new string[]
		{
			"sabitkondakci","serhatcan","jimmy","mojombo","defunkt"
			,"pjhyett","wycats","ezmobius","ivey"
		};
	}
	
	public async Task<string> GetUserInfoTaskAsync(string username)
	{
		string userInfo = _cache.ContainsKey(username)
		? _cache[username] : string.Empty;
		
		if (userInfo != string.Empty)
			return userInfo;

		string url = "https://api.github.com/users/" + username + "/repos";
		using (var client = new HttpClient())
		{
			client.DefaultRequestHeaders.Accept.Clear();

			client.DefaultRequestHeaders.Accept
			.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));

			client.DefaultRequestHeaders.Add("User-Agent",
			".NET Foundation Repository Reporter");

			var freshUserInfo = await client.GetStringAsync(url);
			_cache.TryAdd(username, freshUserInfo);

			return freshUserInfo;
		}
	}

	public async ValueTask<string> GetUserInfoValueTaskAsync(string username)
	{
		string userInfo = _cache.ContainsKey(username)
		? _cache[username] : string.Empty;

		if (userInfo != string.Empty)
			return userInfo;

		string url = "https://api.github.com/users/" + username + "/repos";
		using (var client = new HttpClient())
		{
			client.DefaultRequestHeaders.Accept.Clear();

			client.DefaultRequestHeaders.Accept
			.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));

			client.DefaultRequestHeaders.Add("User-Agent",
			".NET Foundation Repository Reporter");

			var freshUserInfo = await client.GetStringAsync(url);
			_cache.TryAdd(username, freshUserInfo);

			return freshUserInfo;
		}
	}

	[Benchmark]
	public async Task TaskTest()
	{
		for (int i = 0; i < 1_000_000; i++)
		{
			string randomName = names[random.Next(0,9)];
			string info = await GetUserInfoTaskAsync(randomName);
		}
	}

	[Benchmark]
	public async Task ValueTaskTest()
	{
		
		for (int i = 0; i < 1_000_000; i++)
		{
			string randomName = names[random.Next(0, 9)];
			string info = await GetUserInfoValueTaskAsync(randomName);
		}
	}
}
