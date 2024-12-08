namespace LAES_Solver.Application.WebSocketUtilities;

public abstract class WebSocketService
{
    public string Id { get; private set; }
    protected WebSocketConnection Connection { get; private set; }
    protected static WebSocketSessions Sessions { get; private set; } = new WebSocketSessions();

    protected WebSocketService()
    {
        Id = Guid.NewGuid().ToString(); 
    }

    public void SetConnection(WebSocketConnection connection)
    {
        Connection = connection;
        Connection.OnMessage += OnMessage;
        Connection.OnOpen += OnOpen;
        Connection.OnError += OnError;
        Connection.OnClose += OnClose;

        Sessions.AddSession(Id, Connection);
    }

    protected abstract Task OnMessage(string message);
    protected abstract Task OnOpen();
    protected abstract Task OnClose();
    protected abstract Task OnError(Exception exception);

    protected async Task SendAsync(string message)
    {
        if (Connection != null)
        {
            await Connection.SendMessageAsync(message);
        }
    }
}
