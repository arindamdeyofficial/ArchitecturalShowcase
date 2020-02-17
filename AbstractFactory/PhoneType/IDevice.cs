using System;

namespace AbstractFactory
{
    public interface IDevice
    {
        DeviceEnum DeviceType { get; set; }
    }
}
