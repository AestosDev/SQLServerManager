using System;
using System.ServiceProcess;

namespace SQLServerManager
{
    class Programm
    {
        static void Main(string[] args)
        {
            Console.Title = "SQL Server Manager";
            SetColour(4);
            Console.WriteLine("================================");
            Console.WriteLine("= SQL Server Manager by Aestos =");
            Console.WriteLine("================================\n");
            string[] serviceNames = { "MSSQLSERVER", "SQLSERVERAGENT", "SQLBrowser" };

            SetColour(0);
            Console.WriteLine("Current status of services:");
            foreach (var serviceName in serviceNames)
            {
                ServiceController service = new ServiceController(serviceName);
                switch (service.Status)
                {
                    case ServiceControllerStatus.Running:
                        SetColour(2);
                        Console.WriteLine($"{service.DisplayName} is currently: {service.Status}");
                        break;

                    case ServiceControllerStatus.Stopped:
                        SetColour(1);
                        Console.WriteLine($"{service.DisplayName} is currently: {service.Status}");
                        break;
                }
                SetColour(0);
            }

            Console.WriteLine("\nWould you like to start or stop all services? (start/stop): ");
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
                    SetColour(3);
                    Console.WriteLine("Invalid action. Please type 'start' or 'stop'.");
                    return;
                }
            }

            SetColour(0);
            Console.WriteLine("\nPress Enter to exit...");
            Console.ReadLine();
        }

        static void StartService(ServiceController service)
        {
            try
            {
                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    SetColour(1);
                    Console.WriteLine($"\nTrying to start {service.DisplayName}...");
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running);
                    SetColour(2);
                    Console.WriteLine($"{service.DisplayName} has been started successfully.");
                }
                else
                {
                    SetColour(1);
                    Console.WriteLine($"{service.DisplayName} is already running.");
                }
            }
            catch (Exception ex)
            {
                SetColour(3);
                Console.WriteLine($"Error starting {service.DisplayName}: {ex.Message}");
            }
        }

        static void StopService(ServiceController service)
        {
            try
            {
                if (service.Status == ServiceControllerStatus.Running)
                {
                    SetColour(1);
                    Console.WriteLine($"\nTrying to stop {service.DisplayName}...");
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped);
                    SetColour(2);
                    Console.WriteLine($"{service.DisplayName} has been stopped successfully.");
                }
                else if (service.DisplayName == "SQL Server-Agent (MSSQLSERVER)" && service.Status == ServiceControllerStatus.Stopped)
                {
                    Console.WriteLine("SQL Server-Agent is already stopped.");
                }
                else
                {
                    SetColour(1);
                    Console.WriteLine($"{service.DisplayName} is already stopped.");
                }
            }
            catch (Exception ex)
            {
                SetColour(3);
                Console.WriteLine($"Error stopping {service.DisplayName}: {ex.Message}");
            }
        }

        static void SetColour(byte type)
        {
            switch (type)
            {
                // White
                case 0:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                // Yellow
                case 1:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                // Green
                case 2:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                // Red
                case 3:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                //Blue
                case 4:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;

            }
        }
    }
}
