using System;
using Microsoft.Extensions.DependencyInjection;

namespace Bob.Demo.BLL
{
    public class ScopedClass : IScopedTransientDependency
    {
        public int Number { set; get; } = new Random().Next(10000);

        public string Say()
        {
            return "Hello Scoped~!"+ Number;
        }
    }
}