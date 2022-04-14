using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Strada.Framework.Messaging
{
    /// <summary>
    /// Publisher interface
    /// </summary>
    public interface IPublisher
    {
        /// <summary>
        /// Publish a message to a specified topic asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topicName">Name of the topic.</param>
        /// <param name="message">The message.</param>
        /// <param name="callbackName">Callback subscriber name.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task PublishAsync<T>(string topicName, T message, string callbackName = null, CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// Publish a message to a specified topic asynchronous with custom headers.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="topicName">Name of the topic.</param>
        /// <param name="message">The message.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task PublishAsync<T>(string topicName, T message, IDictionary<string, string> headers, CancellationToken cancellationToken = default) where T : class;
    }
}
