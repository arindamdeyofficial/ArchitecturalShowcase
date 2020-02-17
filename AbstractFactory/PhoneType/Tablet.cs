using System;

namespace AbstractFactory
{
    public class Tablet : IDevice
    {
        public DeviceEnum DeviceType { get => DeviceEnum.TABLET; set => DeviceType = value; }
    }
}
