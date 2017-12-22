using System;
using System.Collections.Generic;
using System.Text;

namespace GLAA.Services
{
    public interface IConstantService
    {
        int NewApplicationStatusId { get; }
        int ApplicationSubmittedOnlineStatusId { get; }
    }
}
