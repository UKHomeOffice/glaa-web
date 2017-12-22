using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using GLAA.Domain.Models;

namespace GLAA.Domain.Core.Models
{
    public class LicenceMultiple: ICompositeId
    {
        [NotMapped]
        public int Id
        {
            get { return MultipleId; }
            set { MultipleId = value; }
        }        
        public int LicenceId { get; set; }
        public virtual Licence Licence { get; set; }
        public int MultipleId { get; set; }
        public virtual Multiple Multiple { get; set; }
    }
}
