using Serilog;
using Serilog.Events;
using System;
using System.IO;
using System.Threading;
namespace Killprocess // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = Console.ReadLine();
            Utility.utilityKill(filePath);
        }
        public class Utility
        {
            public static void utilityKill(string path)

            {
                try
                {
                    Task t = Task.Run(() =>
                    {
                        var fileLoad = new FileStream(path,
                         FileMode.Open,
                         FileAccess.Read);
                    });
                    DateTime time = DateTime.Now;
                    double totalTime = DateTime.Now.Subtract(time).TotalMinutes;

                    //totalTime = totalTime + 10;
                    TimeSpan ts = new TimeSpan(0, 0, 5, 0);
                    if (totalTime >= ts.Minutes)
                    {
                        t.Dispose();
                        Console.WriteLine("The timeout interval elapsed.");
                        Console.WriteLine("'TimeOut!' File loading time is more than 5 minutes");

                        Environment.Exit(0);
                    }


                    if (t != null && File.Exists(path))
                    {

                        if (totalTime < 5)
                        {

                            Console.WriteLine("File location and Name: " + path + "\nFile loading time is: " + totalTime + " minutes");
                        }

                    }
                    else
                    {
                        Console.WriteLine("invalid file");
                    }
                    Logging.Logger(path, totalTime, ts);
                }



                catch (FileNotFoundException e)
                {
                    Console.WriteLine("Path is incorrect or file doesn't exist");
                    Console.WriteLine("Please enter file path");
                    Console.ReadLine();
                }
                catch (IOException e)
                {
                    // Extract some information from this exception, and then
                    // throw it to the parent method.
                    if (e.Source != null)
                    {
                        Console.WriteLine("IOException source: {0}", e.Source);
                        Console.ReadLine();
                    }

                    throw;
                }
            }
        }

        public class Logging
        {
            public static void Logger(string filePath, double totalTime, TimeSpan span)
            {
                Log.Logger = (ILogger)new LoggerConfiguration()
              .MinimumLevel.Debug()
              .WriteTo.File("log.txt") // log file.
              .WriteTo.File(filePath)

              .CreateLogger();
                try
                {
                    Log.Information("Starting up " + filePath, totalTime, span);

                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Application start-up failed");
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }
        }

    }
}
