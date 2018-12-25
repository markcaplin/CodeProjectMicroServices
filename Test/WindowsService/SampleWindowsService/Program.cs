using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DotNetCore.WindowsServices;
using System.Linq;

namespace SampleWindowsService
{
	internal class Program
	{
		private static async Task Main(string[] args)
		{
			var isService = !(Debugger.IsAttached || args.Contains("--console"));

			var builder = new HostBuilder()
				.ConfigureServices((hostContext, services) =>
				{
					services.AddHostedService<FileWriterService>();
				});

			if (isService)
			{
				await builder.RunAsServiceAsync();
			}
			else
			{
				await builder.RunConsoleAsync();
			}
		}
	}
}
