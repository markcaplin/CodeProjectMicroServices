using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using CodeProject.Shared.Common.Interfaces;
using CodeProject.Shared.Common.Models;
using CodeProject.Shared.Common.Utilities;
using System.Reactive.Subjects;
using System.Collections;
using System.Threading.Tasks;

namespace CodeProject.MessageQueueing
{
	public class MessageQueueing : IMessageQueueing, IDisposable
	{
		private List<object> _items;

		private string _hostName = "localhost";
		private string _userName = "guest";
		private string _password = "guest";

		private string _exchangeName { get; set; }
		private string _routingKey { get; set; }

		private IConnection _connection;
		private ConnectionFactory _connectionFactory;
		private IBasicProperties _basicProperties;
		private IModel _channel;

		private Subscription _subscription;

		private Hashtable _receivedMessages;

		private bool _running;

		/// <summary>
		/// Message Queueing
		/// </summary>
		public MessageQueueing()
		{
			_items = new List<object>();

			_connectionFactory = new ConnectionFactory();

			_connectionFactory.HostName = _hostName;
			_connectionFactory.UserName = _userName;
			_connectionFactory.Password = _password;

			_connection = _connectionFactory.CreateConnection();
			_channel = _connection.CreateModel();

			_basicProperties = _channel.CreateBasicProperties();
			_basicProperties.Persistent = true;

			_receivedMessages = new Hashtable();

			_running = false;

		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="exchangeName"></param>
		public void InitializeExchange(string exchangeName, string routingKey)
		{
			_channel.ExchangeDeclare(exchangeName, "fanout", true, false);
			_exchangeName = exchangeName;
			_routingKey = routingKey;
		}

		/// <summary>
		/// Initialize Queue
		/// </summary>
		/// <param name="queueName"></param>
		public void InitializeQueue(string queueName)
		{
			_channel.QueueDeclare(queueName, true, false, false);
			_channel.QueueBind(queueName, _exchangeName, _routingKey);
		}

		/// <summary>
		/// Initialize Queue
		/// </summary>
		/// <param name="queueName"></param>
		/// <param name="routingKey"></param>
		public void InitializeQueue(string queueName, string routingKey)
		{
			_channel.QueueDeclare(queueName, true, false, false);
			_routingKey = routingKey;
		}

		/// <summary>
		/// Send Message
		/// </summary>
		/// <param name="entity"></param>
		public ResponseModel<MessageQueue> SendMessage(object entity)
		{
			ResponseModel<MessageQueue> response = new ResponseModel<MessageQueue>();
			response.Entity = new MessageQueue();

			try
			{
				string output = JsonConvert.SerializeObject(entity);

				byte[] payload = Encoding.UTF8.GetBytes(output);

				PublicationAddress address = new PublicationAddress(ExchangeType.Fanout, _exchangeName, _routingKey);

				_channel.BasicPublish(address, _basicProperties, payload);

				response.Entity.Payload = output;

				response.ReturnStatus = true;
			}
			catch (Exception ex)
			{
				response.ReturnStatus = false;
				response.ReturnMessage.Add(ex.Message);
			}

			return response;

		}

		/// <summary>
		/// Receive Messages
		/// </summary>
		/// <param name="queueName"></param>
		/// <param name="subject"></param>
		public async Task ReceiveMessages(string queueName, Subject<MessageQueue> subject, IMessageQueueProcessing _messageProcessor)
		{

			await Task.Delay(0);

			Console.WriteLine("Receiving Messages at " + DateTime.Now);

			if (_running == true) {
				return;
			}

			_running = true;

			var response = _channel.QueueDeclarePassive(queueName);

			_channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

			_subscription = new Subscription(_channel, queueName, false);

			foreach (BasicDeliverEventArgs e in _subscription)
			{
				string message = Encoding.UTF8.GetString(e.Body);

				MessageQueue messageQueue = JsonConvert.DeserializeObject<MessageQueue>(message);
				messageQueue.MessageGuid = Guid.NewGuid();

				Console.WriteLine("Receiving Message id " + messageQueue.TransactionQueueId);

				ResponseModel<MessageQueue> responseMessage = await _messageProcessor.CommitInboundMessage(messageQueue);
				if (responseMessage.ReturnStatus == true)
				{
					Console.WriteLine($"Message Committed: {messageQueue.TransactionQueueId}");
					_subscription.Ack(e);
				}

				//_receivedMessages.Add(messageQueue.MessageGuid, e);

				//subject.OnNext(messageQueue);

				//break;

			}

		}

		/// <summary>
		/// Send Acknowledgement
		/// </summary>
		/// <param name="messageGuid"></param>
		public void SendAcknowledgement(Guid messageGuid)
		{

			if (_receivedMessages.ContainsKey(messageGuid))
			{
				BasicDeliverEventArgs eventArgs = (BasicDeliverEventArgs) _receivedMessages[messageGuid];
				_subscription.Ack(eventArgs);
				Console.WriteLine($"Message acknowledged: {messageGuid}");
			}

		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;

				_connection = null;
				_connectionFactory = null;
				_channel = null;
				_subscription = null;

			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~MessageQueueing() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion
	}
}
