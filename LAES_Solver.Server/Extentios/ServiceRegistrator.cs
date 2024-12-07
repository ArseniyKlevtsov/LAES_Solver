using LAES_Solver.Application.WebSocketServices;
using LAES_Solver.Application.WebSocketUtilities;
using LAES_Solver.Domain.Interfaces.Services;
using LAES_Solver.Infrastracture.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace LAES_Solver.Server.Extentios;

public static class ServiceRegistrator
{
    public static IServiceCollection AddLaesSolver(this IServiceCollection services)
    {
        services.AddTransient<WebSocketServer>();

        services.AddTransient<LltSolverService>();

        services.AddTransient<ILoger, ConsoleLoger>();

        return services;
    }
}
