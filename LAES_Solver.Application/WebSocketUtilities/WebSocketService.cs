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

    protected abstract void OnMessage(string message);
    protected abstract void OnOpen();
    protected abstract void OnClose();
    protected abstract void OnError(Exception exception);

    protected async Task SendAsync(string message)
    {
        if (Connection != null)
        {
            await Connection.SendMessageAsync(message);
        }
    }
}
