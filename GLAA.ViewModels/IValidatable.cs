using System;

namespace GLAA.ViewModels
{
    public interface IValidatable
    {
        void Validate();

        bool IsValid { get; set; }
    }
}
