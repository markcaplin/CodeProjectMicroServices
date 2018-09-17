using CodeProject.Shared.Common.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CodeProject.Shared.Common.Models;
using CodeProject.MessageQueueing;
using Microsoft.AspNetCore.SignalR.Client;

namespace CodeProject.MessageQueueing
{
    public class TestSendMessages : IHostedService, IDisposable
	{
		private readonly IMessageQueueConnection _messageQueueConnection;
		private readonly IMessageQueueConfiguration _messageQueueConfiguration;
		private Timer _timer;

		public TestSendMessages(IMessageQueueConnection messageQueueConnection, IMessageQueueConfiguration messageQueueConfiguration)
		{
			_messageQueueConnection = messageQueueConnection;
			_messageQueueConfiguration = messageQueueConfiguration;
		}

		/// <summary>
		/// Start Process Interval
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public Task StartAsync(CancellationToken cancellationToken)
		{
			
			_timer = new Timer(GetMessagesInQueue, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));

			return Task.CompletedTask;
		}

		/// <summary>
		/// Get Messages In Queue
		/// </summary>
		/// <param name="state"></param>
		private void GetMessagesInQueue(object state)
		{
			string messageQueueName = _messageQueueConfiguration.GetMessageQueueName();
			_messageQueueConnection.IncrementCounter(messageQueueName);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_timer?.Change(Timeout.Infinite, 0);

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}

	}

}
