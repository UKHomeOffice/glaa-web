using System.ComponentModel.DataAnnotations.Schema;
using GLAA.Domain.Models;

namespace GLAA.Domain.Core.Models
{
    public class LicenceWorkerCountry : ICompositeId
    {
        [NotMapped]
        public int Id
        {
            get { return WorkerCountryId; }
            set { WorkerCountryId = value; }
        }        
        public int LicenceId { get; set; }
        public virtual Licence Licence { get; set; }        
        public int WorkerCountryId { get; set; }
        public virtual WorkerCountry WorkerCountry { get; set; }
    }
}
