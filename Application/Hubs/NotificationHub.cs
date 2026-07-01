using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

//[Authorize] 
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userRole = Context.User?.FindFirst(ClaimTypes.Role)?.Value;

        if (userRole == "Producer")
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "ProducersGroup");
        }
        else if (userRole == "Customer")
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "CustomersGroup");
        }
        else if (userRole == "Designer")
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "DesignersGroup");
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
         
        var userRole = Context.User?.FindFirst(ClaimTypes.Role)?.Value;
        if (userRole == "Producer")
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "ProducersGroup");
        }
        else if (userRole == "Customer")
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "CustomersGroup");
        }
        else if (userRole == "Designer")
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "DesignersGroup");
        }

        await base.OnDisconnectedAsync(exception);
    }
}