using LAES_Solver.Domain.Interfaces.Services;

namespace LAES_Solver.Infrastracture.Logging;

public class ConsoleLoger : ILoger
{
    public void Log(string message)
    {
        Console.WriteLine(message);
    }
}
