using System;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HostedServiceConsoleApplication
{
	public class PrintTest : IHostedService, IDisposable
	{
		private readonly ILogger _logger;
		private readonly IOptions<AppConfig> _appConfig;
		private Timer _timer;
		private int _counter;

		private Subject<MessageQueue> _subject;

		public PrintTest(ILogger<PrintTest> logger, IOptions<AppConfig> appConfig)
		{
			_logger = logger;
			_appConfig = appConfig;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Starting");

			_counter = 0;

			_subject = new Subject<MessageQueue>();
			_subject.Subscribe(ReceiveMessage1);
			_subject.Subscribe(ReceiveMessage2);

			_timer = new Timer(DoWork, null, TimeSpan.Zero,
				TimeSpan.FromSeconds(2));

			return Task.CompletedTask;
		}

		private void DoWork(object state)
		{
			//_logger.LogInformation($"Background work with text 1: {_appConfig.Value.TextToPrint}");

			_counter++;

			_logger.LogInformation("send = " + _counter.ToString());

			MessageQueue messageQueue = new MessageQueue();
			messageQueue.MessageId = Guid.NewGuid();
			messageQueue.MessageText = "message " + _counter.ToString();
			_subject.OnNext(messageQueue);

		}

		public void ReceiveMessage1(MessageQueue messageQueue)
		{
			//_logger.LogInformation($"Receive Message 1: {messageQueue.MessageText}");
			Task.Delay(10000).Wait();
		}

		public void ReceiveMessage2(MessageQueue messageQueue)
		{
			_logger.LogInformation($"Receive Message 2: {messageQueue.MessageText}");
			Task.Delay(20000).Wait();

		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Stopping.");

			_timer?.Change(Timeout.Infinite, 0);

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}
	}
}
