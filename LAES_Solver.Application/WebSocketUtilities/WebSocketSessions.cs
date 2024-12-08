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

    public async Task SendToAsync(string id, string message)
    {
        if (_sessions.TryGetValue(id, out var connection))
        {
            await connection.SendMessageAsync(message);
        }
    }

    public async Task SendToManyAsync(string[] ids, string message)
    {
        foreach (var id in ids)
        {
            await SendToAsync(id, message);
        }
    }
}