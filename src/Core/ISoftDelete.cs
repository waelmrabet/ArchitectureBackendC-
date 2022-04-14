﻿using System;

namespace Strada.Framework.Core
{
    /// <summary>
    /// Interface to manage soft delete in database
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// Gets or sets a value indicating whether this entity is deleted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if is deleted; otherwise, <c>false</c>.
        /// </value>
        bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets the deleted date.
        /// </summary>
        /// <value>
        /// The deleted date.
        /// </value>
        DateTimeOffset? DeletedDate { get; set; }
    }
}
