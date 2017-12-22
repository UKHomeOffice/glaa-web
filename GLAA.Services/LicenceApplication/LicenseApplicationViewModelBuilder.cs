using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.Repository;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.LicenceApplication
{
    public class LicenceApplicationViewModelBuilder : ILicenceApplicationViewModelBuilder
    {
        private readonly ILicenceRepository licenceRepository;
        private readonly IMapper mapper;

        public LicenceApplicationViewModelBuilder(ILicenceRepository licenceRepository, IMapper mapper)
        {
            this.licenceRepository = licenceRepository;
            this.mapper = mapper;
        }

        public T New<T>() where T : new()
        {
            return new T();
        }

        public LicenceApplicationViewModel New()
        {
            return new LicenceApplicationViewModel();
        }

        private LicenceApplicationViewModel BuildFromLicence(Licence licence)
        {
            var model = mapper.Map<LicenceApplicationViewModel>(licence);

            if (licence != null)
            {
                model.Eligibility = mapper.Map<EligibilityViewModel>(licence);
                model.Declaration = mapper.Map<DeclarationViewModel>(licence);
                model.PrincipalAuthority =
                    mapper.Map<PrincipalAuthorityViewModel>(licence.PrincipalAuthorities.FirstOrDefault());
                model.AlternativeBusinessRepresentatives =
                    mapper.Map<AlternativeBusinessRepresentativeCollectionViewModel>(licence);
                model.DirectorOrPartner = mapper.Map<DirectorOrPartnerCollectionViewModel>(licence);
                model.NamedIndividuals = mapper.Map<NamedIndividualCollectionViewModel>(licence);
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

        public T Build<T>(int licenceId) where T : new()
        {
            var licence = licenceRepository.GetById(licenceId);

            var model = mapper.Map<T>(licence);

            return model;
        }

        public T Build<T, U>(int licenceId, Func<Licence, U> objectSelector) where T : new() where U : new()
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

            var model = new T();
                
            mapper.Map(source, model);

            return model;
        }

        public T Build<T, U>(int licenceId, Func<Licence, ICollection<U>> objectSelector) where T : new()
        {
            var licence = licenceRepository.GetById(licenceId);

            var source = objectSelector(licence);

            var model = new T();

            if (source.Any())
            {
                mapper.Map(source, model);
            }            

            return model;
        }
    }
}
