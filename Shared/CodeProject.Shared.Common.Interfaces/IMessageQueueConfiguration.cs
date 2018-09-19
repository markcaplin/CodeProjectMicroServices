using CodeProject.Shared.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeProject.Shared.Common.Interfaces
{

	public interface IMessageQueueConfiguration
	{
		void AddQueue(string queueName);
		void InitializeMessageQueueing();
		ResponseModel<MessageQueue> SendMessage(MessageQueue entity);

		string TransactionCode { get; set; }
		

	}
}
