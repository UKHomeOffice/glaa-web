using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.Automapper
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressViewModel>()
                .ForMember(x => x.CountryId, opt => opt.MapFrom(y => y.Country.Id))
                .ForMember(x => x.CountyId, opt => opt.MapFrom(y => y.County.Id))
                .ForMember(x => x.NonUK, opt => opt.MapFrom(y => !y.Country.IsUk))
                .ForMember(x => x.Countries, opt => opt.Ignore())
                .ForMember(x => x.Counties, opt => opt.Ignore());

            CreateMap<AddressViewModel, Address>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Country, opt => opt.Ignore())
                .ForMember(x => x.County, opt => opt.Ignore());
        }
    }
}
