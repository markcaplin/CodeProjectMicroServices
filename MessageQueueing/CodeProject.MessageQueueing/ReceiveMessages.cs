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
using CodeProject.Shared.Common.Models.MessageQueuePayloads;
using Newtonsoft.Json;

namespace CodeProject.MessageQueueing
{

	public class ReceiveMessages : IHostedService, IDisposable
	{
		private readonly IMessageQueueProcessing _messageProcessor;
		private readonly IMessageQueueing _messageQueueing;
		private readonly ILogger _logger;
		private readonly IOptions<MessageQueueAppConfig> _appConfig;

		private int _counter;
		private Timer _timer;

		private Subject<MessageQueue> _subject;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="appConfig"></param>
		/// <param name="messageQueueing"></param>
		/// <param name="inventoryManagementBusinessService"></param>
		public ReceiveMessages(ILogger<ReceiveMessages> logger, IOptions<MessageQueueAppConfig> appConfig, IMessageQueueing messageQueueing, IMessageQueueProcessing messageProcessor)
		{
			_logger = logger;
			_appConfig = appConfig;
			_messageProcessor = messageProcessor;
			_messageQueueing = messageQueueing;

			_messageQueueing.InitializeQueue(appConfig.Value.InboundMessageQueue, appConfig.Value.RoutingKey);

			_logger.LogInformation("Receive Messages Constructor ");
		}

		/// <summary>
		/// Start
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Starting Receiving Messages");

			_counter = 0;

			_subject = new Subject<MessageQueue>();
			_subject.Subscribe(MessageReceived);

			_timer = new Timer(GetMessagesInQueue, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));

			return Task.CompletedTask;
		}

		/// <summary>
		/// Get Messages In Queue
		/// </summary>
		/// <param name="state"></param>
		private async void GetMessagesInQueue(object state)
		{
			await Task.Delay(0);

			_logger.LogInformation("Receive Messages in Queue at " + DateTime.Now);

			await _messageQueueing.ReceiveMessages(_appConfig.Value.InboundMessageQueue, _subject, _messageProcessor);

			//_logger.LogInformation("total messages " + messages.Entity.Count.ToString() + " sent at " + DateTime.Now);

			//MessageQueue messageQueue = new MessageQueue();
			//_subject.OnNext(messageQueue);

		}
		/// <summary>
		/// Message Received
		/// </summary>
		/// <param name="messageQueue"></param>
		public async void MessageReceived(MessageQueue messageQueue)
		{
			await Task.Delay(0);

			//_logger.LogInformation($"Message Receivied: {messageQueue.TransactionQueueId}");

			//ResponseModel<MessageQueue> responseMessage = await _messageProcessor.CommitInboundMessage(messageQueue);
			//if (responseMessage.ReturnStatus == true)
			//{
			//	_logger.LogInformation($"Message Committed: {messageQueue.TransactionQueueId}");
			//	_messageQueueing.SendAcknowledgement(messageQueue.MessageGuid);
			//}

		}

		/// <summary>
		/// Stop Async
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Stopping.");

			return Task.CompletedTask;
		}

		public void Dispose()
		{
		
		}
	}
}
