using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BitmapTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var threads = new List<Thread>();
            for (int i = 0; i < 100; i++)
            {
                var thread = new Thread(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        using (var bitmap = new Bitmap(1024, 768))
                        {

                        }
                    }
                });
                thread.Start();
                threads.Add(thread);
            }
            threads.ForEach(x => x.Join());
        }
    }
}
