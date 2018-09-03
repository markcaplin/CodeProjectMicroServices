using CodeProject.Shared.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeProject.Shared.Common.Interfaces
{
    public interface IMessageQueueProcessing
    {
		Task<ResponseModel<MessageQueue>> CommitInboundMessage(MessageQueue messageQueue);
		Task<ResponseModel<List<MessageQueue>>> SendQueueMessages(IMessageQueueing messageQueueing);
		Task<ResponseModel<List<MessageQueue>>> ProcessMessages();


	}
}
