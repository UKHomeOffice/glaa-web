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
                .ForMember(x => x.Counties, opt => opt.Ignore())
                .ForMember(x => x.IsValid, opt => opt.Ignore());

            CreateMap<Address, AddressPageViewModel>()
                .ForMember(x => x.Address, opt => opt.MapFrom(y => y))
                .ForMember(x => x.PostCodeLookup, opt => opt.MapFrom(y => y.Postcode))
                .ForMember(x => x.Countries, opt => opt.Ignore())
                .ForMember(x => x.Counties, opt => opt.Ignore())
                .ForMember(x => x.IsValid, opt => opt.Ignore());

            CreateMap<AddressViewModel, Address>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.Country, opt => opt.Ignore())
                .ForMember(x => x.County, opt => opt.Ignore());

            CreateMap<AddressPageViewModel, Address>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Address.Id))
                .ForMember(x => x.AddressLine1, opt => opt.MapFrom(y => y.Address.AddressLine1))
                .ForMember(x => x.AddressLine2, opt => opt.MapFrom(y => y.Address.AddressLine2))
                .ForMember(x => x.AddressLine3, opt => opt.MapFrom(y => y.Address.AddressLine3))
                .ForMember(x => x.Town, opt => opt.MapFrom(y => y.Address.Town))
                .ForMember(x => x.Postcode, opt => opt.MapFrom(y => y.Address.Postcode))
                .ForMember(x => x.CountyId, opt => opt.MapFrom(y => y.Address.CountyId))
                .ForMember(x => x.CountryId, opt => opt.MapFrom(y => y.Address.CountryId))
                .ForMember(x => x.Country, opt => opt.Ignore())
                .ForMember(x => x.County, opt => opt.Ignore());
        }
    }
}
