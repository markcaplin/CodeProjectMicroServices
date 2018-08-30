using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using CodeProject.Shared.Interfaces;

namespace CodeProject.MessageQueueing
{
	public class MessageQueueing<T> : IDisposable, IMessageQueueing<T>
	{
		private List<T> _items;

		private string _hostName = "localhost";
		private string _userName = "guest";
		private string _password = "guest";

		private IConnection _connection;
		private ConnectionFactory _connectionFactory;
		private IBasicProperties _basicProperties;
		private IModel _channel;

		private Subscription _subscription;

		private List<BasicDeliverEventArgs> _receivedMessages;

		/// <summary>
		/// Message Queueing
		/// </summary>
		public MessageQueueing()
		{
			_items = new List<T>();

			_connectionFactory = new ConnectionFactory();
			_connectionFactory.HostName = _hostName;
			_connectionFactory.UserName = _userName;
			_connectionFactory.Password = _password;

			_connection = _connectionFactory.CreateConnection();
			_channel = _connection.CreateModel();

			_basicProperties = _channel.CreateBasicProperties();
			_basicProperties.Persistent = true;

			_receivedMessages = new List<BasicDeliverEventArgs>();

		}

		/// <summary>
		/// Send Message
		/// </summary>
		/// <param name="exchangeName"></param>
		/// <param name="routingKey"></param>
		/// <param name="entity"></param>
		public void SendMessage(string exchangeName, string routingKey, T entity)
		{

			string output = JsonConvert.SerializeObject(entity);

			byte[] payload = Encoding.UTF8.GetBytes(output);

			PublicationAddress address = new PublicationAddress(ExchangeType.Fanout, exchangeName, routingKey);

			_channel.BasicPublish(address, _basicProperties, payload);

		}

		/// <summary>
		/// Receive Messages
		/// </summary>
		/// <param name="queueName"></param>
		public List<T> ReceiveMessages(string queueName)
		{

			var response = _channel.QueueDeclarePassive(queueName);

			Console.WriteLine("message count = " + response.MessageCount);

			if (response.MessageCount == 0)
			{
				return _items;
			}

			int messagesProcessed = 0;

			_channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

			_subscription = new Subscription(_channel, queueName, false);

			foreach (BasicDeliverEventArgs e in _subscription)
			{
				string message = Encoding.UTF8.GetString(e.Body);

				T deserializedEntity = JsonConvert.DeserializeObject<T>(message);

				_items.Add(deserializedEntity);

				Console.WriteLine(message);

				_receivedMessages.Add(e);

				messagesProcessed++;

				if (messagesProcessed == response.MessageCount)
				{
					break;
				}

			}
			Console.WriteLine("Subscription Done");

			return _items;

		}

		/// <summary>
		/// Commit Messages
		/// </summary>
		/// <param name="queueName"></param>
		public void SendAcknowledgement()
		{
			//_channel.BasicConsume(queueName, true, _consumer);
			foreach (BasicDeliverEventArgs e in _receivedMessages)
			{
				_subscription.Ack(e);
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
