using CodeProject.Shared.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeProject.MessageQueueing
{
    public class MessageQueueConnection  : IMessageQueueConnection
    {
		private int _counter = 0;
		private readonly object _sendingLock = new object();

		public MessageQueueConnection()
		{
			_counter = 1;
		}

		public void IncrementCounter(string queueName)
		{
			lock (_sendingLock)
			{
				_counter++;
				Console.WriteLine("Counter updated by " + queueName + " = " + _counter);
			}
		}
    }
}
