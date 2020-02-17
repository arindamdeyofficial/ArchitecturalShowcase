using System;

namespace AbstractFactory
{
    public class NoDevice : IDevice
    {
        public DeviceEnum DeviceType { get => DeviceEnum.NODEVICE; set => DeviceType = value; }
    }
}
