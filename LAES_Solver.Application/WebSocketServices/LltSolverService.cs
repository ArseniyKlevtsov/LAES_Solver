using LAES_Solver.Application.DTOs.ResponseDTOs;
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

    protected override void OnOpen()
    {
        _loger.Log($"connected: {Id}:");
    }

    protected override async void OnMessage(string jsonMessage)
    {
        var message = _dtoConvertor.ConvertToDto<Message>(jsonMessage);
        _loger.Log($"receive from {Id}: {message.Command}");
        Console.WriteLine(jsonMessage);
        var handlerResponse = _mainHandler.Route(message, Id);
        await SendHandlerResponse(handlerResponse);
    }

    protected override void OnClose()
    {
        _loger.Log($"disconnected: {Id}:");
    }

    protected override void OnError(Exception exception)
    {
        _loger.Log($"error occurred while processing the request");
        _loger.Log(exception.Message);
    }

    private async Task SendMessageAsync(Message message)
    {
        string jsonMessage = _dtoConvertor.ConvertToJson(message);
        await SendAsync(jsonMessage);
        _loger.Log($"send to {Id}: {message.Command}");
    }

    private async Task SendMessageToAsync(Message message, string id)
    {
        string jsonMessage = _dtoConvertor.ConvertToJson(message);
        await Sessions.SendToAsync(jsonMessage, id);
        _loger.Log($"send to {id}: {message.Command}");
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
