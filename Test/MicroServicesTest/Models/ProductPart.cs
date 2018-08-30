using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServicesTest.Models
{
	public class ProductPart
	{ 
		public int ProductPartId { get; set; }
		public string Description { get; set; }
		public string ProductNumber { get; set; }
		public Decimal UnitPrice { get; set; }
    }
}
