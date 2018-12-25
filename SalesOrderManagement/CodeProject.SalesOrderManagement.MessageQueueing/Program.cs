using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CodeProject.SalesOrderManagement.Data.EntityFramework;
using CodeProject.Shared.Common.Models;
using Microsoft.EntityFrameworkCore;
using CodeProject.SalesOrderManagement.Interfaces;
using CodeProject.Shared.Common.Interfaces;
using CodeProject.SalesOrderManagement.Business.MessageService;
using CodeProject.MessageQueueing;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.PlatformAbstractions;
using DotNetCore.WindowsServices;

namespace CodeProject.SalesOrderManagement.MessageQueueing
{
	class Program
	{
		public static async Task Main(string[] args)
		{

			var basePath = PlatformServices.Default.Application.ApplicationBasePath;
			if (basePath.ToLower().Contains("salesordermanagementqa"))
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

				IHostedService sendSalesOrderManagementMessages = startUpConfiguration.SendingHostedService();
				IHostedService receiveSalesOrderManagementMessages = startUpConfiguration.ReceivingHostedService();
				IHostedService processMessages = startUpConfiguration.ProcessMessagesHostedService();

				var builder = new HostBuilder().ConfigureAppConfiguration((hostingContext, config) => { })
					.ConfigureServices((hostContext, services) =>
					{
						services.AddTransient<IHostedService>(provider => processMessages);
					})
					.ConfigureServices((hostContext, services) =>
					{
						services.AddTransient<IHostedService>(provider => sendSalesOrderManagementMessages);
					})
					.ConfigureServices((hostContext, services) =>
					{
						services.AddTransient<IHostedService>(provider => receiveSalesOrderManagementMessages);
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