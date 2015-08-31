// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserProfileModelBinder.cs" company="">
//   
// </copyright>
// <summary>
//   The user profile model binder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.Web.Infrastructure
{
    using System.Web.Mvc;

    using LMS.Web.Extensions;

    /// <summary>
    /// The user profile model binder.
    /// </summary>
    public class UserProfileModelBinder : IModelBinder
    {
        /// <summary>
        /// The bind model.
        /// </summary>
        /// <param name="controllerContext">
        /// The controller context.
        /// </param>
        /// <param name="bindingContext">
        /// The binding context.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            UserProfile user = controllerContext.RequestContext.HttpContext.Session.GetUserProfile();

            if (user == null)
            {
                user = new UserProfile();
                controllerContext.RequestContext.HttpContext.Session.SetUserProfile(user);
            }

            return user;
        }
    }
}