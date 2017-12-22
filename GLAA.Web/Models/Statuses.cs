using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLAA.Domain.Models;

namespace GLAA.Web.Models
{

    public class DefaultStatus
    {
        public int Id { get; set; }
        public string InternalStatus { get; set; }
        public string InternalDescription { get; set; }
        public string ExternalDescription { get; set; }
        public int[] NextStatusIds { get; set; }
        public string CssClassStem { get; set; }
        public StatusAdminCategory AdminCategory { get; set; }
        public IEnumerable<StatusReason> StatusReasons { get; set; }
    }
}
