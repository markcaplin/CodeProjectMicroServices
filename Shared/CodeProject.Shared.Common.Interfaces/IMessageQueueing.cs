using System;
using System.Collections.Generic;
using System.Text;

namespace CodeProject.Shared.Common.Interfaces
{
    public interface IMessageQueueing<T>
    {
		void SendMessage(string exchangeName, string routingKey, T entity);
		List<T> ReceiveMessages(string queueName);
		void SendAcknowledgement();
	}
}
