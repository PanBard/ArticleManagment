using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleManage
{
    public class Spinner : IDisposable
    {
        private const string Sequence = @"/-\|";
        private int counter = 0;
        private readonly int left;
        private readonly int top;
        private readonly int delay;
        private bool active;
        private readonly Thread thread;
        private DateTime start;
        public DateTime totalTimeStart;
        private int text_width;
        private string ObjectName;
        public Spinner()
        {
            this.totalTimeStart = DateTime.Now;
            this.left = 0;
            this.delay = 200;
            thread = new Thread(Spin);
        }

        public void Start(string ObjectName)
        {
            this.ObjectName = ObjectName;   
            start = DateTime.Now;
            active = true;
            if (!thread.IsAlive)
                thread.Start();
        }

        public void Stop()
        {
            active = false;
            if(Console.CursorLeft != 0)
            {
                Console.WriteLine();
            }
            
        }

        private void Spin()
        {
            while (active)
            {
                Turn();
                Thread.Sleep(delay);

            }
        }

        private void Draw(char c)
        {
            if(Console.CursorLeft == 0 || Console.CursorLeft < text_width)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
            }
            else
            {
                Console.SetCursorPosition(Console.CursorLeft - text_width, Console.CursorTop);
            }

            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.Write(c);
            var t= DateTime.Now - start ;
            string text = $"{ObjectName} running time: {String.Format("{0:0.00}", t.TotalSeconds)} s  ";
            Console.Write(text);
            text_width = text.Length;
        }

        private void Turn()
        {
            Draw(Sequence[++counter % Sequence.Length]);
        }

        public void Dispose()
        {
            Stop();
            TimeSpan ts = (DateTime.Now - this.totalTimeStart);
            Console.WriteLine("Elapsed Time for whole program is {0} s", ts.TotalSeconds);
        }
    }
}
