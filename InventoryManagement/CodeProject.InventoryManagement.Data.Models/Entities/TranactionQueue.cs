using System;
using System.Collections.Generic;
using System.Text;

namespace CodeProject.InventoryManagement.Data.Entities
{
    public class TransactionQueue
    {
		public int TransactionQueueId { get; set; }
		public int MessageQueueDirection { get; set; }
		public string TransactionCode { get; set; }
		public string Payload { get; set; }
		public string ExchangeName { get; set; }
		public Boolean SentToExchange { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateSentToExchange { get; set; }
		public DateTime DateToResendToExchange { get; set; }
		
    }
}
