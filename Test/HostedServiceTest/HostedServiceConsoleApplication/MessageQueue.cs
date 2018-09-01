using System;
using System.Collections.Generic;
using System.Text;

namespace HostedServiceConsoleApplication
{
    public class MessageQueue
    {
		public Guid MessageId { get; set; }
		public string MessageText { get; set; }
    }
}
