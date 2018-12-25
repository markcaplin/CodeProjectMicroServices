using System;
using System.Collections.Generic;
using System.Text;

namespace CodeProject.Shared.Common.Models.AppSettings
{
	public class ConnectionStrings
	{
		public string PrimaryDatabaseConnectionString { get; set; }
	}

	public class MessageQueueAppConfig
	{
		public bool RunAsService { get; set; }
		public string MessageQueueHostName { get; set; }
		public string MessageQueueUserName { get; set; }
		public string MessageQueuePassword { get; set; }
		public string MessageQueueEnvironment { get; set; }
		public string SignalRHubUrl { get; set; }
		public string RoutingKey { get; set; }
		public string InboundSemaphoreKey { get; set; }
		public string OutboundSemaphoreKey { get; set; }
		public string AcknowledgementMessageExchangeSuffix { get; set; }
		public bool SendToLoggingQueue { get; set; }
		public int ProcessingIntervalSeconds { get; set; }
		public int SendingIntervalSeconds { get; set; }
		public int ReceivingIntervalSeconds { get; set; }
		public bool QueueImmediately { get; set; }
		public string TriggerExchangeName { get; set; }
		public string TriggerQueueName { get; set; }
		public string ExchangeName { get; set; }
		public string InboundMessageQueue { get; set; }
		public string OutboundMessageQueues { get; set; }
		public string LoggingExchangeName { get; set; }
		public string LoggingMessageQueue { get; set; }
		public string OriginatingQueueName { get; set; }
		public string AcknowledgementMessageQueueSuffix { get; set; }
	}

	public class LogLevel
	{
		public string Default { get; set; }
	}

	public class Logging
	{
		public LogLevel LogLevel { get; set; }
	}

	public class MessageQueueAppSettings
	{
		public ConnectionStrings ConnectionStrings { get; set; }
		public MessageQueueAppConfig MessageQueueAppConfig { get; set; }
		public Logging Logging { get; set; }
	}

}
