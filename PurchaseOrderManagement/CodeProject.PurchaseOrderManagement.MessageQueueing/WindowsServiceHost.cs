using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CodeProject.PurchaseOrderManagement.MessageQueueing
{
	public class WindowsServiceHost : IHostedService, IDisposable
	{
		private const string Path = @"c:\myfiles\PurchaseOrderQueue.txt";

		private Timer _timer;
		private Boolean _running;

		public Task StartAsync(CancellationToken cancellationToken)
		{

			try
			{
				using (var sw = File.AppendText(Path))
				{
					sw.WriteLine("StartAsync");
				}

				_running = false;

				_timer = new Timer((e) => StartTasks(), null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

			}
			catch (Exception ex)
			{
				string errorMessage = ex.Message;
				using (var sw = File.AppendText(Path))
				{
					if (ex.InnerException != null)
					{
						sw.WriteLine(errorMessage + ex.InnerException.ToString());
					}
					else
					{
						sw.WriteLine(errorMessage);
					}

				}

			}

			return Task.CompletedTask;
		}

		/// <summary>
		/// Start Tasks
		/// </summary>
		public void StartTasks()
		{
			if (_running) return;

			_running = true;

			try
			{
				using (var sw = File.AppendText(Path))
				{
					sw.WriteLine("Start Tasks ");
				}

				StartUpConfiguration startUpConfiguration = new StartUpConfiguration();
				startUpConfiguration.Startup();

				IHostedService sendInventoryManagementMessages = startUpConfiguration.SendingHostedService();
				IHostedService receiveInventoryManagementMessages = startUpConfiguration.ReceivingHostedService();
				IHostedService processMessages = startUpConfiguration.ProcessMessagesHostedService();

				var builder = new HostBuilder().ConfigureAppConfiguration((hostingContext, config) => { })
					.ConfigureServices((hostContext, services) =>
					{
						services.AddTransient<IHostedService>(provider => processMessages);
					})
					.ConfigureServices((hostContext, services) =>
					{
						services.AddTransient<IHostedService>(provider => sendInventoryManagementMessages);
					})
					.ConfigureServices((hostContext, services) =>
					{
						services.AddTransient<IHostedService>(provider => receiveInventoryManagementMessages);
					})
					.ConfigureLogging((hostingContext, logging) =>
					{
						logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
						logging.AddConsole();
					});

				builder.RunConsoleAsync().Wait();
			}
			catch (Exception ex)
			{
				string errorMessage = ex.Message;
				using (var sw = File.AppendText(Path))
				{
					if (ex.InnerException != null)
					{
						sw.WriteLine(errorMessage + ex.InnerException.ToString());
					}
					else
					{
						sw.WriteLine(errorMessage);
					}

				}

			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public Task StopAsync(CancellationToken cancellationToken)
		{
			_timer?.Change(Timeout.Infinite, 0);

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}

	}
}