using System;
using System.Diagnostics;

namespace SpawnProcessConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
			Process process1 = new Process();
			process1.StartInfo.CreateNoWindow = false;
			process1.StartInfo.UseShellExecute = false;
			process1.StartInfo.RedirectStandardOutput = false;
			process1.StartInfo.FileName = @"C:\MyFiles\_CodeProjectMicroServices\Support\StartAccountManagementWebApi.bat";

			/*process1.OutputDataReceived += (sender, data) => {
				Console.WriteLine(data.Data);
			};

			process1.ErrorDataReceived += (sender, data) => {
				Console.WriteLine(data.Data);
			};*/

			process1.Start();

			Process process2 = new Process();
			process2.StartInfo.CreateNoWindow = false;
			process2.StartInfo.UseShellExecute = false;
			process2.StartInfo.RedirectStandardOutput = false;
			process2.StartInfo.FileName = @"C:\MyFiles\_CodeProjectMicroServices\Support\StartInventoryManagementWebApi.bat";

			/*process2.OutputDataReceived += (sender, data) => {
				Console.WriteLine(data.Data);
			};

			process2.ErrorDataReceived += (sender, data) => {
				Console.WriteLine(data.Data);
			};*/

			process2.Start();


			process2.WaitForExit();

		}
    }
}
