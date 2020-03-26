using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OrleansSample.Grain;
using OrleansSample.IGrain;
using System.Net;

namespace OrleansSample.Host
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("Hello World-Host!");
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("\n\n 按回车键停止 \n\n");
                Console.ReadLine();

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            //定义群集配置
            var builder = new SiloHostBuilder()
                .UseLocalhostClustering()//配置Silo只使用开发集群，并监听本地主机
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "orleansCluster";//获取或设置群集标识。这在Orleans 2.0名称之前曾被称为DeploymentId。
                    options.ServiceId = "orleansService";//获取或设置此服务的唯一标识符，该标识符应在部署和重新部署后继续存在，其中Orleans.Configuration.ClusterOptions.ClusterId可能不存在。
                })

            // 配置连接
            .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                //.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(HelloGrain).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();//运行给定的配置来初始化主机。只能调用一次。
            await host.StartAsync();//启动当前Silo.
            return host;

            //            Hello World-Host!
            //warn: Orleans.Runtime.NoOpHostEnvironmentStatistics[100708]
            //      No implementation of IHostEnvironmentStatistics was found. Load shedding will not work yet
            //Orleans.Runtime.OrleansConfigurationException: None of the assemblies added to ApplicationPartManager contain generated code for grain interfaces. Ensure that code generation has been executed for grain interface and grain class assemblies and that they have been added as application parts.
            //   at Orleans.Configuration.Validators.ApplicationPartValidator.ValidateConfiguration()
            //   at Orleans.Hosting.SiloHostBuilder.ValidateSystemConfiguration(IServiceProvider serviceProvider)
            //   at Orleans.Hosting.SiloHostBuilder.Build()
            //   at OrleansSample.Host.Program.StartSilo() in C:\Users\luffy.lu\source\repos\OrleansSample\OrleansSample.Host\Program.cs:line 51
            //   at OrleansSample.Host.Program.RunMainAsync() in C:\Users\luffy.lu\source\repos\OrleansSample\OrleansSample.Host\Program.cs:line 23

            //C:\Users\luffy.lu\source\repos\OrleansSample\OrleansSample.Host\bin\Debug\netcoreapp3.1\OrleansSample.Host.exe (process 23176) exited with code 1.

        }
    }
}
