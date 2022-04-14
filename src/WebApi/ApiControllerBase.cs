using MediatR;

namespace Strada.Framework.WebApi
{
    /// <summary>
    /// Api controller base
    /// </summary>
    public class ApiControllerBase
    {
        /// <summary>
        /// The mediator
        /// </summary>
        protected readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiControllerBase"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public ApiControllerBase(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
