public class Program
{
    public static void Main(string[] args)
    {
        new WebHostBuilder()
       .UseContentRoot(Directory.GetCurrentDirectory())
       .UseKestrel()
       .UseIISIntegration()
       .UseStartup<Startup>()
       .ConfigureKestrel((context, serverOptions) =>
       {
           // setup for HTTP/2
           serverOptions.ConfigureHttpsDefaults(conAdapter => conAdapter.SslProtocols = System.Security.Authentication.SslProtocols.Tls12);
           
            /*
           serverOptions.Limits.MaxConcurrentConnections = 100;
           serverOptions.Limits.MaxConcurrentUpgradedConnections = 100;
           serverOptions.Limits.MaxRequestBodySize = 10 * 1024;
           serverOptions.Limits.MinRequestBodyDataRate =
               new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
           serverOptions.Limits.MinResponseDataRate =
               new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
           serverOptions.Listen(IPAddress.Loopback, 5000);
           serverOptions.Listen(IPAddress.Loopback, 5001, listenOptions =>
           {
               listenOptions.UseHttps("testCert.pfx", "testPassword");
           });
           serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
           serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
           */
           
           // visit the link for more detailed information:
           // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel?view=aspnetcore-2.2#listenoptionsprotocols

       }).Build().Run();
    }
}
