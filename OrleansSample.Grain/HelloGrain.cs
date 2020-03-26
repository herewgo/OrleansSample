using Microsoft.Extensions.Logging;
using Orleans;
using OrleansSample.IGrain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrleansSample.Grain
{
    public class HelloGrain : Orleans.Grain, IHello
    {
        private readonly ILogger logger;

        public HelloGrain(ILogger<HelloGrain> logger)
        {
            this.logger = logger;
        }
        //public static List<User> Users = LoadUsers();
        //private static List<User> LoadUsers()
        //{
        //    var users = new List<User>();

        //    for (int i = 1; i <= 10; i++)
        //    {
        //        users.Add(new User()
        //        {
        //            Id = i,
        //            UserName = "",
        //        });
        //    }

        //    return users;
        //}

        public static User User = new User() { Id = 1 };
        Task<string> IHello.SayHello(string greeting)
        {
            //int key = (int)this.GetPrimaryKeyLong();

            logger.LogInformation($"\n 收到SayHello消息: greeting = '{greeting}'");
            return Task.FromResult($"\n Client said: '{greeting}', so HelloGrain says: Hello!");
        }

        public Task<int> IncriseAge()
        {
            int age = ++User.Age;
            int key = (int)this.GetPrimaryKeyLong();
            //logger.LogInformation($"key:{key},age:{age}\n");

            return Task.FromResult(age);
        }
    }

    public class User
    {
        public int Id { set; get; }
        public string UserName { set; get; }
        public int Age { set; get; }
    }
}
