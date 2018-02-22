using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels;
using GLAA.ViewModels.Admin;

namespace GLAA.Services.Automapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<GLAAUser, AdminUserViewModel>()
                .ForMember(x => x.Email, opt => opt.MapFrom(y => y.Email))
                .ForMember(x => x.Title, opt => opt.MapFrom(y => y.Title))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(y => y.FirstName))
                .ForMember(x => x.MiddleName, opt => opt.MapFrom(y => y.MiddleName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(y => y.LastName))
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Id))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<UserViewModel, GLAAUser>()
                .ForMember(x => x.Title, opt => opt.MapFrom(y => y.Title))
                .ForMember(x => x.FirstName, opt => opt.MapFrom(y => y.FirstName))
                .ForMember(x => x.MiddleName, opt => opt.MapFrom(y => y.MiddleName))
                .ForMember(x => x.LastName, opt => opt.MapFrom(y => y.LastName))
                .ForMember(x => x.Email, opt => opt.MapFrom(y => y.Email))
                .ForMember(x => x.UserName, opt => opt.MapFrom(y => y.Email))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
