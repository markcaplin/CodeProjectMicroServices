using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.Extensions.PlatformAbstractions;
using DotNetCore.WindowsServices;

namespace CodeProject.InventoryManagement.MessageQueueing
{
	class Program
	{
		public static async Task Main(string[] args)
		{

			var basePath = PlatformServices.Default.Application.ApplicationBasePath;
			if (basePath.ToLower().Contains("inventorymanagementqa"))
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

		}
	}
}
