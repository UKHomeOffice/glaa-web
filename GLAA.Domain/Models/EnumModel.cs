using System;
using System.Collections.Generic;
using System.Linq;

namespace GLAA.Domain.Models
{
    public interface IEnumModel : IId
    {
        string Name { get; set; }
    }
}
