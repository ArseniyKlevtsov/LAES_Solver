using LAES_Solver.Application.WebSocketUtilities;
using LAES_Solver.Domain.Interfaces.Services;

namespace LAES_Solver.Application.WebSocketServices;

public class LltSolverService : WebSocketService
{
    private readonly ILoger _loger;

    public LltSolverService(ILoger loger)
    {
        _loger = loger;
    }

    protected override void OnOpen()
    {
        _loger.Log("BROO heloo");
        _loger.Log(Id);
    }

    protected override async void OnMessage(string message)
    {
        _loger.Log("BROO send this:");
        _loger.Log(message);

        await SendAsync($"I get this bro {message}");
        _loger.Log($"send to bro this:{message}");
    }

    protected override void OnClose()
    {
        _loger.Log("BROO poka");
        _loger.Log(Id);
    }

    protected override void OnError(Exception exception)
    {
        _loger.Log(exception.Message);
    }

}
