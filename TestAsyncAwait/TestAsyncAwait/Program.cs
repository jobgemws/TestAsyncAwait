using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestAsyncAwait
{
    class Program
    {
        static void Main(string[] args)
        {
            //first thread
            Console.WriteLine($@"1:{Thread.CurrentThread.ManagedThreadId}");

            ToMain();

            Console.WriteLine($@"5:{Thread.CurrentThread.ManagedThreadId}");

            Console.WriteLine("Hello World!");

            Console.ReadKey();
        }

        static async Task ToMain()
        {
            //first thread
            Console.WriteLine($@"2:{Thread.CurrentThread.ManagedThreadId}");

            int a = 777;

            //continuation of another thread
            string message = await DoWorkAsync(a);

            Console.WriteLine($@"7:{Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine(message);

        }

        static async Task<string> DoWorkAsync(int a)
        {
            Console.WriteLine($@"3:{Thread.CurrentThread.ManagedThreadId}");

            //start another thread
            var res = await Task.Run(() =>
              {
                  Console.WriteLine($@"4:{Thread.CurrentThread.ManagedThreadId}");

                  StringBuilder str = new StringBuilder();
                  str.Append("Done with work!");
                  str.Append(" " + a.ToString());

                  Thread.Sleep(5_000);

                  try
                  {
                      int b =  a/0;
                      str.Append(" " + b.ToString());
                  }
                  catch (Exception exp)
                  {
                      Console.WriteLine($@"Exception:{exp.Message}");
                  }
                  finally
                  {
                      Console.WriteLine($@"finally");
                  }

                  return str.ToString();
              });

            //continuation of another thread
            Console.WriteLine($@"6:{Thread.CurrentThread.ManagedThreadId}");

            return res;
        }
    }
}
