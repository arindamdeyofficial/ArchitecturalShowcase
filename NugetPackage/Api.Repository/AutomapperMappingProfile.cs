using Api.Repository.Models;
using AutoMapper;
using BusinessModel.Payment;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AutomapperConnect
{
    public class AutomapperMappingProfile : Profile
    {
        public AutomapperMappingProfile()
        {
            // Define your mappings here
            CreateMap<PaymentBo, Payment>();
            CreateMap<Payment, PaymentBo>();
        }

    }
}
