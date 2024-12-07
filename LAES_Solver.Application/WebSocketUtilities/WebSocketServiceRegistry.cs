using LAES_Solver.Application.WebSocketUtilities;
using System.Collections.Concurrent;

public class WebSocketServiceRegistry
{
    private readonly ConcurrentDictionary<string, WebSocketService> _handlers = new();

    public void RegisterService(string key, WebSocketService handler)
    {
        _handlers[key] = handler;
    }

    public WebSocketService? GetService(string key)
    {
        _handlers.TryGetValue(key, out var handler);
        return handler;
    }
}