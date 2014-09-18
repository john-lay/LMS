using LMS.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Web.Extensions
{
    public static class HttpSessionStateBaseExtensions
    {
        public static void SetUserProfile(this HttpSessionStateBase session, UserProfile userProfile)
        {
            session["UserInfo"] = userProfile;
        }

        public static UserProfile GetUserProfile(this HttpSessionStateBase session)
        {
            return session["UserInfo"] as UserProfile;
        }
    }
}