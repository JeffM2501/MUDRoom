using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace MudRoom.IO.CommonInterfaces
{
    public class ConsoleIO
    {
        public List<string> PendingInputLines = new List<string>();
        private Thread ReadThread = null;
        public ConsoleIO()
        {
            ReadThread = new Thread(new ThreadStart(ServiceInput));
            ReadThread.Start();
        }

        public void Dispose()
        {
            ReadThread.Abort();
        }

        public void ServiceInput()
        {
            StringBuilder line = new StringBuilder();
            while (true)
            {
                bool newline = false;

                if (Console.KeyAvailable)
                {
                    char c = Console.ReadKey(true).KeyChar;

                    if (c == '\b')
                    {
                        if (line.Length > 0)
                        {
                            line.Remove(line.Length - 1, 1);
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                            Console.Write(' ');
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        }
                    }
                    else
                    {
                        Console.Write(c);
                        if (c == '\r' || c == '\n')
                            newline = true;
                        else
                            line.Append(c);
                    }
                }

                if (newline)
                {
                    lock (PendingInputLines)
                        PendingInputLines.Add(line.ToString());

                    line.Clear();
                    Console.WriteLine();
                }

                if (!Console.KeyAvailable)
                    Thread.Sleep(10);
            }
        }

        public virtual bool HasInputIO()
        {
            lock (PendingInputLines)
                return PendingInputLines.Count > 0;
        }

        public virtual string GetInputIOLine()
        {
            if (!HasInputIO())
                return string.Empty;
            lock (PendingInputLines)
            {
                string line = PendingInputLines[0];
                PendingInputLines.RemoveAt(0);
                return line.Trim();
            }
        }

        public virtual void OutputIOLine(string line)
        {
            Console.WriteLine(line);
        }
    }
}
