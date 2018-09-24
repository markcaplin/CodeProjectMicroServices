using System;
using System.Collections.Generic;
using System.Text;

namespace CodeProject.Shared.Common.Models
{
    public static class MessageQueueExchanges
    {
		public static string PurchaseOrderManagement = "PurchaseOrderManagement";
		public static string InventoryManagement = "InventoryManagement";
		public static string SalesOrderManagement = "SalesOrderManagement";
		public static string ProductUpdated = "ProductUpdated";
		public static string OrderShipped = "OrderShipped";
		public static string Logging = "Logging";
	}
}
