using CodeLab.Share.ViewModels.Response;
using Microsoft.AspNetCore.SignalR;
using ZYTreeHole_Models.Entity;
using ZYTreeHole_Models.ViewModels.Requests;

namespace ZYTreeHole_Services.Services;

public class ChatHub:Hub
{
    private static int _connectionCount;
    public async Task SendComment(string content)  
    {  
        ZyComments comment = new ZyComments
        {
            Content = content
        };
        // 然后，广播评论给所有连接的客户端  
        await Clients.All.SendAsync("ReceiveComment", comment);
    }  
    public override Task OnConnectedAsync()
    {
        // 每次客户端连接时增加计数
        Interlocked.Increment(ref _connectionCount);
        return base.OnConnectedAsync();
    }
 
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        // 每次客户端断开连接时减少计数
        Interlocked.Decrement(ref _connectionCount);
        return base.OnDisconnectedAsync(exception);
    }
 
    public int GetConnectionCount()
    {
        // 获取当前连接数量
        return _connectionCount;
    }
}