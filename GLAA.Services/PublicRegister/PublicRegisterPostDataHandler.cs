using AutoMapper;
using GLAA.Repository;

namespace GLAA.Services.PublicRegister
{
    public class PublicRegisterPostDataHandler : IPublicRegisterPostDataHandler
    {
        private readonly IEntityFrameworkRepository repository;
        private readonly ILicenceRepository licenceRepository;
        private readonly IMapper mapper;

        public PublicRegisterPostDataHandler(
            IMapper mapper,
            IEntityFrameworkRepository repository, 
            ILicenceRepository licenceRepository)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.licenceRepository = licenceRepository;
        }
    }
}
