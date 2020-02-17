using System;

namespace Factory
{
    public interface IDeviceFactory
    {
        IDevice CreateDevice(string deviceType);
    }
}
