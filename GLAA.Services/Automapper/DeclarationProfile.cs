using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.Automapper
{
    public class DeclarationProfile : Profile
    {
        public DeclarationProfile()
        {
            CreateMap<Licence, DeclarationViewModel>()
                .ForMember(x => x.IsValid, opt => opt.Ignore())
                .ForMember(x => x.SignatoryName, opt => opt.MapFrom(y => y.SignatoryName))
                .ForMember(x => x.SignatureDate, opt => opt.ResolveUsing(x => DateTimeViewModelResolver(x.SignatureDate)));                

            CreateMap<DeclarationViewModel, Licence>()
                .ForMember(x => x.SignatoryName, opt => opt.MapFrom(y => y.SignatoryName))
                .ForMember(x => x.SignatureDate, opt => opt.MapFrom(y => y.SignatureDate.Date))
                .ForAllOtherMembers(opt => opt.Ignore());
        }

        private DateViewModel DateTimeViewModelResolver(DateTime? date)
        {
            if (date.HasValue == false)
            {
                date = DateTime.Now;
            }

            return new DateViewModel
            {
                Day = date.Value.Day,
                Month = date.Value.Month,
                Year = date.Value.Year
            };
        }
    }
}
