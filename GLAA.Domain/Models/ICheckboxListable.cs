﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLAA.Domain.Models
{
    public interface ICheckboxListable : IId
    {
        string Name { get; set; }
    }
}
