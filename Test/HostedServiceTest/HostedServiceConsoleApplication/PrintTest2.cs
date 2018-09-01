using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HostedServiceConsoleApplication
{
	public class PrintTest2 : IHostedService, IDisposable
	{
		private readonly ILogger _logger;
		private readonly IOptions<AppConfig> _appConfig;
		private Timer _timer;

		public PrintTest2(ILogger<PrintTest> logger, IOptions<AppConfig> appConfig)
		{
			_logger = logger;
			_appConfig = appConfig;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			//_logger.LogInformation("Starting");

			_timer = new Timer(DoWork, null, TimeSpan.Zero,
				TimeSpan.FromSeconds(2));

			return Task.CompletedTask;
		}

		private void DoWork(object state)
		{
			//_logger.LogInformation($"Background work with text 2: {_appConfig.Value.TextToPrint}");
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
