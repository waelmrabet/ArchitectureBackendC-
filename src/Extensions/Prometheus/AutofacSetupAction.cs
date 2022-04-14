using Microsoft.Extensions.Configuration;

namespace Autofac
{
    public delegate void AutofacSetupAction(ContainerBuilder builder, IConfiguration configuration);
}
