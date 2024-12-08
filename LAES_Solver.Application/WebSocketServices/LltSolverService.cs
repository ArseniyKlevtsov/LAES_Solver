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
        _loger.Log("BROO heloo");
        _loger.Log(Id);
    }

    protected override async void OnMessage(string jsonMessage)
    {
        _loger.Log("MESSAGE");
        _loger.Log(jsonMessage);
        var message = _dtoConvertor.ConvertToDto<Message>(jsonMessage);
        _loger.Log(message.Command);
    }

    protected override void OnClose()
    {
        
    }

    protected override void OnError(Exception exception)
    {
        _loger.Log(exception.Message);
    }

    private async void SendMessageAsync(Message message)
    {
        string jsonMessage = _dtoConvertor.ConvertToJson(message);
        await SendAsync(jsonMessage);
        _loger.Log($"send to {Id}: {message.Command}");
    }

    private void SendMessageAsync(Message message, string id)
    {
        string jsonMessage = _dtoConvertor.ConvertToJson(message);
        Sessions.SendTo(jsonMessage, id);
        _loger.Log($"send to {id}: {message.Command}");
    }

    private async void SendHandlerResponse(HandlerResponse response)
    {

        if(response.SenderMessage != null)
        {
            SendMessageAsync(response.RecipientMessage);
        }
        if (response.RecipientIds.Count > 0)
        {
            foreach (var recipientId in response.RecipientIds)
            {
                SendMessageAsync(response.RecipientMessage, recipientId);
            }
        }
    }

}
