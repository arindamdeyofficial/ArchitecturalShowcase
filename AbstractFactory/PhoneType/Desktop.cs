using System;

namespace AbstractFactory
{
    public class Desktop : IDevice
    {
        public DeviceEnum DeviceType { get => DeviceEnum.DESKTOP; set => DeviceType=value; }
    }
}
