using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using CodeProject.PurchaseOrderManagement.Data.EntityFramework;
using CodeProject.Shared.Common.Models;
using Microsoft.EntityFrameworkCore;
using CodeProject.PurchaseOrderManagement.Interfaces;
using CodeProject.Shared.Common.Interfaces;
using CodeProject.PurchaseOrderManagement.Business.MessageService;
using CodeProject.MessageQueueing;
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;

namespace CodeProject.PurchaseOrderManagement.MessageQueueing
{
	public class StartUpConfiguration
	{

		private const string Path = @"c:\myfiles\PurchaseOrderQueue.txt";

		IHostedService _processMessages;
		IHostedService _receivePurchaseOrderManagementMessages;
		IHostedService _submittedPurchaseOrderManagementMessages;

		/// <summary>
		/// Sending Hosted Service
		/// </summary>
		/// <returns></returns>
		public IHostedService SendingHostedService()
		{
			return _submittedPurchaseOrderManagementMessages;
		}

		/// <summary>
		/// Receiving Hosted Service
		/// </summary>
		/// <returns></returns>
		public IHostedService ReceivingHostedService()
		{
			return _receivePurchaseOrderManagementMessages;
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
			if (basePath.ToLower().Contains("purchaseordermanagementqa"))
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

			IMessageQueueConfiguration purchaseOrderSubmittedConfiguration = new MessageQueueConfiguration(MessageQueueExchanges.PurchaseOrderSubmitted, messageQueueAppConfig, sendingQueueConnection);

			purchaseOrderSubmittedConfiguration.AddQueue(MessageQueueEndpoints.InventoryQueue);
			purchaseOrderSubmittedConfiguration.AddQueue(MessageQueueEndpoints.LoggingQueue);

			purchaseOrderSubmittedConfiguration.InitializeOutboundMessageQueueing();
			messageQueueConfigurations.Add(purchaseOrderSubmittedConfiguration);

			IPurchaseOrderManagementDataService submittedPurchaseOrderManagementDataService = new PurchaseOrderManagementDataService();
			IMessageQueueProcessing messageProcessing = new MessageProcessing(submittedPurchaseOrderManagementDataService);

			_submittedPurchaseOrderManagementMessages =
				new SendMessages(sendingQueueConnection, messageProcessing, messageQueueAppConfig,
				connectionStrings, messageQueueConfigurations, MessageQueueEndpoints.PurchaseOrderQueue);

			//
			//	set up receiving queue
			//

			IMessageQueueConnection receivingConnection = new MessageQueueConnection(messageQueueAppConfig);
			receivingConnection.CreateConnection();

			List<IMessageQueueConfiguration> inboundMessageQueueConfigurations = new List<IMessageQueueConfiguration>();
			IMessageQueueConfiguration inboundConfiguration = new MessageQueueConfiguration(messageQueueAppConfig, receivingConnection);
			inboundMessageQueueConfigurations.Add(inboundConfiguration);

			inboundConfiguration.InitializeInboundMessageQueueing(MessageQueueEndpoints.PurchaseOrderQueue);
			inboundConfiguration.InitializeLoggingExchange(MessageQueueExchanges.Logging, MessageQueueEndpoints.LoggingQueue);
			IPurchaseOrderManagementDataService inboundPurchaseOrderManagementDataService = new PurchaseOrderManagementDataService();
			IMessageQueueProcessing inboundMessageProcessing = new MessageProcessing(inboundPurchaseOrderManagementDataService);

			_receivePurchaseOrderManagementMessages = new ReceiveMessages(receivingConnection, inboundMessageProcessing, messageQueueAppConfig, connectionStrings, inboundMessageQueueConfigurations);
		
			//
			//	Set Up Message Processing
			//

			IPurchaseOrderManagementDataService purchaseOrderManagementProcessingDataService = new PurchaseOrderManagementDataService();
			IMessageQueueProcessing messageProcessor = new MessageProcessing(purchaseOrderManagementProcessingDataService);
			_processMessages = new ProcessMessages(messageProcessor, messageQueueAppConfig, connectionStrings);

			var builder = new HostBuilder().ConfigureAppConfiguration((hostingContext, config) => { })
				.ConfigureServices((hostContext, services) =>
				{
					services.AddTransient<IHostedService>(provider => _processMessages);
				})
				.ConfigureServices((hostContext, services) =>
				{
					services.AddTransient<IHostedService>(provider => _submittedPurchaseOrderManagementMessages);
				})
				.ConfigureServices((hostContext, services) =>
				{
					services.AddTransient<IHostedService>(provider => _receivePurchaseOrderManagementMessages);
				})
				.ConfigureLogging((hostingContext, logging) =>
				{
					logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
					logging.AddConsole();
				});

		}
	}
}

