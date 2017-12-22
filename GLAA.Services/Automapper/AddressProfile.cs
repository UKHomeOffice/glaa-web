using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.Automapper
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressViewModel>();

            CreateMap<AddressViewModel, Address>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
