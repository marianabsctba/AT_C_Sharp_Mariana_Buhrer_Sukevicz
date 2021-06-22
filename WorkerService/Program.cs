using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Domain;
using Infrastructure.Data;
using Microsoft.Extensions.Configuration;

namespace WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var configRepository = hostContext.Configuration.GetValue<string>("AppSettings:configRepository");

                    services.AddSingleton<IDonationRepository>(provider => DefineRepositoryInstance(configRepository));
                    services.AddHostedService<Worker>();
                });

        private static IDonationRepository DefineRepositoryInstance(string configRepository)
        {
            if (configRepository == "DonationFileRepository")
                return new DonationFileRepository();
            else if (configRepository == "DonationListRepository")
                return new DonationRepositoryList();
            else
                throw new NotImplementedException("Não existe implementação de repositório para a configuração existente.");
        }
    }
}
