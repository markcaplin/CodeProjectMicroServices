using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SpawnProcesses
{
    class Program
    {
		public IConfiguration Configuration { get; }

		static void Main(string[] args)
        {
			StartUpProcesses startUpProcesses = new StartUpProcesses();

			string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			string jsonFile = $"appsettings.{environment}.json";

			var builder = new ConfigurationBuilder()
			  .SetBasePath(Directory.GetCurrentDirectory())
			  .AddJsonFile(jsonFile, optional: true, reloadOnChange: true);

			IConfigurationRoot configuration = builder.Build();

			configuration.GetSection("StartUpProcesses").Bind(startUpProcesses);

			if (startUpProcesses.AccountManagementWebApi==true)
			{
				Console.WriteLine("Starting Accoubt Management Web Api");
				Process process1 = new Process();
				process1.StartInfo.CreateNoWindow = false;
				process1.StartInfo.UseShellExecute = false;
				process1.StartInfo.RedirectStandardOutput = false;
				process1.StartInfo.FileName = @"C:\MyFiles\_CodeProjectMicroServices\Support\StartAccountManagementWebApi.bat";
				process1.Start();
			}

			if (startUpProcesses.InventoryManagementWebApi == true)
			{
				Console.WriteLine("Starting Inventory Management Web Api");
				Process process2 = new Process();
				process2.StartInfo.CreateNoWindow = false;
				process2.StartInfo.UseShellExecute = false;
				process2.StartInfo.RedirectStandardOutput = false;
				process2.StartInfo.FileName = @"C:\MyFiles\_CodeProjectMicroServices\Support\StartInventoryManagementWebApi.bat";
				process2.Start();

			}

			if (startUpProcesses.SalesOrderManagementWebApi == true)
			{
				Console.WriteLine("Starting Sales Order Management Web Api");
				Process process3 = new Process();
				process3.StartInfo.CreateNoWindow = false;
				process3.StartInfo.UseShellExecute = false;
				process3.StartInfo.RedirectStandardOutput = false;
				process3.StartInfo.FileName = @"C:\MyFiles\_CodeProjectMicroServices\Support\StartSalesOrderManagementWebApi.bat";
				process3.Start();
			}

			if (startUpProcesses.PurchaseOrderManagementWebApi == true)
			{
				Console.WriteLine("Starting Purchase Order Management Web Api");
				Process process3 = new Process();
				process3.StartInfo.CreateNoWindow = false;
				process3.StartInfo.UseShellExecute = false;
				process3.StartInfo.RedirectStandardOutput = false;
				process3.StartInfo.FileName = @"C:\MyFiles\_CodeProjectMicroServices\Support\StartPurchaseOrderManagementWebApi.bat";
				process3.Start();
			}

			if (startUpProcesses.InventoryManagementMessageQueue == true)
			{
				Console.WriteLine("Starting Inventory Management Message Queue");

				Process process4 = new Process();
				process4.StartInfo.CreateNoWindow = false;
				process4.StartInfo.UseShellExecute = false;
				process4.StartInfo.RedirectStandardOutput = false;
				process4.StartInfo.FileName = @"C:\MyFiles\_CodeProjectMicroServices\Support\StartInventoryManagementMessageQueue.bat";
				process4.Start();
			}

			if (startUpProcesses.SalesOrderManagementMessageQueue == true)
			{
				Console.WriteLine("Starting Sales Order Management Message Queue");

				Process process5 = new Process();
				process5.StartInfo.CreateNoWindow = false;
				process5.StartInfo.UseShellExecute = false;
				process5.StartInfo.RedirectStandardOutput = false;
				process5.StartInfo.FileName = @"C:\MyFiles\_CodeProjectMicroServices\Support\StartSalesOrderManagementMessageQueue.bat";
				process5.Start();
			}


			if (startUpProcesses.PurchaseOrderManagementMessageQueue == true)
			{
				Console.WriteLine("Starting Purchase Order Management Message Queue");

				Process process5 = new Process();
				process5.StartInfo.CreateNoWindow = false;
				process5.StartInfo.UseShellExecute = false;
				process5.StartInfo.RedirectStandardOutput = false;
				process5.StartInfo.FileName = @"C:\MyFiles\_CodeProjectMicroServices\Support\StartPurchaseOrderManagementMessageQueue.bat";
				process5.Start();
			}



			if (startUpProcesses.PurchaseOrderManagementMessageQueue == true)
			{
				Console.WriteLine("Starting Purchase Order Management Message Queue");

				Process process5 = new Process();
				process5.StartInfo.CreateNoWindow = false;
				process5.StartInfo.UseShellExecute = false;
				process5.StartInfo.RedirectStandardOutput = false;
				process5.StartInfo.FileName = @"C:\MyFiles\_CodeProjectMicroServices\Support\StartPurchaseOrderManagementMessageQueue.bat";
				process5.Start();
			}

			if (startUpProcesses.LoggingManagementMessageQueue == true)
			{
				Console.WriteLine("Starting Logging Management Message Queue");

				Process process6 = new Process();
				process6.StartInfo.CreateNoWindow = false;
				process6.StartInfo.UseShellExecute = false;
				process6.StartInfo.RedirectStandardOutput = false;
				process6.StartInfo.FileName = @"C:\MyFiles\_CodeProjectMicroServices\Support\StartLoggingManagementMessageQueue.bat";
				process6.Start();
			}

			Console.ReadKey();

		}
    }
}
