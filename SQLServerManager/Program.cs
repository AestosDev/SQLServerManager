using System;
using System.ServiceProcess;

namespace SQLServerManager
{
    class Programm
    {
        static void Main(string[] args)
        {
            string serviceName = "MSSQLSERVER";

            ServiceController sqlService = new ServiceController(serviceName);

            try
            {
                if (sqlService.Status == ServiceControllerStatus.Running)
                {
                    Console.WriteLine("SQL Server is already running. Would you like to stop it? (y/n)");
                    if (Console.ReadKey().Key == ConsoleKey.Y)
                    {
                        StopService(sqlService);
                    }
                }
                else if (sqlService.Status == ServiceControllerStatus.Stopped)
                {
                    Console.WriteLine("SQL Server is not running. Would you like to start it? (y/n)");
                    if (Console.ReadKey().Key == ConsoleKey.Y)
                    {
                        StartService(sqlService);
                    }
                }
                else
                {
                    Console.WriteLine($"Current Status: {sqlService.Status}");
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            // Prevent the program from closing immediately after performing the action
            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }

        static void StartService(ServiceController service)
        {
            try
            {
                Console.WriteLine("\nTrying to start SQL Server Service...");
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running);
                Console.WriteLine("SQL Server Service has been started successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an error while trying to start the SQL Server Service. Error: {ex.Message}");
            }
        }

        static void StopService(ServiceController service)
        {
            try
            {
                Console.WriteLine("\nTrying to stop SQL Server Service...");
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped);
                Console.WriteLine("SQL Server Service has been stopped successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"There was an error while trying to stop the SQL Server Service. Error: {ex.Message}");
            }
        }
    }
}
