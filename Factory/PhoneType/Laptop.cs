using System;

namespace Factory
{
    public class Laptop:IDevice
    {
        public DeviceEnum DeviceType { get => DeviceEnum.LAPTOP; set => DeviceType = value; }
    }
}
