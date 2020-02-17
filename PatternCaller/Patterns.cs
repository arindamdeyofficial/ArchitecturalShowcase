using Factory;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace PatternCaller
{
    public class Patterns: BasePatterns, IPatterns
    {
        public readonly IFactoryPattern _factory;
        public Patterns(IFactoryPattern factory):base()
        {
            _factory = factory;
        }

        public IDevice CreateDevice(string deviceType)
        {
            return _factory.CreateDevice(deviceType);
        }
    }
}
