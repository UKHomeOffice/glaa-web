using System;

namespace GLAA.Domain
{
    public interface IDeletable
    {
        bool Deleted { get; set; }
        DateTime? DateDeleted { get; set; }
    }
}
