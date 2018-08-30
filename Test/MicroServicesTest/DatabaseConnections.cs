using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServicesTest
{
    public static class DatabaseConnections
    {
		public static string StockDatabaseConnection { get; } = "Data Source=JOEY\\SQLEXPRESS;Database=Test;Trusted_Connection=True";
		public static string ProductDatabaseConnection { get; } = "Data Source=JOEY\\SQLEXPRESS;Database=Test;Trusted_Connection=True";

	}
}
