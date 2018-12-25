using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using CodeProject.InventoryManagement.Data.EntityFramework;
using CodeProject.Shared.Common.Models;
using Microsoft.EntityFrameworkCore;
using CodeProject.InventoryManagement.Interfaces;
using CodeProject.Shared.Common.Interfaces;
using CodeProject.InventoryManagement.Business.MessageService;
using CodeProject.MessageQueueing;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;

namespace CodeProject.InventoryManagement.MessageQueueing
{
	public class StartUpConfiguration
	{
		
		private const string Path = @"c:\myfiles\InventoryQueue.txt";

		IHostedService _processMessages;
		IHostedService _receiveInventoryManagementMessages;
		IHostedService _sendInventoryManagementMessages;

		/// <summary>
		/// Sending Hosted Service
		/// </summary>
		/// <returns></returns>
		public IHostedService SendingHostedService()
		{
			return _sendInventoryManagementMessages;
		}

		/// <summary>
		/// Receiving Hosted Service
		/// </summary>
		/// <returns></returns>
		public IHostedService ReceivingHostedService()
		{
			return _receiveInventoryManagementMessages;
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
			if (basePath.ToLower().Contains("inventorymanagementqa"))
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

			//
			//	Inventory Received Transactions
			//

			IMessageQueueConfiguration inventoryReceivedConfiguration = new MessageQueueConfiguration(MessageQueueExchanges.InventoryReceived, messageQueueAppConfig, sendingQueueConnection);

			inventoryReceivedConfiguration.AddQueue(MessageQueueEndpoints.SalesOrderQueue);
			inventoryReceivedConfiguration.AddQueue(MessageQueueEndpoints.PurchaseOrderQueue);
			inventoryReceivedConfiguration.AddQueue(MessageQueueEndpoints.LoggingQueue);

			inventoryReceivedConfiguration.InitializeOutboundMessageQueueing();
			messageQueueConfigurations.Add(inventoryReceivedConfiguration);

			//
			//	Product Creation and Updates
			//

			IMessageQueueConfiguration productUpdatedConfiguration = new MessageQueueConfiguration(MessageQueueExchanges.ProductUpdated, messageQueueAppConfig, sendingQueueConnection);

			productUpdatedConfiguration.AddQueue(MessageQueueEndpoints.SalesOrderQueue);
			productUpdatedConfiguration.AddQueue(MessageQueueEndpoints.PurchaseOrderQueue);
			productUpdatedConfiguration.AddQueue(MessageQueueEndpoints.LoggingQueue);

			productUpdatedConfiguration.InitializeOutboundMessageQueueing();
			messageQueueConfigurations.Add(productUpdatedConfiguration);

			//
			//	Inventory Shipped Transactions
			//

			IMessageQueueConfiguration inventoryShippedConfiguration = new MessageQueueConfiguration(MessageQueueExchanges.InventoryShipped, messageQueueAppConfig, sendingQueueConnection);

			inventoryShippedConfiguration.AddQueue(MessageQueueEndpoints.SalesOrderQueue);
			inventoryShippedConfiguration.AddQueue(MessageQueueEndpoints.LoggingQueue);

			inventoryShippedConfiguration.InitializeOutboundMessageQueueing();
			messageQueueConfigurations.Add(inventoryShippedConfiguration);

			//
			//	initialize Sending Messages
			//

			IInventoryManagementDataService inventoryManagementDataService = new InventoryManagementDataService();
			IMessageQueueProcessing messageProcessing = new MessageProcessing(inventoryManagementDataService);

			_sendInventoryManagementMessages = new SendMessages(sendingQueueConnection, messageProcessing,
				messageQueueAppConfig, connectionStrings, messageQueueConfigurations, MessageQueueEndpoints.InventoryQueue);
			
			//
			//	set up receiving queue
			//

			IMessageQueueConnection receivingConnection = new MessageQueueConnection(messageQueueAppConfig);
			receivingConnection.CreateConnection();

			List<IMessageQueueConfiguration> inboundMessageQueueConfigurations = new List<IMessageQueueConfiguration>();
			IMessageQueueConfiguration inboundConfiguration = new MessageQueueConfiguration(messageQueueAppConfig, receivingConnection);
			inboundMessageQueueConfigurations.Add(inboundConfiguration);

			inboundConfiguration.InitializeInboundMessageQueueing(MessageQueueEndpoints.InventoryQueue);
			inboundConfiguration.InitializeLoggingExchange(MessageQueueExchanges.Logging, MessageQueueEndpoints.LoggingQueue);
			IInventoryManagementDataService inboundInventoryManagementDataService = new InventoryManagementDataService();
			IMessageQueueProcessing inboundMessageProcessing = new MessageProcessing(inboundInventoryManagementDataService);

			_receiveInventoryManagementMessages = new ReceiveMessages(receivingConnection, inboundMessageProcessing, messageQueueAppConfig, connectionStrings, inboundMessageQueueConfigurations);
			
			//
			//	Set Up Message Processing
			//

			IInventoryManagementDataService inventoryManagementProcessingDataService = new InventoryManagementDataService();
			IMessageQueueProcessing messageProcessor = new MessageProcessing(inventoryManagementProcessingDataService);

			_processMessages = new ProcessMessages(messageProcessor, messageQueueAppConfig, connectionStrings);

		}
	}
}

