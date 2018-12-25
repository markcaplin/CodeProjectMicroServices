using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using CodeProject.SalesOrderManagement.Data.EntityFramework;
using CodeProject.Shared.Common.Models;
using Microsoft.EntityFrameworkCore;
using CodeProject.SalesOrderManagement.Interfaces;
using CodeProject.Shared.Common.Interfaces;
using CodeProject.SalesOrderManagement.Business.MessageService;
using CodeProject.MessageQueueing;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;

namespace CodeProject.SalesOrderManagement.MessageQueueing
{
	public class StartUpConfiguration
	{

		private const string Path = @"c:\myfiles\SalesOrderQueue.txt";

		IHostedService _processMessages;
		IHostedService _receiveSalesOrderManagementMessages;
		IHostedService _sendSalesOrderManagementMessages;

		/// <summary>
		/// Sending Hosted Service
		/// </summary>
		/// <returns></returns>
		public IHostedService SendingHostedService()
		{
			return _sendSalesOrderManagementMessages;
		}

		/// <summary>
		/// Receiving Hosted Service
		/// </summary>
		/// <returns></returns>
		public IHostedService ReceivingHostedService()
		{
			return _receiveSalesOrderManagementMessages;
		}

		/// <summary>
		/// Process Messages Hosted Service
		/// </summary>
		/// <returns></returns>
		public IHostedService ProcessMessagesHostedService()
		{
			return _processMessages;
		}

		public void Startup()
		{
			//
			//	get configuration information
			//

			MessageQueueAppConfig messageQueueAppConfig = new MessageQueueAppConfig();
			ConnectionStrings connectionStrings = new ConnectionStrings();

			var basePath = PlatformServices.Default.Application.ApplicationBasePath;
			if (basePath.ToLower().Contains("salesordermanagementqa"))
			{
				CodeProject.Shared.Common.Models.AppSettings.MessageQueueAppSettings appSettings = new CodeProject.Shared.Common.Models.AppSettings.MessageQueueAppSettings();

				string readContents;
				using (StreamReader streamReader = new StreamReader(basePath + @"\AppSettings.QA.json", Encoding.UTF8))
				{
					readContents = streamReader.ReadToEnd();
				}

				appSettings = CodeProject.Shared.Common.Utilities.SerializationFunction<CodeProject.Shared.Common.Models.AppSettings.MessageQueueAppSettings>.ReturnObjectFromString(readContents);

				messageQueueAppConfig.MessageQueueHostName = appSettings.MessageQueueAppConfig.MessageQueueHostName;
				messageQueueAppConfig.MessageQueueUserName = appSettings.MessageQueueAppConfig.MessageQueueUserName;
				messageQueueAppConfig.MessageQueuePassword = appSettings.MessageQueueAppConfig.MessageQueuePassword;
				messageQueueAppConfig.MessageQueueEnvironment = appSettings.MessageQueueAppConfig.MessageQueueEnvironment;
				messageQueueAppConfig.ExchangeName = appSettings.MessageQueueAppConfig.ExchangeName;
				messageQueueAppConfig.RoutingKey = appSettings.MessageQueueAppConfig.RoutingKey;
				messageQueueAppConfig.InboundMessageQueue = appSettings.MessageQueueAppConfig.InboundMessageQueue;
				messageQueueAppConfig.OutboundMessageQueues = appSettings.MessageQueueAppConfig.OutboundMessageQueues;
				messageQueueAppConfig.LoggingExchangeName = appSettings.MessageQueueAppConfig.LoggingExchangeName;
				messageQueueAppConfig.LoggingMessageQueue = appSettings.MessageQueueAppConfig.LoggingMessageQueue;
				messageQueueAppConfig.OriginatingQueueName = appSettings.MessageQueueAppConfig.OriginatingQueueName;
				messageQueueAppConfig.SendToLoggingQueue = appSettings.MessageQueueAppConfig.SendToLoggingQueue;
				messageQueueAppConfig.AcknowledgementMessageExchangeSuffix = appSettings.MessageQueueAppConfig.AcknowledgementMessageExchangeSuffix;
				messageQueueAppConfig.AcknowledgementMessageQueueSuffix = appSettings.MessageQueueAppConfig.AcknowledgementMessageQueueSuffix;
				messageQueueAppConfig.TriggerExchangeName = appSettings.MessageQueueAppConfig.TriggerExchangeName;
				messageQueueAppConfig.TriggerQueueName = appSettings.MessageQueueAppConfig.TriggerQueueName;
				messageQueueAppConfig.QueueImmediately = appSettings.MessageQueueAppConfig.QueueImmediately;
				messageQueueAppConfig.InboundSemaphoreKey = appSettings.MessageQueueAppConfig.InboundSemaphoreKey;
				messageQueueAppConfig.OutboundSemaphoreKey = appSettings.MessageQueueAppConfig.OutboundSemaphoreKey;
				messageQueueAppConfig.ProcessingIntervalSeconds = appSettings.MessageQueueAppConfig.ProcessingIntervalSeconds;
				messageQueueAppConfig.SendingIntervalSeconds = appSettings.MessageQueueAppConfig.SendingIntervalSeconds;
				messageQueueAppConfig.ReceivingIntervalSeconds = appSettings.MessageQueueAppConfig.ReceivingIntervalSeconds;
				messageQueueAppConfig.SignalRHubUrl = appSettings.MessageQueueAppConfig.SignalRHubUrl;
				messageQueueAppConfig.RunAsService = appSettings.MessageQueueAppConfig.RunAsService;

				connectionStrings.PrimaryDatabaseConnectionString = appSettings.ConnectionStrings.PrimaryDatabaseConnectionString;

				using (var sw = File.AppendText(Path))
				{
					sw.WriteLine("HostName=" + messageQueueAppConfig.MessageQueueHostName + "*");
				}
			}
			else
			{
				string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
				string jsonFile = $"AppSettings.{environment}.json";

				var configBuilder = new ConfigurationBuilder()
				  .SetBasePath(Directory.GetCurrentDirectory())
				  .AddJsonFile(jsonFile, optional: true, reloadOnChange: true);

				IConfigurationRoot configuration = configBuilder.Build();

				configuration.GetSection("MessageQueueAppConfig").Bind(messageQueueAppConfig);
				configuration.GetSection("ConnectionStrings").Bind(connectionStrings);

			}

			//
			//	set up sending queue
			//

			IMessageQueueConnection sendingQueueConnection = new MessageQueueConnection(messageQueueAppConfig);
			sendingQueueConnection.CreateConnection();

			List<IMessageQueueConfiguration> messageQueueConfigurations = new List<IMessageQueueConfiguration>();

			IMessageQueueConfiguration salesOrderSubmittedConfiguration = new MessageQueueConfiguration(MessageQueueExchanges.SalesOrderSubmitted, messageQueueAppConfig, sendingQueueConnection);

			salesOrderSubmittedConfiguration.AddQueue(MessageQueueEndpoints.InventoryQueue);
			salesOrderSubmittedConfiguration.AddQueue(MessageQueueEndpoints.LoggingQueue);

			salesOrderSubmittedConfiguration.InitializeOutboundMessageQueueing();
			messageQueueConfigurations.Add(salesOrderSubmittedConfiguration);

			ISalesOrderManagementDataService salesOrderManagementDataService = new SalesOrderManagementDataService();
			IMessageQueueProcessing messageProcessing = new MessageProcessing(salesOrderManagementDataService);

			_sendSalesOrderManagementMessages =
				new SendMessages(sendingQueueConnection, messageProcessing, messageQueueAppConfig,
				connectionStrings, messageQueueConfigurations, MessageQueueEndpoints.SalesOrderQueue);


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

			_receiveSalesOrderManagementMessages = new ReceiveMessages(receivingConnection, inboundMessageProcessing, messageQueueAppConfig, connectionStrings, inboundMessageQueueConfigurations);

			//
			//	Set Up Message Processing
			//

			ISalesOrderManagementDataService salesOrderManagementProcessingDataService = new SalesOrderManagementDataService();
			IMessageQueueProcessing messageProcessor = new MessageProcessing(salesOrderManagementProcessingDataService);
			_processMessages = new ProcessMessages(messageProcessor, messageQueueAppConfig, connectionStrings);

			var builder = new HostBuilder().ConfigureAppConfiguration((hostingContext, config) =>
			{

			})
			.ConfigureServices((hostContext, services) =>
			{
				services.AddTransient<IHostedService>(provider => _sendSalesOrderManagementMessages);
			})
			.ConfigureServices((hostContext, services) =>
			{
				services.AddTransient<IHostedService>(provider => _processMessages);
			})
			.ConfigureServices((hostContext, services) =>
			{
				services.AddTransient<IHostedService>(provider => _receiveSalesOrderManagementMessages);
			});

		}
	}
}

