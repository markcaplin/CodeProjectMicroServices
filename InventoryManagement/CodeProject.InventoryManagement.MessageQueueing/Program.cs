using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using CodeProject.InventoryManagement.Data.EntityFramework;
using CodeProject.Shared.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using CodeProject.InventoryManagement.Interfaces;
using CodeProject.Shared.Common.Interfaces;
using CodeProject.InventoryManagement.Business.MessageService;
using CodeProject.MessageQueueing;

namespace CodeProject.InventoryManagement.MessageQueueing
{
	class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = new HostBuilder().ConfigureAppConfiguration((hostingContext, config) =>
				{
					string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
					string jsonFile = $"appsettings.{environment}.json";
					config.AddJsonFile(jsonFile, optional: true);
					config.AddEnvironmentVariables();

					if (args != null)
					{
						config.AddCommandLine(args);
					}

				})
				.ConfigureServices((hostContext, services) =>
				{
					services.AddDbContext<InventoryManagementDatabase>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("PrimaryDatabaseConnectionString")));

					services.AddTransient<IInventoryManagementDataService, InventoryManagementDataService>();
					services.AddTransient<IMessageQueueing, CodeProject.MessageQueueing.MessageQueueing>();

					services.AddTransient<IMessageQueueProcessing>(provider =>
					new MessageProcessing(provider.GetRequiredService<IInventoryManagementDataService>()));

					services.AddOptions();
					services.Configure<MessageQueueAppConfig>(hostContext.Configuration.GetSection("MessageQueueAppConfig"));

					services.AddSingleton<IHostedService, SendMessages>();


				})
				.ConfigureServices((hostContext, services) =>
				{
					services.AddDbContext<InventoryManagementDatabase>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("PrimaryDatabaseConnectionString")));

					services.AddTransient<IInventoryManagementDataService, InventoryManagementDataService>();
					services.AddTransient<IMessageQueueing, CodeProject.MessageQueueing.MessageQueueing>();

					services.AddTransient<IMessageQueueProcessing>(provider =>
					new MessageProcessing(provider.GetRequiredService<IInventoryManagementDataService>()));

					services.AddOptions();
					services.Configure<MessageQueueAppConfig>(hostContext.Configuration.GetSection("MessageQueueAppConfig"));

					services.AddSingleton<IHostedService, ReceiveMessages>();

				})
				.ConfigureServices((hostContext, services) =>
				{
					services.AddDbContext<InventoryManagementDatabase>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("PrimaryDatabaseConnectionString")));

					services.AddTransient<IInventoryManagementDataService, InventoryManagementDataService>();
					services.AddTransient<IMessageQueueing, CodeProject.MessageQueueing.MessageQueueing>();

					services.AddTransient<IMessageQueueProcessing>(provider =>
					new MessageProcessing(provider.GetRequiredService<IInventoryManagementDataService>()));

					services.AddOptions();
					services.Configure<MessageQueueAppConfig>(hostContext.Configuration.GetSection("MessageQueueAppConfig"));

					services.AddSingleton<IHostedService, ProcessMessages>();

				})
				.ConfigureLogging((hostingContext, logging) =>
				{
					logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
					logging.AddConsole();
				});

			/*.ConfigureServices((hostContext, services) =>
			{
				services.AddOptions();
				services.Configure<AppConfig>(hostContext.Configuration.GetSection("AppConfig"));

				services.AddSingleton<IHostedService, PrintTest2>();
			})
			.ConfigureLogging((hostingContext, logging) => {
				logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
				logging.AddConsole();
			});*/

			await builder.RunConsoleAsync();
		}
	}
}
