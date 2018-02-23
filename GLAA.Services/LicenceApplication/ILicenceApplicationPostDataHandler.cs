using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GLAA.Domain;
using GLAA.Domain.Models;
using GLAA.ViewModels.LicenceApplication;

namespace GLAA.Services.LicenceApplication
{
    public interface ILicenceApplicationPostDataHandler : IPostDataHandler<LicenceApplicationViewModel>
    {
        int Insert(LicenceApplicationViewModel model);

        void Delete<T>(int id) where T : class, IId, IDeletable;

        void Update<T, U>(int licenceId, Func<Licence, U> objectSelector, T model) where U : class, IId, new();

        void Update(int licenceId, LicenceApplicationViewModel model);

        void UpdateAddress<T>(int licenceId, Func<Licence, T> objectSelector, AddressViewModel model) where T : class, IId, IAddressable, new();

        void UpdateShellfishStatus(int licenceId, OperatingIndustriesViewModel model);

        void UpdateUser(int licenceId, string userId);

        void Update<T, U>(int licenceId, Func<Licence, ICollection<T>> objectSelector, U model)
            where T : class, IId, new() where U : IId;

        void UpdateAll<T, U>(int licenceId, Func<Licence, ICollection<T>> objectSelector, IEnumerable<U> models)
            where T : class, IId, IDeletable, new() where U : IId;

        int Update<T, U>(int licenceId, Func<Licence, ICollection<T>> objectSelector, U model, int id)
            where T : class, IId, ILinkedToLicence, new();

        void Update<T, U>(int licenceId, Func<Licence, ICollection<T>> objectSelector, List<U> modelList)
            where U : ICheckboxList where T : class, ICompositeId, new();

        int UpsertSecurityAnswerAndLinkToParent<T, TAnswer, TParent>(int parentId, int answerId, T model,
            Expression<Func<TParent, IEnumerable<TAnswer>>> answerCollection,
            Expression<Func<TAnswer, TParent>> parentLinkingProperty)
            where TParent : class, IId where TAnswer : class, IId, new();

        int UpsertPrincipalAuthorityAndLinkToDirectorOrPartner<T>(int licenceId, int dopId, int paId, T model);

        int UpsertDirectorOrPartnerAndLinkToPrincipalAuthority<T>(int licenceId, int paId, int dopId, T model);

        void UnlinkPrincipalAuthorityFromDirectorOrPartner(int dopId);

        void UnlinkDirectorOrPartnerFromPrincipalAuthority(int paId);
    }
}
