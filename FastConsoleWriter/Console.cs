using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace FastConsoleWriter
{
    public class Console : IDisposable
    {
        public Thread Thread { get; private set; }
        public bool Disposed { get; private set; }
        public TextWriter TextWriter { get; private set; }
        private List<Line> LinesQueue;
        private bool DisposeOnEnd;
        public int Count { get { return LinesQueue.Count; } }
        //private ManualResetEvent waiter;
        public Console(TextWriter tw)
        {
            TextWriter = tw;
            //waiter = new ManualResetEvent(false);
        }

        public void Write(object obj)
        {
            LinesQueue.Add(new Line(obj.ToString()));
        }
        public void WriteLine(object obj)
        {
            LinesQueue.Add(new Line(obj.ToString(), true));
        }

        
        public void Start(bool disposeonend = false)
        {
            DisposeOnEnd = disposeonend;
            LinesQueue = new List<Line>();
            Disposed = false;
            Thread = new Thread(new ParameterizedThreadStart(ConsoleWork));
            Thread.Start(this);
        }

        public void Dispose()
        {
            Disposed = true;
            LinesQueue = null;
            Thread = null;
        }

        private void ConsoleWork(object obj)
        {
            var console = (Console)obj; // get console
            while (true)
            {
                if (DisposeOnEnd && console.LinesQueue.Count == 0)
                    Dispose();
                if (Disposed)
                {
                    console = null; // free link
                    break; // exit on disposed
                }


                var lines = console.LinesQueue;
                var co = lines.Count;
                if (co > 0)
                {
                    var first = lines[0]; // get from queue
                    var isline = first.IsLine;

                    if (isline)
                        console.TextWriter.WriteLine(first.Text); // write as line
                    else
                        console.TextWriter.Write(first.Text); // write to line

                    lines.Remove(first); // remove from queue

                    Thread.Sleep(1); // reduce cpu usage for fast
                }
                else
                {
                    Thread.Sleep(10); // reduce cpu usage for idle
                }
            }
        }
    }
}
