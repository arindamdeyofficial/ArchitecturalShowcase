using System;

namespace AbstractFactory
{
    public class Mobile : IDevice
    {
        public DeviceEnum DeviceType { get => DeviceEnum.MOBILE; set => DeviceType = value; }
    }
}
