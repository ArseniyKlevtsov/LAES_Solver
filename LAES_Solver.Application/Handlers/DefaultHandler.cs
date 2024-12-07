using LAES_Solver.Domain.ValueObjects;

namespace LAES_Solver.Application.Handlers;

public static class DefaultHandler
{
    public static HandlerResponse Execute(Message message)
    {
        var resposeMessage = new Message("error", $"unknown command: ({message.Command})");
        var handlerResponse = new HandlerResponse()
        {
            SenderMessage = resposeMessage
        };
        return handlerResponse;
    }
}
