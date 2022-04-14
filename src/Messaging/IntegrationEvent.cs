using System;

namespace Strada.Framework.Messaging
{
    /// <summary>
    /// Integration event
    /// </summary>
    public class IntegrationEvent
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets the creation date.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        public DateTimeOffset CreationDate { get; private set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the tenant namespace.
        /// </summary>
        /// <value>
        /// The tenant namespace.
        /// </value>
        public string TenantNamespace { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEvent"/> class.
        /// </summary>
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTimeOffset.Now;
            Version = "1.0.0";
        }
    }
}
