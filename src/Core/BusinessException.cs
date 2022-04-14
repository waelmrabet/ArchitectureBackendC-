using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Strada.Framework.Core
{
    [Serializable]
    /// <summary>
    /// Describes a business exception
    /// </summary>
    public class BusinessException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException" /> class.
        /// </summary>
        public BusinessException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public BusinessException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException" /> class.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        public BusinessException(string format, params object[] args)
            : base(string.Format(CultureInfo.InvariantCulture, format, args)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public BusinessException(string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException" /> class.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="args">The args.</param>
        public BusinessException(string format, Exception innerException, params object[] args)
            : base(string.Format(CultureInfo.InvariantCulture, format, args), innerException) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessException" /> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected BusinessException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
