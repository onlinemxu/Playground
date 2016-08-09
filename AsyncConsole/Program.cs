using System;
using System.Threading;

namespace AsyncConsole
{
    internal class Program
    {
        private static volatile bool _cancel = false;

        private static Random _random = new Random();

        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(
                    $@"[{Thread.CurrentThread.ManagedThreadId}] Processor Core = {Environment.ProcessorCount}: starting...");

                DemoThread1();

                Console.WriteLine($@"[{Thread.CurrentThread.ManagedThreadId}]: Main thread is done.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
            //Console.WriteLine("Press ENTER to cancel.");
            //Console.ReadLine();

            //_cancel = true;
            //if (t != null)
            //{
            //    t.Join();
            //}

        }

        private static void DemoThread1()
        {
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(ShowMessage, i);
            }
            
            Thread.Sleep(_random.Next(200, 1000));
        }

        private static void ShowMessage(object arg)
        {
            Thread.Sleep(_random.Next(250, 500));
            var n = (int) arg;
            Console.WriteLine(
                $"[{Thread.CurrentThread.ManagedThreadId}] hello {n}! (IsBackground={Thread.CurrentThread.IsBackground})");
        }

        private static Thread DemoThread()
        {
            var t = new Thread(DisplayMessage)
            {
                Name = "Display a message",
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal
            };
            t.Start(10);

            return t;
        }

        private static void DisplayMessage(object arg)
        {
            var iteration = (int) arg;
            for (int i = 0; i < iteration; i++)
            {
                Console.WriteLine(
                    $@"[{Thread.CurrentThread.ManagedThreadId}]: Hello {i}!(IsBackground: {Thread.CurrentThread
                        .IsBackground})");
                Thread.Sleep(1000);
            }

            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Thread is stopping...");

            //throw new NotImplementedException();
        }
    }
}