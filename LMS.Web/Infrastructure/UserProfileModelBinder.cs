using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMS.Web.Extensions;

namespace LMS.Web.Infrastructure
{
    public class UserProfileModelBinder : IModelBinder
    {
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