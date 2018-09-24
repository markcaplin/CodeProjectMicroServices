using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CodeProject.SalesOrderManagement.Data.EntityFramework;
using CodeProject.Shared.Common.Models;
using Microsoft.EntityFrameworkCore;
using CodeProject.SalesOrderManagement.Interfaces;
using CodeProject.Shared.Common.Interfaces;
using CodeProject.SalesOrderManagement.Business.MessageService;
using CodeProject.MessageQueueing;
using System.IO;
using System.Collections.Generic;

namespace CodeProject.SalesOrderManagement.MessageQueueing
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

			//
			//	set up receiving queue
			//
			IMessageQueueConnection receivingConnection = new MessageQueueConnection(messageQueueAppConfig);
			receivingConnection.CreateConnection();

			List<IMessageQueueConfiguration> inboundMessageQueueConfigurations = new List<IMessageQueueConfiguration>();
			IMessageQueueConfiguration inboundConfiguration = new MessageQueueConfiguration(messageQueueAppConfig, receivingConnection);
			inboundMessageQueueConfigurations.Add(inboundConfiguration);

			inboundConfiguration.InitializeInboundMessageQueueing(MessageQueueEndpoints.SalesOrderQueue);
			inboundConfiguration.InitializeLoggingExchange(MessageQueueExchanges.Logging, MessageQueueEndpoints.LoggingQueue);
			ISalesOrderManagementDataService inboundSalesOrderManagementDataService = new SalesOrderManagementDataService();
			IMessageQueueProcessing inboundMessageProcessing = new MessageProcessing(inboundSalesOrderManagementDataService);

			IHostedService receiveSalesOrderManagementMessages = new ReceiveMessages(receivingConnection, inboundMessageProcessing, messageQueueAppConfig, connectionStrings, inboundMessageQueueConfigurations);
	
			//
			//	Set Up Message Processing
			//
			ISalesOrderManagementDataService salesOrderManagementProcessingDataService = new SalesOrderManagementDataService();
			IMessageQueueProcessing messageProcessor = new MessageProcessing(salesOrderManagementProcessingDataService);
			ProcessMessages processMessages = new ProcessMessages(messageProcessor, messageQueueAppConfig, connectionStrings);

			var builder = new HostBuilder().ConfigureAppConfiguration((hostingContext, config) =>
			{
				//string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
				//string jsonFile = $"appsettings.{environment}.json";
				//config.AddJsonFile(jsonFile, optional: true);
				//config.AddEnvironmentVariables();

				//if (args != null)
				//{
				//	config.AddCommandLine(args);
				//}

			})
			.ConfigureServices((hostContext, services) =>
			{
				/*services.AddDbContext<SalesOrderManagementDatabase>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("PrimaryDatabaseConnectionString")));

				services.AddTransient<ISalesOrderManagementDataService, SalesOrderManagementDataService>();
				services.AddTransient<IMessageQueueing, CodeProject.MessageQueueing.MessageQueueing>();

				services.AddTransient<IMessageQueueProcessing>(provider =>
				new MessageProcessing(provider.GetRequiredService<ISalesOrderManagementDataService>()));

				services.AddOptions();
				services.Configure<MessageQueueAppConfig>(hostContext.Configuration.GetSection("MessageQueueAppConfig"));
				services.Configure<ConnectionStrings>(hostContext.Configuration.GetSection("ConnectionStrings"));

				services.AddSingleton<IHostedService, SendMessages>();*/


			})
			.ConfigureServices((hostContext, services) =>
			{
				/*services.AddDbContext<SalesOrderManagementDatabase>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("PrimaryDatabaseConnectionString")));

				services.AddTransient<ISalesOrderManagementDataService, SalesOrderManagementDataService>();
				services.AddTransient<IMessageQueueing, CodeProject.MessageQueueing.MessageQueueing>();

				services.AddTransient<IMessageQueueProcessing>(provider =>
				new MessageProcessing(provider.GetRequiredService<ISalesOrderManagementDataService>()));

				services.AddOptions();
				services.Configure<MessageQueueAppConfig>(hostContext.Configuration.GetSection("MessageQueueAppConfig"));
				services.Configure<ConnectionStrings>(hostContext.Configuration.GetSection("ConnectionStrings"));

				services.AddSingleton<IHostedService, ReceiveMessages>();*/

			})
			.ConfigureServices((hostContext, services) =>
			{
				/*services.AddDbContext<SalesOrderManagementDatabase>(options => options.UseSqlServer(hostContext.Configuration.GetConnectionString("PrimaryDatabaseConnectionString")));

				services.AddTransient<ISalesOrderManagementDataService, SalesOrderManagementDataService>();
				services.AddTransient<IMessageQueueing, CodeProject.MessageQueueing.MessageQueueing>();

				services.AddTransient<IMessageQueueProcessing>(provider =>
				new MessageProcessing(provider.GetRequiredService<ISalesOrderManagementDataService>()));

				services.AddOptions();
				services.Configure<MessageQueueAppConfig>(hostContext.Configuration.GetSection("MessageQueueAppConfig"));
				services.Configure<ConnectionStrings>(hostContext.Configuration.GetSection("ConnectionStrings"));

				services.AddSingleton<IHostedService, ProcessMessages>();*/

			})
			.ConfigureServices((hostContext, services) =>
			{
				services.AddTransient<IHostedService>(provider => processMessages);
			})
			.ConfigureServices((hostContext, services) =>
			{
					services.AddTransient<IHostedService>(provider => receiveSalesOrderManagementMessages);
			})
			.ConfigureLogging((hostingContext, logging) =>
			{
				//logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
				//logging.AddConsole();
			});

			await builder.RunConsoleAsync();
		}
	}
}
