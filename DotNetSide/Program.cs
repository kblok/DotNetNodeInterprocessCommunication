using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace DotNetSide
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var pipeWriter = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);
            using var pipeReader = new AnonymousPipeServerStream(PipeDirection.In, HandleInheritability.Inheritable);

            Process client = new Process();

            client.StartInfo.FileName = "node";
            client.StartInfo.Arguments = "../../../../Childwitharguments/index.js " + pipeWriter.GetClientHandleAsString() + " " + pipeReader.GetClientHandleAsString();
            client.StartInfo.UseShellExecute = false;
            client.Start();

            pipeWriter.DisposeLocalCopyOfClientHandle();
            pipeReader.DisposeLocalCopyOfClientHandle();

            _ = StartReadingAsync(pipeReader);

            using var sw = new StreamWriter(pipeWriter)
            {
                AutoFlush = true
            };

            string message = Console.ReadLine();

            while (message != "exit")
            {
                await sw.WriteAsync(message);
                message = Console.ReadLine();
            }

            client.Close();
        }

        private static async Task StartReadingAsync(AnonymousPipeServerStream pipeReader)
        {
            try
            {
                StreamReader sr = new StreamReader(pipeReader);

                while (true)
                {
                    var message = await sr.ReadLineAsync();

                    if (message != null)
                    {
                        Console.WriteLine(message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
