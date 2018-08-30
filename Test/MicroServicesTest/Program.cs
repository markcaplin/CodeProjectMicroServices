using System;

namespace MicroServicesTest
{
    class Program
    {
        static void Main(string[] args)
        {
			//Console.WriteLine("Hello World!");
			//Functions.TestTransaction testTranaction = new Functions.TestTransaction();
			//testTranaction.RunTestTransaction();

			//Functions.MessageQueue message = new Functions.MessageQueue();

			/*int counter = 0;
			while (counter < 10)
			{
				counter++;
				message.Send(counter);
			}
			//message.Send();
			message.Receive("test2");*/

			Functions.TestMessageQueueing queue = new Functions.TestMessageQueueing();
		    //queue.TestSend();
			queue.TestReceive();

		}

		

    }
}
