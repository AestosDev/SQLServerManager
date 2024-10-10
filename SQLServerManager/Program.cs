using System;
using System.ServiceProcess;

namespace SQLServerManager
{
    class Programm
    {
        static void Main(string[] args)
        {
            string[] serviceNames = { "MSSQLSERVER", "SQLSERVERAGENT", "SQLBrowser" };

            Console.WriteLine("Would you like to start or stop all services? (start/stop): ");
            string action = Console.ReadLine()?.ToLower();

            foreach (var serviceName in serviceNames)
            {
                ServiceController service = new ServiceController(serviceName);

                if (action == "start")
                {
                    StartService(service);
                }
                else if (action == "stop")
                {
                    StopService(service);
                }
                else
                {
                    Console.WriteLine("Invalid action. Please type 'start' or 'stop'.");
                    return; 
                }
            }

            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }

        static void StartService(ServiceController service)
        {
            try
            {
                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    Console.WriteLine($"\nTrying to start {service.DisplayName}...");
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running);
                    Console.WriteLine($"{service.DisplayName} has been started successfully.");
                }
                else
                {
                    Console.WriteLine($"{service.DisplayName} is already running.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting {service.DisplayName}: {ex.Message}");
            }
        }

        static void StopService(ServiceController service)
        {
            try
            {
                if (service.Status == ServiceControllerStatus.Running)
                {
                    Console.WriteLine($"\nTrying to stop {service.DisplayName}...");
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);
                    Console.WriteLine($"{service.DisplayName} has been stopped successfully.");
                }
                else if (service.DisplayName == "SQL Server-Agent (MSSQLSERVER)" && service.Status == ServiceControllerStatus.Stopped)
                {
                    Console.WriteLine("SQL Server-Agent is already stopped.");
                }
                else
                {
                    Console.WriteLine($"{service.DisplayName} is already stopped.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error stopping {service.DisplayName}: {ex.Message}");
            }
        }
    }
}
