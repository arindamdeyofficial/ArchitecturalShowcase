using System;

namespace AbstractFactory
{
    public interface IDeviceFactory
    {
        IDevice CreateDevice(string deviceType);
    }
}
