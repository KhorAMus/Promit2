using System;
using System.Linq;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ConsoleUI
{
    class Program
    {
        //public static void Main(string[] args)
        //{
        //    CreateHostBuilder(args).Build().Run();
        //}

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureAppConfiguration(builder =>
        //        {
        //            builder.UseStartup<Startup>();
        //        });

        static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();


            var services = host.Services;

            var context = services.GetRequiredService<WordsContext>();

            if (!args.Any())
            {
                var input = Console.ReadLine();
            }
            else if (args[0] == "создание словаря")
            {
                var filePath = args[1];
                Console.WriteLine("Dictionary creation should perform here");
            }
            else if (args[0] == "обновление словаря")
            {
                var filePath = args[1];
                Console.WriteLine("Dictionary updating should perform here");
            }
            else if (args[0] == "очистить словарь")
            {
                var filePath = args[1];
                Console.WriteLine("Dictionary deleting should perform here");
            }


        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostBuilderContext, services) => services.AddDbContext<WordsContext>(option =>
                {
                    var connString = hostBuilderContext.Configuration.GetConnectionString("DefaultConnection");
                    var section1 = hostBuilderContext.Configuration.GetSection("ConnectionStrings")["DefaultConnection"];
                    option.UseSqlServer(connString);
                }));
        }
    }
}
