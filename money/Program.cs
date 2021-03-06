using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Money
{
    public static class Program
    {
        public static void Main(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(wh => wh.UseStartup<Startup>())
                .Build()
                .Run();
    }
}
