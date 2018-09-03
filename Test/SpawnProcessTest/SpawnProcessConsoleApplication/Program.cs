using System;
using System.Diagnostics;

namespace SpawnProcessConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {

			/*process1.OutputDataReceived += (sender, data) => {
				Console.WriteLine(data.Data);
			};

			process1.ErrorDataReceived += (sender, data) => {
				Console.WriteLine(data.Data);
			};*/

			Console.WriteLine("Hello World!");
			Process process1 = new Process();
			process1.StartInfo.CreateNoWindow = false;
			process1.StartInfo.UseShellExecute = false;
			process1.StartInfo.RedirectStandardOutput = false;
			process1.StartInfo.FileName = @"C:\MyFiles\_CodeProjectMicroServices\Support\StartAccountManagementWebApi.bat";
			process1.Start();

			Process process2 = new Process();
			process2.StartInfo.CreateNoWindow = false;
			process2.StartInfo.UseShellExecute = false;
			process2.StartInfo.RedirectStandardOutput = false;
			process2.StartInfo.FileName = @"C:\MyFiles\_CodeProjectMicroServices\Support\StartInventoryManagementWebApi.bat";
			process2.Start();

			Process process3 = new Process();
			process3.StartInfo.CreateNoWindow = false;
			process3.StartInfo.UseShellExecute = false;
			process3.StartInfo.RedirectStandardOutput = false;
			process3.StartInfo.FileName = @"C:\MyFiles\_CodeProjectMicroServices\Support\StartSalesOrderManagementWebApi.bat";
			process3.Start();

			process3.WaitForExit();

		}
    }
}
