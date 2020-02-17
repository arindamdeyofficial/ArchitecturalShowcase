using Microsoft.Extensions.DependencyInjection;
using System;

namespace PatternCaller
{
    public class BasePatterns
    {
        public BasePatterns()
        {
            var services = new ServiceCollection();
            services.AddPatternsLibrary();
        }
    }
}
