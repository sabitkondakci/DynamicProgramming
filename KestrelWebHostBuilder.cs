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

       }).Build().Run();
    }
}
