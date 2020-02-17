using Factory;
using System;

namespace Factory
{
    public class FactoryPattern: IFactoryPattern
    {        
        public IDevice CreateDevice(string deviceType)
        {
            switch (Enum.Parse(typeof(DeviceEnum), deviceType.ToUpper()))
            {
                case DeviceEnum.DESKTOP:
                    return new Desktop();
                case DeviceEnum.MOBILE:
                    return new Mobile();
                case DeviceEnum.LAPTOP:
                    return new Laptop();
                case DeviceEnum.TABLET:
                    return new Tablet();
                default:
                    return new NoDevice();
            }
        }
    }
}
