using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using GLAA.Domain.Models;
using GLAA.Repository;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.LicenceApplication
{
    public class LicenceApplicationPostDataHandler : ILicenceApplicationPostDataHandler
    {
        private readonly IEntityFrameworkRepository repository;
        private readonly ILicenceRepository licenceRepository;
        private readonly IStatusRepository statusRepository;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;

        public LicenceApplicationPostDataHandler(
            IMapper mapper,
            IEntityFrameworkRepository repository, ILicenceRepository licenceRepository,
            IStatusRepository statusRepository,
            IDateTimeProvider dateTimeProvider)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.licenceRepository = licenceRepository;
            this.statusRepository = statusRepository;
            this.dateTimeProvider = dateTimeProvider;
        }

        public int Insert(LicenceApplicationViewModel model)
        {
            var licence = repository.Create<Licence>();

            var address = repository.Create<Address>();            

            licence.Address = address;

            repository.Upsert(licence);

            licence.ApplicationId = CreateDraftURN(licence.Id);
            UpdateStatus(licence, model);

            repository.Upsert(licence);

            return licence.Id;
        }

        public void Delete<T>(int id) where T : class, IId
        {
            // TODO: soft deletes?
            repository.Delete<T>(id);
        }

        public void Update<T, U>(int licenceId, Func<Licence, U> objectSelector, T model) where U : class, IId, new()
        {
            var licence = licenceRepository.GetById(licenceId);

            var selectedObject = objectSelector(licence) ?? repository.Create<U>();

            mapper.Map(model, selectedObject);

            repository.Upsert(licence);
        }

        public void Update(int licenceId, LicenceApplicationViewModel model)
        {
            var licence = licenceRepository.GetById(licenceId);

            UpdateStatus(licence, model);

            repository.Upsert(licence);
        }

        public void UpdateAddress<T>(int licenceId, Func<Licence, T> objectSelector, AddressViewModel model) where T : class, IAddressable, IId, new()
        {
            var licence = licenceRepository.GetById(licenceId);

            T selectedObject = objectSelector(licence);

            if (selectedObject == null)
            {
                selectedObject = repository.Create<T>();

                var linkable = selectedObject as ILinkedToLicence;
                if (linkable != null)
                {
                    linkable.Licence = licence;
                }
            }

            if (selectedObject.Address == null)
            {
                selectedObject.Address = repository.Create<Address>();
            }

            mapper.Map(model, selectedObject.Address);

            repository.Upsert(licence);
        }

        public void Update<T, U>(int licenceId, Func<Licence, ICollection<T>> objectSelector, U model)
            where T : class, IId, new() where U : IId
        {
            var licence = licenceRepository.GetById(licenceId);

            var collection = objectSelector(licence);

            if (collection.None(c => c.Id == model.Id))
            {
                var item = repository.Create<T>();
                mapper.Map(model, item);
                collection.Add(item);
            }
            else
            {
                mapper.Map(model, collection.Single(c => c.Id == model.Id));
            }

            repository.Upsert(licence);
        }

        public void UpdateAll<T, U>(int licenceId, Func<Licence, ICollection<T>> objectSelector, IEnumerable<U> models)
            where T : class, IId, new() where U : IId
        {
            foreach (var model in models)
            {
                Update(licenceId, objectSelector, model);
            }
        }

        public int Update<T, U>(int licenceId, Func<Licence, ICollection<T>> objectSelector, U model, int id)
            where T : class, IId, ILinkedToLicence, new()            
        {
            var licence = licenceRepository.GetById(licenceId);

            var collection = objectSelector(licence);

            T entity;

            if (collection.None(p => p.Id == id))
            {
                entity = repository.Create<T>();
                entity.Licence = licence;

                var hasAddress = entity as IAddressable;
                if (hasAddress != null)
                {
                    var address = repository.Create<Address>();
                    hasAddress.Address = address;
                }
            }
            else
            {
                entity = collection.Single(p => p.Id == id);
            }

            mapper.Map(model, entity);

            return repository.Upsert(entity);
        }

        public void Update<T, U>(int licenceId, Func<Licence, ICollection<T>> objectSelector, List<U> modelList)
            where U : ICheckboxList
            where T : class, ICompositeId, new()
        {
            var licence = licenceRepository.GetById(licenceId);

            var collection = objectSelector(licence);

            foreach (var addModel in modelList.Where(x => x.Checked && collection.None(t => t.Id == x.Id && t.LicenceId == licence.Id)))
            {                
                var joiningEntity = new T
                {
                    Id = addModel.Id,
                    LicenceId = licence.Id
                };
                
                collection.Add(joiningEntity);                
            }

            foreach (var removeModel in modelList.Where(x => !x.Checked && collection.Any(c => c.Id == x.Id && c.LicenceId == licence.Id)))
            {
                collection.Remove(collection.Single(x => x.Id == removeModel.Id && x.LicenceId == licence.Id));
            }

            repository.Upsert(licence);
        }

        /// <summary>
        /// Create or update a restraint order and link it to the principal authority
        /// </summary>
        /// <param name="paId">The ID of the PA</param>
        /// <param name="roId">The ID of the ro if it exists</param>
        /// <param name="model">The model to update from</param>
        /// <returns>The ID of the RO</returns>
        public int UpsertRestraintOrderAndLinkToPrincipalAuthority(int paId, int roId, RestraintOrdersViewModel model)
        {
            var pa = repository.GetById<PrincipalAuthority>(paId);

            RestraintOrder ro;

            if (pa.RestraintOrders.None(r => r.Id == roId))
            {
                ro = repository.Create<RestraintOrder>();
                ro.PrincipalAuthority = pa;
            }
            else
            {
                ro = pa.RestraintOrders.Single(r => r.Id == roId);
            }

            this.mapper.Map(model, ro);

            return repository.Upsert(ro);
        }

        /// <summary>
        /// Create or update an answer to a security question with multiple entries and link it to its parent.
        /// </summary>
        /// <param name="parentId">The ID of the parent</param>
        /// <param name="answerId">The ID of the answer if it exists</param>
        /// <param name="model">The model to update the answer from</param>
        /// <param name="answerCollection">A function to get the collection of answers from the parent</param>
        /// <param name="parentLinkingProperty">A function to get the member that links the answer to the parent</param>
        /// <returns>The ID of the answer</returns>
        public int UpsertSecurityAnswerAndLinkToParent<T, TAnswer, TParent>(int parentId, int answerId, T model,
            Expression<Func<TParent, IEnumerable<TAnswer>>> answerCollection, Expression<Func<TAnswer, TParent>> parentLinkingProperty)
            where TParent : class, IId where TAnswer : class, IId, new()
        {
            // Get the parent
            var parent = repository.GetById<TParent>(parentId);

            var answers = answerCollection.Compile()(parent)?.ToArray() ?? new TAnswer[] { };

            TAnswer answer;

            // Does the parent already have this answer?
            if (answers.None(a => a.Id == answerId))
            {
                // Get the record to link to or create a new one
                answer = repository.GetById<TAnswer>(answerId) ?? repository.Create<TAnswer>();
                // Link the answer to the parent by setting the parent property on the answer
                // Have to do it this way round as the answer is the think we're Upserting
                answer.SetPropertyValue(parentLinkingProperty, parent);
            }
            else
            {
                // Get this answer from the parent
                answer = answers.Single(a => a.Id == answerId);
            }

            // Update the answer from the model
            this.mapper.Map(model, answer);

            return repository.Upsert(answer);
        }

        public int UpsertPrincipalAuthorityAndLinkToDirectorOrPartner<T>(int licenceId, int dopId, int paId, T model)
        {
            var licence = licenceRepository.GetById(licenceId);

            PrincipalAuthority pa;

            if (licence.PrincipalAuthorities.None(p => p.Id == paId))
            {
                pa = repository.Create<PrincipalAuthority>();
                pa.Licence = licence;
            }
            else
            {
                pa = licence.PrincipalAuthorities.Single(p => p.Id == paId);
            }

            this.mapper.Map(model, pa);

            if (pa.DirectorOrPartner == null)
            {
                var dop = repository.GetById<DirectorOrPartner>(dopId);
                pa.DirectorOrPartner = dop;
                dop.PrincipalAuthority = pa;
            }

            return repository.Upsert(pa);
        }

        public int UpsertDirectorOrPartnerAndLinkToPrincipalAuthority<T>(int licenceId, int paId, int dopId, T model)
        {
            var licence = licenceRepository.GetById(licenceId);

            DirectorOrPartner dop;

            if (licence.DirectorOrPartners.None(d => d.Id == dopId))
            {
                dop = repository.Create<DirectorOrPartner>();
                dop.Licence = licence;
            }
            else
            {
                dop = licence.DirectorOrPartners.Single(d => d.Id == dopId);
            }

            this.mapper.Map(model, dop);

            if (dop.PrincipalAuthority == null)
            {
                var pa = repository.GetById<PrincipalAuthority>(paId) ?? licence.PrincipalAuthorities.SingleOrDefault(p => p.IsCurrent);
                dop.PrincipalAuthority = pa;
                if (pa != null)
                {
                    pa.DirectorOrPartner = dop;
                }
            }

            return repository.Upsert(dop);
        }
        
        public void UnlinkPrincipalAuthorityFromDirectorOrPartner(int dopId)
        {
            var dop = repository.GetById<DirectorOrPartner>(dopId);

            if (dop != null)
            {
                dop.PrincipalAuthority = null;
            }

            repository.Upsert(dop);
        }

        public void UnlinkDirectorOrPartnerFromPrincipalAuthority(int paId)
        {
            var pa = repository.GetById<PrincipalAuthority>(paId);

            if (pa != null)
            {
                pa.DirectorOrPartner = null;
            }

            repository.Upsert(pa);
        }

        private string CreateDraftURN(int licenceId)
        {
            var licence = repository.GetById<Licence>(licenceId);

            var paddedId = licence.Id.ToString().PadLeft(4, '0');

            return $"DRAFT-{paddedId}";
        }

        private void UpdateStatus(Licence licence, LicenceApplicationViewModel model)
        {
            if (model.NewLicenceStatus != null)
            {
                var change = repository.Create<LicenceStatusChange>();
                //TODO Map this from the VM?
                change.Status = repository.GetById<LicenceStatus>(model.NewLicenceStatus.Id);
                change.DateCreated = dateTimeProvider.Now();
                change.Licence = licence;
                repository.Upsert(change);
            }
        }

        public void UpdateShellfishStatus(int licenceId, OperatingIndustriesViewModel model)
        {
            var licence = licenceRepository.GetById<Licence>(licenceId);

            licence.IsShellfish = model.OperatingIndustries.Any(x => x.Id == 4 && x.Checked);
        }

        public void UpdateUser(int licenceId, string userId)
        {
            var licence = licenceRepository.GetById(licenceId);

            licence.UserId = userId;
        }
    }
}
