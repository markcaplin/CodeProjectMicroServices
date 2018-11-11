using System;
using System.Collections.Generic;
using System.Text;

namespace CodeProject.Shared.Common.Models
{
  
	public static class MessageExchangeFanouts
	{
		public static int ProductUpdated = 2;
		public static int PurchaseOrderSubmitted = 1;
		public static int InventoryReceived = 2;
	}
}
