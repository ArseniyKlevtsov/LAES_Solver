using System.Net.WebSockets;
using System.Text;

namespace LAES_Solver.Application.WebSocketUtilities;

public class WebSocketConnection
{
    private readonly WebSocket _webSocket;

    public event Action<string> OnMessage;
    public event Action OnOpen;
    public event Action OnClose;
    public event Action<Exception> OnError;

    public WebSocketConnection(WebSocket webSocket)
    {
        _webSocket = webSocket;
        OnOpen?.Invoke();
    }

    public async void Start()
    {
        while (_webSocket.State == WebSocketState.Open)
        {
            try
            {
                var buffer = new byte[1024];
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                OnMessage?.Invoke(message);
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex);
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
        OnClose?.Invoke();
    }
}