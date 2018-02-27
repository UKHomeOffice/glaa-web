using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.Admin;
using System.Linq;

namespace GLAA.Services.Automapper
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<Licence, AdminLicenceViewModel>()
                .ForMember(x => x.Licence, opt => opt.MapFrom(y => y))
                .ForMember(x => x.OrganisationDetails, opt => opt.MapFrom(y => y))
                .ForMember(x => x.Organisation, opt => opt.MapFrom(y => y))
                .ForMember(x => x.PrincipalAuthority, opt => opt.MapFrom(y => y.PrincipalAuthorities.FirstOrDefault()))
                .ForMember(x => x.AlternativeBusinessRepresentatives, opt => opt.MapFrom(y => y.AlternativeBusinessRepresentatives))
                .ForMember(x => x.NamedIndividuals, opt => opt.MapFrom(y => y.NamedIndividuals))
                .ForMember(x => x.DirectorsOrPartners, opt => opt.MapFrom(y => y.DirectorOrPartners))
                .ForMember(x => x.NewLicenceStatus, opt => opt.Ignore())
                .ForMember(x => x.NewStatusReason, opt => opt.Ignore())
                .ForMember(x => x.PleaseSelectItem, opt => opt.Ignore())
                .ForMember(x => x.Standards, opt => opt.Ignore())
                .ForMember(x => x.Countries, opt => opt.Ignore())
                .ForMember(x => x.Counties, opt => opt.Ignore())
                .ForMember(x => x.IsSubmitted, opt => opt.ResolveUsing(ProfileHelpers.GetIsSubmitted));
        }
    }
}
