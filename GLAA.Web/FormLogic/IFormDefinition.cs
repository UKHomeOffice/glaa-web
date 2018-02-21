using GLAA.ViewModels;

namespace GLAA.Web.FormLogic
{
    public interface IFormDefinition
    {
        /// <summary>
        /// Gets the view model from the parent for the page matching the section and action name.
        /// </summary>
        /// <typeparam name="TParent">The type of the parent view model.</typeparam>
        /// <param name="section">The form section.</param>
        /// <param name="actionName">The name of the action the model is for.</param>
        /// <param name="parent">The parent view model.</param>
        /// <returns>The child view model of the parent corresponding to the section and action name.</returns>
        object GetViewModel<TParent>(FormSection section, string actionName, TParent parent);

        /// <summary>
        /// <para>Checks if the page for the specified action is viewable.</para>
        /// <para>If the model for the page implements <see cref="ICanView{TParent}"/> it will check against that condition using <see cref="parent"/>, otherwise it will assume that the page can be viewed.</para>
        /// </summary>
        /// <typeparam name="TParent">The type of the parent view model.</typeparam>
        /// <param name="section">The form section.</param>
        /// <param name="actionName">The name of the action the model is for.</param>
        /// <param name="parent">The parent view model.</param>
        /// <returns>Whether the page for the specified action is viewable</returns>
        bool CanViewPage<TParent>(FormSection section, string actionName, TParent parent);

        /// <summary>
        /// Gets the number of pages in the specified section.
        /// </summary>
        /// <param name="section">The form section.</param>
        /// <returns>The number of pages in the specified section.</returns>
        int GetSectionLength(FormSection section);
    }
}