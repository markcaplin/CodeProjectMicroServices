using System;
using System.Collections.Generic;
using System.Text;
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
using System.IO;
using Microsoft.Extensions.PlatformAbstractions;

namespace CodeProject.LoggingManagement.MessageQueueing
{
	public class StartUpConfiguration
	{
		IHostedService _sendLoggingManagementMessages;
		IHostedService _receiveLoggingManagementMessages;

		private const string Path = @"c:\myfiles\LoggingQueue.txt";

		/// <summary>
		/// Sending Hosted Service
		/// </summary>
		/// <returns></returns>
		public IHostedService SendingHostedService()
		{
			return _sendLoggingManagementMessages;
		}

		/// <summary>
		/// Receiving Hosted Service
		/// </summary>
		/// <returns></returns>
		public IHostedService ReceivingHostedService()
		{
			return _receiveLoggingManagementMessages;
		}

		public void Startup()
		{
			//
			//	get configuration information
			//
			MessageQueueAppConfig messageQueueAppConfig = new MessageQueueAppConfig();
			ConnectionStrings connectionStrings = new ConnectionStrings();

			var basePath = PlatformServices.Default.Application.ApplicationBasePath;
			if (basePath.ToLower().Contains("loggingmanagementqa"))
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

			IMessageQueueConfiguration loggingConfiguration = new MessageQueueConfiguration(MessageQueueExchanges.Logging, messageQueueAppConfig, sendingQueueConnection);

			loggingConfiguration.InitializeOutboundMessageQueueing();
			messageQueueConfigurations.Add(loggingConfiguration);

			ILoggingManagementDataService loggingManagementDataService = new LoggingManagementDataService();
			IMessageQueueProcessing messageProcessing = new MessageProcessing(loggingManagementDataService);

			_sendLoggingManagementMessages = new SendMessages(sendingQueueConnection, messageProcessing, messageQueueAppConfig, connectionStrings, messageQueueConfigurations, string.Empty);

			//
			//	set up receiving queue
			//

			IMessageQueueConnection receivingConnection = new MessageQueueConnection(messageQueueAppConfig);
			receivingConnection.CreateConnection();

			List<IMessageQueueConfiguration> inboundMessageQueueConfigurations = new List<IMessageQueueConfiguration>();
			IMessageQueueConfiguration inboundConfiguration = new MessageQueueConfiguration(messageQueueAppConfig, receivingConnection);
			inboundMessageQueueConfigurations.Add(inboundConfiguration);

			inboundConfiguration.InitializeInboundMessageQueueing(MessageQueueEndpoints.LoggingQueue);
			inboundConfiguration.InitializeLoggingExchange(MessageQueueExchanges.Logging, MessageQueueEndpoints.LoggingQueue);
			ILoggingManagementDataService inboundLoggingManagementDataService = new LoggingManagementDataService();
			IMessageQueueProcessing inboundMessageProcessing = new MessageProcessing(inboundLoggingManagementDataService);

			_receiveLoggingManagementMessages = new ReceiveMessages(receivingConnection, inboundMessageProcessing, messageQueueAppConfig, connectionStrings, inboundMessageQueueConfigurations);

		}
	}
}
