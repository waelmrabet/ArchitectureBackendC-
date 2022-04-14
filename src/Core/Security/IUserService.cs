using System.Security.Principal;

namespace Strada.Framework.Core.Security
{
    /// <summary>
    /// User service interface
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <value>
        /// The current user.
        /// </value>
        IPrincipal Current { get; }

        /// <summary>
        /// Gets the subject identifier.
        /// </summary>
        /// <returns></returns>
        string GetSubjectId();

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <returns></returns>
        string GetName();

        /// <summary>
        /// Gets the email.
        /// </summary>
        /// <returns></returns>
        string GetEmail();
    }
}
