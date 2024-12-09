using LAES_Solver.Application.Handlers;
using LAES_Solver.Application.Services;
using LAES_Solver.Application.WebSocketServices;
using LAES_Solver.Application.WebSocketUtilities;
using LAES_Solver.Domain.Interfaces;
using LAES_Solver.Domain.Interfaces.Services;
using LAES_Solver.Infrastracture.Logging;
using LAES_Solver.Infrastracture.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LAES_Solver.Server.Extentios;

public static class ServiceRegistrator
{
    public static IServiceCollection AddLaesSolver(this IServiceCollection services)
    {
        services.AddTransient<WebSocketServer>();

        services.AddTransient<LltSolverService>();

        //handlers
        services.AddTransient<MainHandler>();

        //services
        services.AddTransient<LltService>();
        services.AddTransient<LltTaskService>();
        services.AddTransient<ILoger, ConsoleLoger>();
        services.AddTransient<IDtoConvertor, DtoConvertor>();
        services.AddTransient<IMatrixFileService>(provider =>
        {
            string baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "MatrixData");
            return new MatrixFileService(baseDirectory);
        });



        return services;
    }
}
