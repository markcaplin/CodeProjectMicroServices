using System;
using System.Collections.Generic;
using System.Text;

namespace LoggingManagement.Data.Models.Entities
{
    public class MessageQueueLogging
    {
		public int MessageQueueLoggingId { get; set; }
		public int SenderTransactionQueueId { get; set; }
		public string TransactionCode { get; set; }
		public string ExchangeName { get; set; }
		public string Payload { get; set; }
		public DateTime DateCreated { get; set; }
		public int AcknowledgementsRequired { get; set; }
		public int AcknowledgementsReceived { get; set; }
	}
}
