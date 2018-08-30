using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroServicesTest.Functions
{
    public class MessageQueue
    {
		public void Send(int counter)
		{
			ConnectionFactory connectionFactory = new ConnectionFactory();
			connectionFactory.HostName = "localhost";
			connectionFactory.UserName = "guest";
			connectionFactory.Password = "guest";

			IConnection connection = connectionFactory.CreateConnection();
			IModel model = connection.CreateModel();

			IBasicProperties basicProperties = model.CreateBasicProperties();
			basicProperties.Persistent = true;

			byte[] payload = Encoding.UTF8.GetBytes("This is a message from Visual Studio " + counter.ToString());

			PublicationAddress address = new PublicationAddress(ExchangeType.Fanout, "test2", "routingkey");

			model.BasicPublish(address, basicProperties, payload);



		}

		public void Receive(string queueName)
		{

			int counter = 0;

			var factory = new ConnectionFactory()
			{
				HostName = "localhost",
				UserName = "guest",
				Password = "guest"
			};
			using (var connection = factory.CreateConnection())
			using (var channel = connection.CreateModel())
			{
				channel.QueueDeclare(queue: queueName,
										durable: true,
										exclusive: false,
										autoDelete: false,
										arguments: null);

				var consumer = new EventingBasicConsumer(channel);
				consumer.Received += (model, ea) =>
				{
					counter++;
					var body = ea.Body;
					var message = Encoding.UTF8.GetString(body);
					Console.WriteLine("message {0} ", message);
					
					channel.BasicAck(ea.DeliveryTag, true);
					
					
				};
				channel.BasicConsume(queueName, false, consumer);

				Console.WriteLine("Done.");
				Console.ReadLine();
				

			}
			/*var factory = new ConnectionFactory() { HostName = "localhost" };
			using (var connection = factory.CreateConnection())
			{
				using (var channel = connection.CreateModel())
				{
					channel.QueueDeclare("test1", false, false, false, null);

					var consumer = new EventingBasicConsumer(channel);

					channel.BasicConsume("test1", true, consumer);

					Console.WriteLine(" [*] Waiting for messages." +
											 "To exit press CTRL+C");
					while (true)
					{
						var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

						var body = ea.Body;
						var message = Encoding.UTF8.GetString(body);
						Console.WriteLine(" [x] Received {0}", message);
					}
				}
			}*/


			/*ConnectionFactory connectionFactory = new ConnectionFactory();
			connectionFactory.HostName = "localhost";
			connectionFactory.UserName = "guest";
			connectionFactory.Password = "guest";

			IConnection connection = connectionFactory.CreateConnection();
			IModel channel = connection.CreateModel();
			channel.QueueDeclare(queue: "test1",
									 durable: true,
									 exclusive: false,
									 autoDelete: false,
									 arguments: null);

			var consumer = new EventingBasicConsumer(channel);
			channel.BasicConsume("test1", true, consumer);

			BasicDeliverEventArgs ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
			var body = ea.Body;
			var message = Encoding.UTF8.GetString(body);

			//var data = channel.BasicGet("test1", true);
			//var message = System.Text.Encoding.UTF8.GetString(data.Body);
			//channel.BasicConsume(queue: "test1", autoAck: true);




			/*consumer.Received += (model, ea) =>
			{
				var body = ea.Body;
				var message = Encoding.UTF8.GetString(body);
				Console.WriteLine(" [x] Received {0}", message);
			};
			channel.BasicConsume(queue: "test1",
								 autoAck: true,
								 consumer: consumer);

			/*ConnectionFactory connectionFactory = new ConnectionFactory();
			connectionFactory.HostName = "localhost";
			connectionFactory.UserName = "guest";
			connectionFactory.Password = "guest";

			IConnection connection = connectionFactory.CreateConnection();
			IModel model = connection.CreateModel();

			var consumer = new EventingBasicConsumer(model);

			consumer.Received += (model, ea) =>
			{
				var body = ea.Body;
				var message = Encoding.UTF8.GetString(body);
				Console.WriteLine(" [x] Received {0}", message);
			};

			model.BasicConsume(queue: "test1",
							   autoAck: true,
							   consumer: consumer);

			model.BasicQos(0, 1, false);
			QueueingBasicConsumer consumer = new QueueingBasicConsumer(model);
			model.BasicConsume(CommonService.SerialisationQueueName, false, consumer);

			BasicDeliverEventArgs deliveryArguments = consumer.Queue.Dequeue() as BasicDeliverEventArgs;
			String jsonified = Encoding.UTF8.GetString(deliveryArguments.Body);
			Customer customer = JsonConvert.DeserializeObject<Customer>(jsonified);
			Console.WriteLine("Pure json: {0}", jsonified);
			Console.WriteLine("Customer name: {0}", customer.Name);
			model.BasicAck(deliveryArguments.DeliveryTag, false);*/

		}

	}
}
