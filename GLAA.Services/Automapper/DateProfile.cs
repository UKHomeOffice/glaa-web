using System;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.Automapper
{
    public class DateProfile : Profile
    {
        public DateProfile()
        {
            CreateMap<DateTime, DateViewModel>()
                .ForMember(x => x.Day, opt => opt.MapFrom(y => y.Day))
                .ForMember(x => x.Month, opt => opt.MapFrom(y => y.Month))
                .ForMember(x => x.Year, opt => opt.MapFrom(y => y.Year))
                .ForMember(x => x.Date, opt => opt.Ignore());

            CreateMap<DateTime, DateOfBirthViewModel>()
                .ForMember(x => x.DateOfBirth, opt => opt.MapFrom(y => y));

            CreateMap<DateViewModel, DateTime?>()
                .ConvertUsing(x => x.Date);

            CreateMap<DateViewModel, DateTime>()
                .ConvertUsing(x => x.Date.Value);

            CreateMap<DateOfBirthViewModel, DateTime?>()
                .ConvertUsing(x => x.DateOfBirth.Date);
        }
    }
}
