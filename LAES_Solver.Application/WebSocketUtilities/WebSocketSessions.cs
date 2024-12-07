using LAES_Solver.Application.WebSocketUtilities;
using System.Collections.Concurrent;

public class WebSocketSessions
{
    private readonly ConcurrentDictionary<string, WebSocketConnection> _sessions = new();

    public void AddSession(string id, WebSocketConnection connection)
    {
        _sessions[id] = connection;
    }

    public void RemoveSession(string id)
    {
        _sessions.TryRemove(id, out _);
    }

    public WebSocketConnection? GetConnection(string id)
    {
        _sessions.TryGetValue(id, out var connection);
        return connection;
    }

    public void SendTo(string id, string message)
    {
        if (_sessions.TryGetValue(id, out var connection))
        {
            connection.SendMessageAsync(message).Wait();
        }
    }

    public void SendToMany(string[] ids, string message)
    {
        foreach (var id in ids)
        {
            SendTo(id, message);
        }
    }
}