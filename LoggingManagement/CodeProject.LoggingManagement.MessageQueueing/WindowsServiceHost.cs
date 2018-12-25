using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using CodeProject.LoggingManagement.Data.EntityFramework;
using CodeProject.Shared.Common.Models;
using Microsoft.EntityFrameworkCore;
using CodeProject.LoggingManagement.Interfaces;
using CodeProject.Shared.Common.Interfaces;
using CodeProject.LoggingManagement.Business.MessageService;
using CodeProject.MessageQueueing;

namespace CodeProject.LoggingManagement.MessageQueueing
{
	public class WindowsServiceHost : IHostedService, IDisposable
	{
		private const string Path = @"c:\myfiles\LoggingQueue.txt";

		private Timer _timer;
		private Boolean _running;

		/// <summary>
		/// STart Async
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public Task StartAsync(CancellationToken cancellationToken)
		{
			using (var sw = File.AppendText(Path))
			{
				sw.WriteLine("StartAsync");
			}

			_running = false;

			_timer = new Timer((e) => StartTasks(), null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

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

				IHostedService sendLoggingManagementMessages = startUpConfiguration.SendingHostedService();
				IHostedService receiveLoggingManagementMessages = startUpConfiguration.ReceivingHostedService();

				var builder = new HostBuilder()
					.ConfigureServices((hostContext, services) =>
					{
						services.AddTransient<IHostedService>(provider => sendLoggingManagementMessages);
					})
					.ConfigureServices((hostContext, services) =>
					{
						services.AddTransient<IHostedService>(provider => receiveLoggingManagementMessages);
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