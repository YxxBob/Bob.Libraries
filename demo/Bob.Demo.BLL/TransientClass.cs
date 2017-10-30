using System;
using Microsoft.Extensions.DependencyInjection;

namespace Bob.Demo.BLL
{
    public class TransientClass : ITransientDependency
    {
        public int Number { set; get; } = new Random().Next(10000);

        public string Say()
        {
            return "Hello Scoped~!" + Number;
        }
    }
}
