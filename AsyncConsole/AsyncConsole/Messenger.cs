using System;
using System.Threading;

namespace AsyncConsole
{
    public class Messenger
    {
        private string _msg;

        public Messenger(string message)
        {
            _msg = message;
        }

        public void DisplayMessenger()
        {
            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] {_msg}");

        }
    }
}