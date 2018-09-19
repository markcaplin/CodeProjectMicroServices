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
using System.IO;
using System.Collections.Generic;

namespace CodeProject.InventoryManagement.MessageQueueing
{
	class Program
	{
		public static async Task Main(string[] args)
		{

			MessageQueueAppConfig messageQueueAppConfig = new MessageQueueAppConfig();
			ConnectionStrings connectionStrings = new ConnectionStrings();

			string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			string jsonFile = $"appsettings.{environment}.json";

			var configBuilder = new ConfigurationBuilder()
			  .SetBasePath(Directory.GetCurrentDirectory())
			  .AddJsonFile(jsonFile, optional: true, reloadOnChange: true);

			IConfigurationRoot configuration = configBuilder.Build();

			configuration.GetSection("MessageQueueAppConfig").Bind(messageQueueAppConfig);
			configuration.GetSection("ConnectionStrings").Bind(connectionStrings);

			IMessageQueueConnection messageQueueConnection = new MessageQueueConnection(messageQueueAppConfig);
			messageQueueConnection.CreateConnection();

			List<IMessageQueueConfiguration> messageQueueConfigurations = new List<IMessageQueueConfiguration>();

			IMessageQueueConfiguration productUpdatedConfiguration = new MessageQueueConfiguration(MessageQueueExchanges.ProductUpdated, messageQueueAppConfig, messageQueueConnection);
			productUpdatedConfiguration.AddQueue(MessageQueueEndpoints.SalesOrderQueue);
			productUpdatedConfiguration.AddQueue(MessageQueueEndpoints.PurchaseOrderQueue);
			productUpdatedConfiguration.AddQueue(MessageQueueEndpoints.LoggingQueue);
			productUpdatedConfiguration.InitializeMessageQueueing();
			messageQueueConfigurations.Add(productUpdatedConfiguration);

			IMessageQueueConfiguration orderShippedConfiguration = new MessageQueueConfiguration(MessageQueueExchanges.OrderShipped, messageQueueAppConfig, messageQueueConnection);
			orderShippedConfiguration.AddQueue(MessageQueueEndpoints.SalesOrderQueue);
			orderShippedConfiguration.AddQueue(MessageQueueEndpoints.LoggingQueue);
			orderShippedConfiguration.InitializeMessageQueueing();
			messageQueueConfigurations.Add(orderShippedConfiguration);

			IMessageQueueing messageQueueing = new CodeProject.MessageQueueing.MessageQueueing();
			IInventoryManagementDataService inventoryManagementDataService = new InventoryManagementDataService();
			IMessageQueueProcessing messageProcessing = new MessageProcessing(inventoryManagementDataService);

			IHostedService sendInventoryManagementMessages = new TestSendMessages(messageQueueConnection, messageProcessing, messageQueueAppConfig, connectionStrings, messageQueueConfigurations);
	
			var builder = new HostBuilder().ConfigureAppConfiguration((hostingContext, config) =>
				{
					string aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
					string jsonConfigFile = $"appsettings.{aspNetCoreEnvironment}.json";
					config.AddJsonFile(jsonConfigFile, optional: true);
					config.AddEnvironmentVariables();

					if (args != null)
					{
						config.AddCommandLine(args);
					}

				})
				.ConfigureServices((hostContext, services) =>
				{
					/*services.AddDbContext<InventoryManagementDatabase>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("PrimaryDatabaseConnectionString")));

					services.AddTransient<IInventoryManagementDataService, InventoryManagementDataService>();
					services.AddTransient<IMessageQueueing, CodeProject.MessageQueueing.MessageQueueing>();

					services.AddTransient<IMessageQueueProcessing>(provider => new MessageProcessing(provider.GetRequiredService<IInventoryManagementDataService>()));

					services.AddOptions();
					services.Configure<MessageQueueAppConfig>(hostContext.Configuration.GetSection("MessageQueueAppConfig"));
					services.Configure<ConnectionStrings>(hostContext.Configuration.GetSection("ConnectionStrings"));

					services.AddSingleton<IHostedService, SendMessages>();*/


				})
				.ConfigureServices((hostContext, services) =>
				{
					/*services.AddDbContext<InventoryManagementDatabase>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("PrimaryDatabaseConnectionString")));

					services.AddTransient<IInventoryManagementDataService, InventoryManagementDataService>();
					services.AddTransient<IMessageQueueing, CodeProject.MessageQueueing.MessageQueueing>();

					services.AddTransient<IMessageQueueProcessing>(provider => new MessageProcessing(provider.GetRequiredService<IInventoryManagementDataService>()));

					services.AddOptions();
					services.Configure<MessageQueueAppConfig>(hostContext.Configuration.GetSection("MessageQueueAppConfig"));
					services.Configure<ConnectionStrings>(hostContext.Configuration.GetSection("ConnectionStrings"));

					services.AddSingleton<IHostedService, ReceiveMessages>();*/

				})
				.ConfigureServices((hostContext, services) =>
				{
					/*services.AddDbContext<InventoryManagementDatabase>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("PrimaryDatabaseConnectionString")));

					services.AddTransient<IInventoryManagementDataService, InventoryManagementDataService>();
					services.AddTransient<IMessageQueueing, CodeProject.MessageQueueing.MessageQueueing>();

					services.AddTransient<IMessageQueueProcessing>(provider => new MessageProcessing(
						provider.GetRequiredService<IInventoryManagementDataService>()));

					services.AddOptions();
					services.Configure<MessageQueueAppConfig>(hostContext.Configuration.GetSection("MessageQueueAppConfig"));
					services.Configure<ConnectionStrings>(hostContext.Configuration.GetSection("ConnectionStrings"));

					services.AddSingleton<IHostedService, ProcessMessages>();*/

				})
				.ConfigureServices((hostContext, services) =>
				{
					services.AddTransient<IHostedService>(provider => sendInventoryManagementMessages);
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
