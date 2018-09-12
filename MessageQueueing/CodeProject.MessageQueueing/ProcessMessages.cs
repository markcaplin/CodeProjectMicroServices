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

	public class ProcessMessages : IHostedService, IDisposable
	{
		private readonly IMessageQueueProcessing _messageProcessor;
		private readonly IMessageQueueing _messageQueueing;
		private readonly ILogger _logger;
		private readonly IOptions<MessageQueueAppConfig> _appConfig;
		private Timer _timer;
		private int _counter;

		//private Subject<MessageQueue> _subject;

		public ProcessMessages(ILogger<SendMessages> logger, IOptions<MessageQueueAppConfig> appConfig, IMessageQueueing messageQueueing, IMessageQueueProcessing messageProcessor)
		{
			_logger = logger;
			_appConfig = appConfig;
			_messageProcessor = messageProcessor;
			_messageQueueing = messageQueueing;

			_logger.LogInformation("Process Messages Constructor ");
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Starting Processing Messages");

			_counter = 0;

			_timer = new Timer(ProcessMessagesInQueue, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));

			return Task.CompletedTask;
		}
		/// <summary>
		/// Get Messages In Queue
		/// </summary>
		/// <param name="state"></param>
		private async void ProcessMessagesInQueue(object state)
		{

			_counter++;

			ResponseModel<List<MessageQueue>> messages = await _messageProcessor.ProcessMessages(_appConfig.Value.InboundMessageQueue);

			_logger.LogInformation("total messages processed " + messages.Entity.Count.ToString() + " sent at " + DateTime.Now);

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
