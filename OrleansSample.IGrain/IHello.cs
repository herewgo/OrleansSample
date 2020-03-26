using Orleans;
using System;
using System.Threading.Tasks;

namespace OrleansSample.IGrain
{
    public interface IHello : IGrainWithIntegerKey
    {
        Task<string> SayHello(string greeting);
        Task<int> IncriseAge();
    }
}
