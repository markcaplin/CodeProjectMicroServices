using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using CodeProject.LoggingManagement.Data.EntityFramework;
using CodeProject.Shared.Common.Models;
using Microsoft.EntityFrameworkCore;
using CodeProject.LoggingManagement.Interfaces;
using CodeProject.Shared.Common.Interfaces;
using CodeProject.LoggingManagement.Business.MessageService;
using CodeProject.MessageQueueing;

namespace CodeProject.LoggingManagement.MessageQueueing
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
					services.AddDbContext<LoggingManagementDatabase>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("PrimaryDatabaseConnectionString")));

					services.AddTransient<ILoggingManagementDataService, LoggingManagementDataService>();
					services.AddTransient<IMessageQueueing, CodeProject.MessageQueueing.MessageQueueing>();


					services.AddTransient<IMessageQueueProcessing>(provider =>
					new MessageProcessing(provider.GetRequiredService<ILoggingManagementDataService>()));

					services.AddOptions();
					services.Configure<MessageQueueAppConfig>(hostContext.Configuration.GetSection("MessageQueueAppConfig"));
					services.Configure<ConnectionStrings>(hostContext.Configuration.GetSection("ConnectionStrings"));

					services.AddSingleton<IHostedService, SendMessages>();


				})
				.ConfigureServices((hostContext, services) =>
				{
					services.AddDbContext<LoggingManagementDatabase>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("PrimaryDatabaseConnectionString")));

					services.AddTransient<ILoggingManagementDataService, LoggingManagementDataService>();
					services.AddTransient<IMessageQueueing, CodeProject.MessageQueueing.MessageQueueing>();

					services.AddTransient<IMessageQueueProcessing>(provider =>
					new MessageProcessing(provider.GetRequiredService<ILoggingManagementDataService>()));

					services.AddOptions();
					services.Configure<MessageQueueAppConfig>(hostContext.Configuration.GetSection("MessageQueueAppConfig"));
					services.Configure<ConnectionStrings>(hostContext.Configuration.GetSection("ConnectionStrings"));

					services.AddSingleton<IHostedService, ReceiveMessages>();

				})
				.ConfigureLogging((hostingContext, logging) =>
				{
					logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
					logging.AddConsole();
				});

			await builder.RunConsoleAsync();
		}
	}
}
