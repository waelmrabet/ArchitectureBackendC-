using System;
using System.Collections.Generic;
using System.Text;

namespace Strada.Framework.Persistence
{
    /// <summary>
    /// Database connection info
    /// </summary>
    public class DbConnectionInfo
    {
        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port ; default  = "5432".
        /// </summary>
        /// <value>
        /// The port ; default  = "5432".
        /// </value>
        public string Port { get; set; }

        /// <summary>
        /// Gets or sets the database.
        /// </summary>
        /// <value>
        /// The database.
        /// </value>
        public string Database { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if(string.IsNullOrWhiteSpace(Port))
                return $"Host={Host};Database={Database};Username={Username};Password={Password};";
            else
                return $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password};";
        }
    }
}
