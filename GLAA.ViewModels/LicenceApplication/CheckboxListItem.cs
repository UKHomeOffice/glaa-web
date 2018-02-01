namespace GLAA.ViewModels.LicenceApplication
{
    public class CheckboxListItem<T> : ICheckboxList<T>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
        public T EnumMappedTo { get; set; }
    }

    public class CheckboxListItem : ICheckboxList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
    }
}
