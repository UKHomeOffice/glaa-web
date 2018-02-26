using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.Repository;
using GLAA.ViewModels;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.LicenceApplication
{
    public class LicenceApplicationViewModelBuilder : ILicenceApplicationViewModelBuilder
    {
        private readonly ILicenceRepository licenceRepository;
        private readonly IMapper mapper;
        private readonly IReferenceDataProvider referenceDataProvider;
        private readonly IConstantService constantService;

        public LicenceApplicationViewModelBuilder(ILicenceRepository licenceRepository, IMapper mapper, IReferenceDataProvider rdp, IConstantService constantService)
        {
            this.licenceRepository = licenceRepository;
            this.mapper = mapper;
            this.referenceDataProvider = rdp;
            this.constantService = constantService;
        }

        public T New<T>() where T : new()
        {
            var model = new T();

            if (model is INeedCountries countryModel)
            {
                countryModel.Countries = referenceDataProvider.GetCountries();
            }

            if (model is INeedCounties countyModel)
            {
                countyModel.Counties = referenceDataProvider.GetCounties();
            }

            return model;
        }

        public LicenceApplicationViewModel New()
        {
            return New<LicenceApplicationViewModel>();
        }

        private LicenceApplicationViewModel BuildFromLicence(Licence licence)
        {
            var model = mapper.Map<LicenceApplicationViewModel>(licence);

            if (licence != null)
            {
                model.Declaration = mapper.Map<DeclarationViewModel>(licence);
                model.PrincipalAuthority =
                    mapper.Map<PrincipalAuthorityViewModel>(licence.PrincipalAuthorities.FirstOrDefault());
                model.AlternativeBusinessRepresentatives =
                    mapper.Map<AlternativeBusinessRepresentativeCollectionViewModel>(licence);
                model.DirectorOrPartner = mapper.Map<DirectorOrPartnerCollectionViewModel>(licence);
                model.NamedIndividuals = mapper.Map<NamedIndividualCollectionViewModel>(licence);
                model.IsSubmitted = GetIsSubmitted(licence);
            }

            return model;
        }

        public LicenceApplicationViewModel Build(string applicationId)
        {
            var licence = licenceRepository.GetByApplicationId(applicationId);
            return BuildFromLicence(licence);
        }

        public LicenceApplicationViewModel Build(int id)
        {
            var licence = licenceRepository.GetById(id);
            return BuildFromLicence(licence);
        }

        public T Build<T>(int licenceId) where T : IIsSubmitted, new()
        {
            var licence = licenceRepository.GetById(licenceId);

            var model = New<T>();

            model = mapper.Map(licence, model);

            model.IsSubmitted = GetIsSubmitted(licence);

            return model;
        }

        public T Build<T, U>(int licenceId, Func<Licence, U> objectSelector) where T : IIsSubmitted, new() where U : new()
        {
            var licence = licenceRepository.GetById(licenceId);
            
            var source = objectSelector(licence);

            if (source == null)
            {
                source = new U();
            }

            if (source is ILinkedToLicence linkable)
            {
                linkable.Licence = licence;
            }

            var model = New<T>();

            mapper.Map(source, model);

            model.IsSubmitted = GetIsSubmitted(licence);

            return model;
        }

        public T Build<T, U>(int licenceId, Func<Licence, ICollection<U>> objectSelector) where T : IIsSubmitted, new()
        {
            var licence = licenceRepository.GetById(licenceId);

            var source = objectSelector(licence);

            var model = New<T>();

            if (source.Any())
            {
                mapper.Map(source, model);
            }

            model.IsSubmitted = GetIsSubmitted(licence);

            return model;
        }

        public T BuildCountriesFor<T>(T model) where T : INeedCountries
        {
            model.Countries = referenceDataProvider.GetCountries();

            return model;
        }

        public IList<LicenceApplicationViewModel> BuildLicencesForUser(string userId)
        {
            var licences = licenceRepository.GetAllApplications().Where(x => x.UserId == userId);

            return mapper.Map<IList<LicenceApplicationViewModel>>(licences);
        }

        private bool GetIsSubmitted(Licence licence)
        {
            var currentStatus = licence.CurrentStatusChange.Status;

            return currentStatus.Id == constantService.ApplicationSubmittedOnlineStatusId;
        }
    }
}
