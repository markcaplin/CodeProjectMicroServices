using System;
using System.Collections.Generic;
using System.Text;

namespace CodeProject.Shared.Common.Models
{
	public class MessageQueueAppConfig
	{
		public string ExchangeName { get; set; }
		public string RoutingKey { get; set; }
		public string InboundMessageQueue { get; set; }
		public string OutboundMessageQueues { get; set; }
	}
}
