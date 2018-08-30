using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServicesTest.Models
{
    public class StockPart
    {
		public int StockPartId { get; set; }
		public string ProductNumber { get; set; }
		public int QuantityOnHand { get; set; }
		public int QuantityOnOrder { get; set; }
    }
}
