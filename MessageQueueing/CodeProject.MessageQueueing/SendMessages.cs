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
using RabbitMQ.Client;
using System.IO;

namespace CodeProject.MessageQueueing
{
    public class SendMessages : IHostedService, IDisposable
	{
		private readonly List<IMessageQueueConfiguration> _messageQueueConfigurations;
		private readonly IMessageQueueConnection _messageQueueConnection;
		private readonly IMessageQueueProcessing _messageProcessor;
		private readonly MessageQueueAppConfig _appConfig;
		private readonly ConnectionStrings _connectionStrings;
		private readonly string _signalRQueue;

		private const string Path = @"c:\myfiles\InventoryQueue.txt";

		// private IBasicProperties basicProperties;
		// private IModel_channel;

		HubConnection _signalRHubConnection;
		private Timer _timer;
	
		/// <summary>
		/// Send Messages
		/// </summary>
		/// <param name="messageQueueConnection"></param>
		/// <param name="messageProcessor"></param>
		/// <param name="appConfig"></param>
		/// <param name="connectionStrings"></param>
		/// <param name="messageQueueConfigurations"></param>
		public SendMessages(IMessageQueueConnection messageQueueConnection, IMessageQueueProcessing messageProcessor, MessageQueueAppConfig appConfig, ConnectionStrings connectionStrings, List<IMessageQueueConfiguration> messageQueueConfigurations, string signalRQueue)
		{
			_messageQueueConnection = messageQueueConnection;
			_messageQueueConfigurations = messageQueueConfigurations;
			_connectionStrings = connectionStrings;
			_messageProcessor = messageProcessor;
			_appConfig = appConfig;
			_signalRQueue = signalRQueue;

			using (var sw = File.AppendText(Path))
			{
				sw.WriteLine("Send Messages Constructor");
			}

		}

		/// <summary>
		/// Start Process Interval
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public Task StartAsync(CancellationToken cancellationToken)
		{

			using (var sw = File.AppendText(Path))
			{
				sw.WriteLine("Send Mesages StartAsync");
			}

			StartSignalRConnection();

			_timer = new Timer(GetMessagesInQueue, null, TimeSpan.Zero, TimeSpan.FromSeconds(_appConfig.SendingIntervalSeconds));

			return Task.CompletedTask;
		}

		/// <summary>
		/// Start SignalR Connection
		/// </summary>
		private async void StartSignalRConnection()
		{
			using (var sw = File.AppendText(Path))
			{
				sw.WriteLine("Send Mesages StartSignalRConnection " + _appConfig.SignalRHubUrl);
			}

			if (string.IsNullOrEmpty(_appConfig.SignalRHubUrl))
			{
				return;
			}

			string url = _appConfig.SignalRHubUrl;

			Console.WriteLine("CONNECTING TO SIGNAL R  " + url);

			using (var sw = File.AppendText(Path))
			{
				sw.WriteLine("Send Mesages Connecting to Signal R" + _appConfig.SignalRHubUrl);
			}

			Boolean connected = false;
			while (connected == false)
			{
				try
				{
					using (var sw = File.AppendText(Path))
					{
						sw.WriteLine("Send Mesages Try Connecting to Signal R" + _appConfig.SignalRHubUrl);
					}

					_signalRHubConnection = new HubConnectionBuilder().WithUrl(url).Build();
					connected = true;
				}
				catch (Exception ex)
				{
					using (var sw = File.AppendText(Path))
					{
						sw.WriteLine("Send Mesages Connecting to Signal R ERROR " + ex.Message);
					}

					Console.WriteLine(ex.Message);
					await Task.Delay(5000);
				}

			}
			
			_signalRHubConnection.On<string>(_signalRQueue, (message) =>
			{
				using (var sw = File.AppendText(Path))
				{
					sw.WriteLine("Send Mesages MESSAGE RECEIVED ");
				}
				this.GetMessagesInQueue(null);

			});

			_signalRHubConnection.Closed += async (error) =>
			{
				Console.WriteLine("SignalR Connection Closed");

				using (var sw = File.AppendText(Path))
				{
					sw.WriteLine("Send Mesages Connection Closed ");
				}

				await Task.Delay(10000);
				await _signalRHubConnection.StartAsync();
				Console.WriteLine("Restart SignalR");
			};

			connected = false;
			while (connected == false)
			{
				try
				{
					using (var sw = File.AppendText(Path))
					{
						sw.WriteLine("Send Mesages Connecting to SIGNAL R StartAsync ");
					}

					Console.WriteLine("CONNECTING TO SIGNARL R");
					await _signalRHubConnection.StartAsync();
					Console.WriteLine("CONNECTED TO SIGNAL R " + url);
					connected = true;

					using (var sw = File.AppendText(Path))
					{
						sw.WriteLine("CONNECTED TO SIGNAL R ");
					}

				}
				catch (Exception ex)
				{
					using (var sw = File.AppendText(Path))
					{
						sw.WriteLine("ERROR CONNECTING TO SIGNAL R " + ex.Message);
					}

					Console.WriteLine("ERROR CONNECTING TO SIGNAL R " + ex.Message);
					await Task.Delay(10000);
				}
			}
		

		}
		/// <summary>
		/// Get Messages In Queue
		/// </summary>
		/// <param name="state"></param>
		private async void GetMessagesInQueue(object state)
		{

			using (var sw = File.AppendText(Path))
			{
				sw.WriteLine("Get Messages In Queue");
			}

			ResponseModel<List<MessageQueue>> messages = await _messageProcessor.SendQueueMessages(_messageQueueConfigurations, _appConfig.OutboundSemaphoreKey, _connectionStrings);
			Console.WriteLine("total messages " + messages.Entity.Count.ToString() + " sent at " + DateTime.Now);
			using (var sw = File.AppendText(Path))
			{
				sw.WriteLine("total messages " + messages.Entity.Count.ToString() + " sent at " + DateTime.Now);
			}
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
