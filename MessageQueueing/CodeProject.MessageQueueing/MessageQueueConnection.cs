using CodeProject.Shared.Common.Interfaces;
using CodeProject.Shared.Common.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeProject.MessageQueueing
{
    public class MessageQueueConnection  : IMessageQueueConnection
    {
		//private int _counter = 0;
		//private readonly object _sendingLock = new object();

		private ConnectionFactory _connectionFactory;
		private MessageQueueAppConfig _messageQueueAppConfig;
		private IConnection _connection;

		public MessageQueueConnection(MessageQueueAppConfig messageQueueAppConfig)
		{
			_messageQueueAppConfig = messageQueueAppConfig;
		}

		public void CreateConnection()
		{
			_connectionFactory = new ConnectionFactory();

			_connectionFactory.HostName = _messageQueueAppConfig.MessageQueueHostName;
			_connectionFactory.UserName = _messageQueueAppConfig.MessageQueueUserName;
			_connectionFactory.Password = _messageQueueAppConfig.MessageQueuePassword;

			_connection = _connectionFactory.CreateConnection();

			//_channel = _connection.CreateModel();

			//_basicProperties = _channel.CreateBasicProperties();
			//_basicProperties.Persistent = true;
		}

		public IConnection GetConnection()
		{
			return _connection;
		}

		//public void IncrementCounter(string queueName)
		//{
		//	lock (_sendingLock)
		//	{
		//		_counter++;
		//		Console.WriteLine("Counter updated by " + queueName + " = " + _counter);
		//	}
		//}
    }
}
