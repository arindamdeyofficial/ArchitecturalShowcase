using Microsoft.Extensions.DependencyInjection;
using System;

namespace PatternCaller
{
    public class BasePatterns
    {
        public readonly IServiceCollection _services;
        public BasePatterns(IServiceCollection services)
        {
            _services.AddPatternsLibrary();
        }
    }
}
