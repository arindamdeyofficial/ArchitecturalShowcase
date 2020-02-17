using System;

namespace Factory
{
    public class Tablet : IDevice
    {
        public DeviceEnum DeviceType { get => DeviceEnum.TABLET; set => DeviceType = value; }
    }
}
