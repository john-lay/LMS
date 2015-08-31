// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpSessionStateBaseExtensions.cs" company="">
//   
// </copyright>
// <summary>
//   The http session state base extensions.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace LMS.Web.Extensions
{
    using System.Web;

    using LMS.Web.Infrastructure;

    /// <summary>
    /// The http session state base extensions.
    /// </summary>
    public static class HttpSessionStateBaseExtensions
    {
        /// <summary>
        /// The get user profile.
        /// </summary>
        /// <param name="session">
        /// The session.
        /// </param>
        /// <returns>
        /// The <see cref="UserProfile"/>.
        /// </returns>
        public static UserProfile GetUserProfile(this HttpSessionStateBase session)
        {
            return session["UserInfo"] as UserProfile;
        }

        /// <summary>
        /// The get user token.
        /// </summary>
        /// <param name="session">
        /// The session.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetUserToken(this HttpSessionStateBase session)
        {
            return session["UserToken"] as string;
        }

        /// <summary>
        /// The set user profile.
        /// </summary>
        /// <param name="session">
        /// The session.
        /// </param>
        /// <param name="userProfile">
        /// The user profile.
        /// </param>
        public static void SetUserProfile(this HttpSessionStateBase session, UserProfile userProfile)
        {
            session["UserInfo"] = userProfile;
        }

        /// <summary>
        /// The set user token.
        /// </summary>
        /// <param name="session">
        /// The session.
        /// </param>
        /// <param name="token">
        /// The token.
        /// </param>
        public static void SetUserToken(this HttpSessionStateBase session, string token)
        {
            session["UserToken"] = token;
        }
    }
}