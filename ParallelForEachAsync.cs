using System.Runtime.CompilerServices;

async IAsyncEnumerable<string> GetInfoAsync(string[] users,
    [EnumeratorCancellation]CancellationToken cancellationToken = default)
{
    if (users is not null)
    {
        for (int i = 0; i < users.Length; i++)
        {
            var userHandler = users[i];
            if (!cancellationToken.IsCancellationRequested)
                yield return await Task.FromResult(userHandler);
            else
            { 
              // throw new TaskCancelledException()
              await Task.FromCanceled(cancellationToken);
            }
        }
    }
}

using HttpClient httpClient = new()
{
    BaseAddress = new Uri("https://api.github.com"),
};

httpClient.DefaultRequestHeaders.
    UserAgent.Add(new ProductInfoHeaderValue("Trying_Net6", "GitAPI"));

// GitHub provides OAuth token for 30 days,
// which is capable of recieving and sending 5000 respose/request per minute
httpClient.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("token",
       "Setttings > Developer Settings > Personal Access Tokens ");


ParallelOptions prlOptions = new()
{
    MaxDegreeOfParallelism = 5
};

using var cancTokenSource = new CancellationTokenSource(5000);

try
{
    await RunTasksParallelAsync(cancTokenSource.Token);
}
catch (TaskCanceledException)
{
    Console.WriteLine("Request has been cancelled!");
}

async Task RunTasksParallelAsync(CancellationToken cancellationToken = default)
{
    for (int i = 2000; i < 2025; i++)
    {
        string uri = $"https://api.github.com/users?since={i}";
        var all_users =
            await httpClient.GetStringAsync(uri,cancellationToken);


        var filter_users = all_users.Split(",");
        
        // creation of "users/{username}" string[] list;
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

        var userAsync = GetInfoAsync(username_list, cancellationToken);

        await Parallel.
            ForEachAsync(userAsync, prlOptions, async (uri, token) =>
            {

                var user =
                     await httpClient.GetFromJsonAsync<GithubAccount>(uri, token);

                Console.WriteLine(
                $"Name: {user?.Name}\n" +
                $"Bio: {user?.Bio}\n" +
                $"Twitter-UserName: {user?.Twitter_UserName}\n" +
                $"Location: {user?.Location}\n" +
                $"Blog: {user?.Blog}\n" +
                $"Company: {user?.Company}\n");
            });
    }
}

public record GithubAccount(string Name,
        string Bio, string Twitter_UserName,
            string Location, string Blog,
                string Company, string Email);
