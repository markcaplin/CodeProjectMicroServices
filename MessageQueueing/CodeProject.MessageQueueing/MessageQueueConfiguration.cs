using CodeProject.Shared.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeProject.MessageQueueing
{
    public class MessageQueueConfiguration : IMessageQueueConfiguration
    {
		private readonly string _messageQueueName;

		public MessageQueueConfiguration(string messageQueueName)
		{
			_messageQueueName = messageQueueName;
		}

		public string GetMessageQueueName()
		{
			return _messageQueueName;
		}
	}

}
