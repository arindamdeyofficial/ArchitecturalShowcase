using System;

namespace Factory
{
    public class NoDevice : IDevice
    {
        public DeviceEnum DeviceType { get => DeviceEnum.NODEVICE; set => DeviceType = value; }
    }
}
