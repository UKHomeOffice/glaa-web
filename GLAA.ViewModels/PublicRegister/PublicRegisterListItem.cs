using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLAA.ViewModels.PublicRegister
{
    public class PublicRegisterListItem<T> : SelectListItem, IEnumMapped<T>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public T EnumMappedTo { get; set; }
    }
}
