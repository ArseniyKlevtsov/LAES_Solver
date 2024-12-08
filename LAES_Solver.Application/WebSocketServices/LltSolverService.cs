using LAES_Solver.Application.Handlers;
using LAES_Solver.Application.WebSocketUtilities;
using LAES_Solver.Domain.Interfaces.Services;
using LAES_Solver.Domain.ValueObjects;

namespace LAES_Solver.Application.WebSocketServices;

public class LltSolverService : WebSocketService
{
    private readonly ILoger _loger;
    private readonly IDtoConvertor _dtoConvertor;
    private readonly MainHandler _mainHandler;

    public LltSolverService(ILoger loger, MainHandler mainHandler, IDtoConvertor dtoConvertor)
    {
        _loger = loger;
        _mainHandler = mainHandler;
        _dtoConvertor = dtoConvertor;
    }

    protected override async Task OnOpen()
    {
        _loger.Log($"CONNECTED: {Id}:");
    }

    protected override async Task OnMessage(string jsonMessage)
    {
        var message = _dtoConvertor.ConvertToDto<Message>(jsonMessage);
        _loger.Log($"RECEIVE from {Id}: {message.Command}");
        var handlerResponse =  await _mainHandler.Route(message, Id);
        await SendHandlerResponse(handlerResponse);
    }

    protected override async Task OnClose()
    {
        _loger.Log($"DISCONNECTED: {Id}:");
    }

    protected override async Task OnError(Exception exception)
    {
        _loger.Log($"EXCEPTION");
        _loger.Log(exception.Message);
    }

    private async Task SendMessageAsync(Message message)
    {
        string jsonMessage = _dtoConvertor.ConvertToJson(message);
        await SendAsync(jsonMessage);
        _loger.Log($"SEND to {Id}: {message.Command}");
    }

    private async Task SendMessageToAsync(Message message, string id)
    {
        string jsonMessage = _dtoConvertor.ConvertToJson(message);
        await Sessions.SendToAsync(jsonMessage, id);
        _loger.Log($"SEND to {id}: {message.Command}");
    }

    private async Task SendHandlerResponse(HandlerResponse response)
    {

        if(response.SenderMessage != null)
        {
            await SendMessageAsync(response.SenderMessage);
        }
        if (response.RecipientIds.Count > 0)
        {
            foreach (var recipientId in response.RecipientIds)
            {
                await SendMessageToAsync(response.RecipientMessage!, recipientId);
            }
        }
    }

}
