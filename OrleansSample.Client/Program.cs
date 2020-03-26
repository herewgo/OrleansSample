using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using OrleansSample.IGrain;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace OrleansSample.Client
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Hello World-Client!");
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                using (var client = await ConnectClient())
                {
                    await DoClientWork(client);
                    Console.ReadKey();
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n尝试运行客户端时发生异常: {ex.Message}");
                Console.WriteLine("请确保客户端尝试连接的 Silo Host 正在运行。");
                Console.WriteLine("\n按任意键退出。");
                Console.ReadKey();
                return 1;
            }
        }

        private static async Task<IClusterClient> ConnectClient()
        {
            IClusterClient client;
            client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "orleansCluster";
                    options.ServiceId = "orleansService";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect();
            Console.WriteLine("客户端已成功连接到Silo Host  \n");
            return client;
        }

        private static async Task DoClientWork(IClusterClient client)
        {

            //从客户端调用Grain的示例
            var friend = client.GetGrain<IHello>(1);
            var response = await friend.SayHello("Good morning, HelloGrain!");
            Console.WriteLine($"\n\n{response}\n\n");
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //int count = 10000;
            //for (int i = 0; i < count; i++)
            //{
            ////var response = await friend.SayHello("Good morning, HelloGrain!");
            ////Console.WriteLine($"\n\n{response}\n\n");
            //    var response = await friend.IncriseAge();
            //    //Console.WriteLine($"{response}\n");

            //}
            //sw.Stop();
            //Console.WriteLine($"total:{sw.ElapsedMilliseconds},avg:{(float)sw.ElapsedMilliseconds / count}");
        }

        //        尝试运行客户端时发生异常: Cannot find generated GrainReference class for interface 'OrleansSample.IGrain.IHello'
        //请确保客户端尝试连接的 Silo Host 正在运行。
    }
}
