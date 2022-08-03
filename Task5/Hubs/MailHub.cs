using Microsoft.AspNetCore.SignalR;
namespace Task5.Hubs;

public class MailHub:Hub
{
    public static readonly Dictionary<string, int> ConnectedUsers = new();
    
    public void Login(int userId)
    {
        var id = Context.ConnectionId;
        ConnectedUsers.Add(id, userId);
    }
    
    public override Task OnDisconnectedAsync(Exception? exception)  
    {
        ConnectedUsers.Remove(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);  
    }
}