using System;

namespace Factory
{
    public class Mobile : IDevice
    {
        public DeviceEnum DeviceType { get => DeviceEnum.MOBILE; set => DeviceType = value; }
    }
}
