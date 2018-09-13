using System;
using System.Collections.Generic;
using System.Text;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CodeProject.Shared.Common.Models;
using CodeProject.Shared.Common.Interfaces;
using CodeProject.MessageQueueing;

namespace CodeProject.MessageQueueing
{
    
	public class SendMessages : IHostedService, IDisposable
	{
		private readonly IMessageQueueProcessing _messageProcessor;
		private readonly IMessageQueueing _messageQueueing;
		private readonly ILogger _logger;
		private readonly IOptions<MessageQueueAppConfig> _appConfig;
		private Timer _timer;
	
		public SendMessages(ILogger<SendMessages> logger, IOptions<MessageQueueAppConfig> appConfig, IMessageQueueing messageQueueing, IMessageQueueProcessing messageProcessor)
		{
			_logger = logger;
			_appConfig = appConfig;
			_messageProcessor = messageProcessor;
			_messageQueueing = messageQueueing;

			_messageQueueing.InitializeMessageQueueing(appConfig.Value.MessageQueueHostName, appConfig.Value.MessageQueueUserName, appConfig.Value.MessageQueuePassword);
			_messageQueueing.SetInboundSemaphoreKey(appConfig.Value.InboundSemaphoreKey);
			_messageQueueing.SetOutboundSemaphoreKey(appConfig.Value.OutboundSemaphoreKey);

			_messageQueueing.InitializeExchange(appConfig.Value.ExchangeName, appConfig.Value.RoutingKey);
			_messageQueueing.InitializeAcknowledgementConfiguration(appConfig.Value.AcknowledgementMessageExchangeSuffix, appConfig.Value.AcknowledgementMessageQueueSuffix);

			string[] outboundQueues = appConfig.Value.OutboundMessageQueues.Split(",");

			foreach(string outboundQueue in outboundQueues)
			{
				_messageQueueing.InitializeQueue(outboundQueue);
			}

			_logger.LogInformation("Send Messages Constructor " + appConfig.Value.ExchangeName);
		}

		/// <summary>
		/// Start Process Interval
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Starting Send Messages");

			_timer = new Timer(GetMessagesInQueue, null, TimeSpan.Zero, TimeSpan.FromSeconds(_appConfig.Value.SendingIntervalSeconds));

			return Task.CompletedTask;
		}
		/// <summary>
		/// Get Messages In Queue
		/// </summary>
		/// <param name="state"></param>
		private async void GetMessagesInQueue(object state)
		{
			ResponseModel<List<MessageQueue>> messages = await _messageProcessor.SendQueueMessages(_messageQueueing, _appConfig.Value.OutboundSemaphoreKey);
			_logger.LogInformation("total messages " + messages.Entity.Count.ToString() + " sent at " + DateTime.Now);

		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Stopping.");

			_timer?.Change(Timeout.Infinite, 0);

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}
	}
}
