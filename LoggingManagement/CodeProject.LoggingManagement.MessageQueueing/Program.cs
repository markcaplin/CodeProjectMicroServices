using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.PlatformAbstractions;

namespace CodeProject.LoggingManagement.MessageQueueing
{
	class Program
	{

		public static async Task Main(string[] args)
		{
		
			var basePath = PlatformServices.Default.Application.ApplicationBasePath;
			if (basePath.ToLower().Contains("loggingmanagementqa"))
			{
			
				var builderRunAsService = new HostBuilder()
				.ConfigureServices((hostContext, services) =>
				{
					services.AddHostedService<WindowsServiceHost>();
				});

				await builderRunAsService.RunAsServiceAsync();
			}
			else
			{

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

				await builder.RunConsoleAsync();
			}


		}
	}
}
