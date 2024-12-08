using System.Net.WebSockets;
using System.Text;

namespace LAES_Solver.Application.WebSocketUtilities;

public class WebSocketConnection
{
    private readonly WebSocket _webSocket;

    public event Func<string, Task> OnMessage;
    public event Func<Task> OnOpen;
    public event Func<Task> OnClose;
    public event Func<Exception, Task> OnError;

    public WebSocketConnection(WebSocket webSocket)
    {
        _webSocket = webSocket;
         
    }

    public async Task Start()
    {
        if (OnOpen != null)
        {
            await OnOpen.Invoke();
        }
        while (_webSocket.State == WebSocketState.Open)
        {
            try
            {
                var buffer = new byte[1024];
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                if (OnMessage != null)
                {
                    await OnMessage.Invoke(message);
                }
            }
            catch (Exception ex)
            {
                if (OnError != null)
                {
                    await OnError.Invoke(ex);
                }
            }
        }
        await CloseAsync();
    }

    public async Task SendMessageAsync(string message)
    {
        if (_webSocket.State == WebSocketState.Open)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

    public async Task CloseAsync()
    {
        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closure", CancellationToken.None);
        if (OnClose != null)
        {
            await OnClose.Invoke();
        }
    }
}