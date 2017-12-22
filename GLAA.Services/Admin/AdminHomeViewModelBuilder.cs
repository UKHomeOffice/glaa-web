using System.Linq;
using GLAA.Repository;
using GLAA.ViewModels.Admin;

namespace GLAA.Services.Admin
{
    public class AdminHomeViewModelBuilder : IAdminHomeViewModelBuilder
    {
        private readonly ILicenceRepository licenceRepository;

        public AdminHomeViewModelBuilder(ILicenceRepository licenceRepository)
        {
            this.licenceRepository = licenceRepository;
        }

        public T New<T>() where T : new()
        {
            return new T();
        }

        public AdminHomeViewModel New()
        {
            return new AdminHomeViewModel
            {
                ApplicationCount = licenceRepository.GetAllApplications().Count(),
                LicenceCount = licenceRepository.GetAllLicences().Count()
            };
        }
    }
}
