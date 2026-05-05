using CoreMesh.Dispatching.Extensions;
using CoreMesh.Validation.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationDependency
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(ApplicationDependency).Assembly;
        services.AddDispatching([assembly]);
        services.AddValidatable([assembly]);
        return services;
    }
}
