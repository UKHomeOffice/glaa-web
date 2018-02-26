using System.Collections.Generic;
using System.Linq;
using GLAA.Domain.Models;
using GLAA.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.Services
{
    public class ReferenceDataProvider : IReferenceDataProvider
    {
        private readonly IEntityFrameworkRepository repository;

        public ReferenceDataProvider(IEntityFrameworkRepository repo)
        {
            repository = repo;
        }

        public IEnumerable<SelectListItem> GetCountries()
        {
            return repository
                .GetAll<Country>()
                .Select(c => new SelectListItem {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).OrderBy(x => x.Text);
        }

        public IEnumerable<SelectListItem> GetCounties()
        {
            return repository
                .GetAll<County>()
                .Select(c => new SelectListItem {
                    Value = c.Id.ToString(),
                    Text = c.Name 
                 }).OrderBy(x => x.Text);
        }
    }
}
