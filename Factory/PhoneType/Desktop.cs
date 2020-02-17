using System;

namespace Factory
{
    public class Desktop : IDevice
    {
        public DeviceEnum DeviceType { get => DeviceEnum.DESKTOP; set => DeviceType=value; }
    }
}
