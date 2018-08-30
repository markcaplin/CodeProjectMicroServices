using System;
using System.Collections.Generic;
using System.Text;
using CodeProject.MessageQueueing;
using MicroServicesTest.Models;


namespace MicroServicesTest.Functions
{
    public class TestMessageQueueing
    {
		public void TestSend()
		{
			

			MessageQueueing<ProductPart> queueing = new MessageQueueing<ProductPart>();

			ProductPart product = new ProductPart();
			product.Description = "This is a test 1";
			product.ProductNumber = "CAPLIN-01";
			product.ProductPartId = 1;
			product.UnitPrice = 50.00M;
			queueing.SendMessage("ProductExchange", "Product", product);

			product = new ProductPart();
			product.Description = "This is a test 2";
			product.ProductNumber = "CAPLIN-02";
			product.ProductPartId = 2;
			product.UnitPrice = 75.00M;
			queueing.SendMessage("ProductExchange", "Product", product);


		}

		public void TestReceive()
		{
			List<ProductPart> parts = new List<ProductPart>();

			MessageQueueing<ProductPart> queueing = new MessageQueueing<ProductPart>();
			parts = queueing.ReceiveMessages("ProductQueue");

			Console.WriteLine("number of items = " + parts.Count);
			queueing.SendAcknowledgement();


		}

	}
}
