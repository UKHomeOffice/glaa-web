namespace GLAA.ViewModels
{
    /// <summary>
    /// Allows a view model to determine whether its view should be shown,
    /// based on the state of its parent.
    /// </summary>
    /// <typeparam name="TParent">The parent view model</typeparam>
    public interface ICanView<in TParent>
    {
        /// <summary>
        /// Can the model be viewed?
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        bool CanView(TParent parent);
    }
}
