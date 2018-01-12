using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.Admin;

namespace GLAA.Services.Automapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<GLAAUser, AdminUserViewModel>()
                .ForMember(x => x.Email, opt => opt.MapFrom(y => y.Email))
                .ForMember(x => x.FullName, opt => opt.MapFrom(y => y.FullName))
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Id))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<AdminUserViewModel, GLAAUser>()
                .ForMember(x => x.FullName, opt => opt.MapFrom(y => y.FullName))
                .ForMember(x => x.Email, opt => opt.MapFrom(y => y.Email))
                .ForMember(x => x.UserName, opt => opt.MapFrom(y => y.Email))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
