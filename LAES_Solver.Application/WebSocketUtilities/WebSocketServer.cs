using LAES_Solver.Domain.Interfaces.Services;
using System.Net;

namespace LAES_Solver.Application.WebSocketUtilities;

public class WebSocketServer
{
    private readonly WebSocketServiceRegistry _serviceRegistry;
    private readonly HttpListener _httpListener;
    private readonly ILoger _loger;
    private string _url;

    public WebSocketServer(ILoger loger)
    {
        _httpListener = new HttpListener();
        _serviceRegistry = new WebSocketServiceRegistry();
        _loger = loger;
    }

    public void RegisterService(string key, WebSocketService handler)
    {
        _serviceRegistry.RegisterService(key, handler);
    }

    public void StartOn(string uriPrefix)
    {
        _httpListener.Prefixes.Add(uriPrefix);
        _httpListener.Start();
        _loger.Log($"server started on {uriPrefix}");

        Task.Run(async () => await AcceptConnectionsAsync());
    }

    private async Task AcceptConnectionsAsync()
    {
        while (true)
        {
            var context = await _httpListener.GetContextAsync();
            await ProcessRequestAsync(context);
        }
    }

    private async Task ProcessRequestAsync(HttpListenerContext context)
    {
        if (context.Request.IsWebSocketRequest)
        {
            await HandleWebSocketRequestAsync(context);
        }
        else
        {
            context.Response.StatusCode = 400;
            context.Response.Close();
        }
    }

    private async Task HandleWebSocketRequestAsync(HttpListenerContext context)
    {
        var webSocketContext = await context.AcceptWebSocketAsync(null);
        var connection = new WebSocketConnection(webSocketContext.WebSocket);

        string absolutePath = context.Request.Url.AbsolutePath;

        var service = _serviceRegistry.GetService(absolutePath);
        if (service != null)
        {
            service.SetConnection(connection);
            connection.Start();
        }
        else
        {
            await HandleUnknownHandlerAsync(connection);
        }
    }

    private async Task HandleUnknownHandlerAsync(WebSocketConnection connection)
    {
        await connection.CloseAsync();
    }
}
