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

			IMessageQueueConnection messageQueueConnection = new MessageQueueConnection();
			IMessageQueueConfiguration messageQueueConfiguation1 = new MessageQueueConfiguration("first thread");
		    IMessageQueueConfiguration messageQueueConfiguation2 = new MessageQueueConfiguration("second thread");

			IHostedService testSendMessage1 = new TestSendMessages(messageQueueConnection, messageQueueConfiguation1);
			IHostedService testSendMessage2 = new TestSendMessages(messageQueueConnection, messageQueueConfiguation2);

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

					services.AddTransient<IMessageQueueProcessing>(provider => new MessageProcessing(provider.GetRequiredService<IInventoryManagementDataService>()));

					services.AddOptions();
					services.Configure<MessageQueueAppConfig>(hostContext.Configuration.GetSection("MessageQueueAppConfig"));
					services.Configure<ConnectionStrings>(hostContext.Configuration.GetSection("ConnectionStrings"));

					services.AddSingleton<IHostedService, SendMessages>();


				})
				.ConfigureServices((hostContext, services) =>
				{
					services.AddDbContext<InventoryManagementDatabase>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("PrimaryDatabaseConnectionString")));

					services.AddTransient<IInventoryManagementDataService, InventoryManagementDataService>();
					services.AddTransient<IMessageQueueing, CodeProject.MessageQueueing.MessageQueueing>();

					services.AddTransient<IMessageQueueProcessing>(provider => new MessageProcessing(provider.GetRequiredService<IInventoryManagementDataService>()));

					services.AddOptions();
					services.Configure<MessageQueueAppConfig>(hostContext.Configuration.GetSection("MessageQueueAppConfig"));
					services.Configure<ConnectionStrings>(hostContext.Configuration.GetSection("ConnectionStrings"));

					services.AddSingleton<IHostedService, ReceiveMessages>();

				})
				.ConfigureServices((hostContext, services) =>
				{
					services.AddDbContext<InventoryManagementDatabase>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("PrimaryDatabaseConnectionString")));

					services.AddTransient<IInventoryManagementDataService, InventoryManagementDataService>();
					services.AddTransient<IMessageQueueing, CodeProject.MessageQueueing.MessageQueueing>();

					services.AddTransient<IMessageQueueProcessing>(provider => new MessageProcessing(
						provider.GetRequiredService<IInventoryManagementDataService>()));

					services.AddOptions();
					services.Configure<MessageQueueAppConfig>(hostContext.Configuration.GetSection("MessageQueueAppConfig"));
					services.Configure<ConnectionStrings>(hostContext.Configuration.GetSection("ConnectionStrings"));

					services.AddSingleton<IHostedService, ProcessMessages>();

				})
				.ConfigureServices((hostContext, services) =>
				{
					services.AddTransient<IHostedService>(provider => testSendMessage1);
				})
				.ConfigureServices((hostContext, services) =>
				{
					services.AddTransient<IHostedService>(provider => testSendMessage2);
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
