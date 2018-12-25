using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using CodeProject.PurchaseOrderManagement.Data.EntityFramework;
using CodeProject.Shared.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using CodeProject.PurchaseOrderManagement.Interfaces;
using CodeProject.Shared.Common.Interfaces;
using CodeProject.PurchaseOrderManagement.Business.MessageService;
using CodeProject.MessageQueueing;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.PlatformAbstractions;
using DotNetCore.WindowsServices;

namespace CodeProject.PurchaseOrderManagement.MessageQueueing
{
	class Program
	{
		public static async Task Main(string[] args)
		{
			
			var basePath = PlatformServices.Default.Application.ApplicationBasePath;
			if (basePath.ToLower().Contains("purchaseordermanagementqa"))
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

				IHostedService sendPurchaseOrderManagementMessages = startUpConfiguration.SendingHostedService();
				IHostedService receivePurchaseOrderManagementMessages = startUpConfiguration.ReceivingHostedService();
				IHostedService processMessages = startUpConfiguration.ProcessMessagesHostedService();

				var builder = new HostBuilder().ConfigureAppConfiguration((hostingContext, config) => { })
					.ConfigureServices((hostContext, services) =>
					{
						services.AddTransient<IHostedService>(provider => processMessages);
					})
					.ConfigureServices((hostContext, services) =>
					{
						services.AddTransient<IHostedService>(provider => sendPurchaseOrderManagementMessages);
					})
					.ConfigureServices((hostContext, services) =>
					{
						services.AddTransient<IHostedService>(provider => receivePurchaseOrderManagementMessages);
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
