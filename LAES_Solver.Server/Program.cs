using LAES_Solver.Application.WebSocketServices;
using LAES_Solver.Application.WebSocketUtilities;
using LAES_Solver.Server.Extentios;
using Microsoft.Extensions.DependencyInjection;

namespace LAES_Solver.Server;

public class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddLaesSolver();
        using var serviceProvider = services.BuildServiceProvider();


        var server = serviceProvider.GetRequiredService<WebSocketServer>();
        var lltService = serviceProvider.GetRequiredService<LltSolverService>();

        server.RegisterService("/lltService", lltService);
        server.StartOn("http://localhost:5000/");

        Console.WriteLine("Press Enter to stop server");
        Console.ReadLine();
    }
}
