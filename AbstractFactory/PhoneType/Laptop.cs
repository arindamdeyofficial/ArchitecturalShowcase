using System;

namespace AbstractFactory
{
    public class Laptop:IDevice
    {
        public DeviceEnum DeviceType { get => DeviceEnum.LAPTOP; set => DeviceType = value; }
    }
}
