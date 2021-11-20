using System.Runtime.CompilerServices;

async IAsyncEnumerable<string> GetInfoAsync(string[] users,
    [EnumeratorCancellation]CancellationToken cancellationToken = default)
{
    if (users is not null)
    {
        for (int i = 0; i < users.Length; i++)
        {
            var userHandler = users[i];
            if(!cancellationToken.IsCancellationRequested)
                yield return await Task.FromResult(userHandler);
        }
    }
}

using HttpClient httpClient = new()
{
    BaseAddress = new Uri("https://api.github.com"),
};

httpClient.DefaultRequestHeaders.
    UserAgent.Add(new ProductInfoHeaderValue("Trying_Net6", "GitAPI"));

httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("token",
        "Settings > DeveloperSettings > Personal Access Tokens"); // GitHub OAuth token for testing environment, providing 5000 requests per minute


ParallelOptions prlOptions = new()
{
    MaxDegreeOfParallelism = 5
};

for (int i = 2000; i < 2025; i++)
{
    string uri = $"https://api.github.com/users?since={i}";
    var all_users = await httpClient.GetStringAsync(uri);


    var filter_users = all_users.Split(",");
    
    // creation of "users/{username}" list
    var username_list =
        filter_users.Where(x =>
        {
            int index = x.IndexOf("login");

            if (index != -1)
                return x.Substring(index, 5) == "login";
            else
                return false;
        }).
        Select(l => "users/" + l.Substring(l.IndexOf(":") + 1).
        Replace("\"", String.Empty)).ToArray();

    var userAsync = GetInfoAsync(username_list);

    await Parallel.ForEachAsync(userAsync, prlOptions, async (uri, token) =>
   {
        var user =await httpClient.GetFromJsonAsync<GithubAccount>(uri, token);
       
        Console.WriteLine(
            $"Name: {user?.Name}\n" +
            $"Bio: {user?.Bio}\n" +
            $"Twitter-UserName: {user?.Twitter_UserName}\n" +
            $"Location: {user?.Location}\n" +
            $"Blog: {user?.Blog}\n" +
            $"Company: {user?.Company}\n");
    });
}

public record GithubAccount(string Name,
        string Bio, string Twitter_UserName,
            string Location, string Blog,
                string Company, string Email);
