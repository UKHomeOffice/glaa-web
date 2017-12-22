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
                .ForMember(x => x.AgreedToStatementOne, opt => opt.MapFrom(y => y.AgreedToStatementOne ?? false))
                .ForMember(x => x.AgreedToStatementTwo, opt => opt.MapFrom(y => y.AgreedToStatementTwo ?? false))
                .ForMember(x => x.AgreedToStatementThree, opt => opt.MapFrom(y => y.AgreedToStatementThree ?? false))
                .ForMember(x => x.AgreedToStatementFour, opt => opt.MapFrom(y => y.AgreedToStatementFour ?? false))
                .ForMember(x => x.AgreedToStatementFive, opt => opt.MapFrom(y => y.AgreedToStatementFive ?? false))
                .ForMember(x => x.AgreedToStatementSix, opt => opt.MapFrom(y => y.AgreedToStatementSix ?? false))
                .ForMember(x => x.SignatoryName, opt => opt.MapFrom(y => y.SignatoryName))
                .ForMember(x => x.SignatureDate, opt => opt.ResolveUsing(x => DateTimeViewModelResolver(x.SignatureDate)));                

            CreateMap<DeclarationViewModel, Licence>()
                .ForMember(x => x.AgreedToStatementOne, opt => opt.MapFrom(y => y.AgreedToStatementOne))
                .ForMember(x => x.AgreedToStatementTwo, opt => opt.MapFrom(y => y.AgreedToStatementTwo))
                .ForMember(x => x.AgreedToStatementThree, opt => opt.MapFrom(y => y.AgreedToStatementThree))
                .ForMember(x => x.AgreedToStatementFour, opt => opt.MapFrom(y => y.AgreedToStatementFour))
                .ForMember(x => x.AgreedToStatementFive, opt => opt.MapFrom(y => y.AgreedToStatementFive))
                .ForMember(x => x.AgreedToStatementSix, opt => opt.MapFrom(y => y.AgreedToStatementSix))
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
