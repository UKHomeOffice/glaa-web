namespace GLAA.ViewModels.LicenceApplication
{
    public interface ICheckboxList<T> : IEnumMapped<T>
    {
        int Id { get; set; }
        string Name { get; set; }
        bool Checked { get; set; }
    }

    public interface ICheckboxList
    {
        int Id { get; set; }
        string Name { get; set; }
        bool Checked { get; set; }
    }
}