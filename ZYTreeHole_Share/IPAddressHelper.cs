using Microsoft.AspNetCore.Http;

namespace ZYTreeHole_Share;

public class IPAddressHelper
{
    /// <summary>  
    /// 尝试从HttpContext中获取当前客户端的真实IP地址。  
    /// 如果使用了反向代理，则尝试从X-Forwarded-For或X-Real-IP头中获取。  
    /// </summary>  
    /// <param name="httpContext">当前的HttpContext对象。</param>  
    /// <returns>客户端的IP地址或null（如果无法确定）。</returns>  
    public static string GetCurrentClientIP(HttpContext httpContext)  
    {  
        // 首先检查X-Forwarded-For头，这在客户端通过HTTP代理或负载均衡器连接到服务器时很有用  
        string[] xForwardedForHeaders = httpContext.Request.Headers["X-Forwarded-For"].ToArray();  
        if (xForwardedForHeaders.Any())  
        {  
            // X-Forwarded-For可以包含多个IP地址（由逗号分隔），通常第一个是最接近客户端的IP  
            return xForwardedForHeaders.First().Split(',').First().Trim();  
        }  
  
        // 如果没有X-Forwarded-For头，则尝试使用X-Real-IP头  
        if (httpContext.Request.Headers.TryGetValue("X-Real-IP", out var xRealIpHeaders) && !string.IsNullOrEmpty(xRealIpHeaders.First()))  
        {  
            return xRealIpHeaders.First().Trim();  
        }  
  
        // 如果上述两个头都没有，则回退到连接信息中的RemoteIpAddress  
        // 注意：在Kestrel后面有反向代理时，RemoteIpAddress可能是代理的IP  
        var remoteIpAddress = httpContext.Connection.RemoteIpAddress;  
        return remoteIpAddress != null ? remoteIpAddress.ToString() : null;  
    }  
}