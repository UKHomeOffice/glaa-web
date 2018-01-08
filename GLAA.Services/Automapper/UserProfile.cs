using System;
using System.Collections.Generic;
using System.Text;
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
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
