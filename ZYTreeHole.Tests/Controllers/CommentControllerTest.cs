using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using ZYTreeHole_Models.ViewModels.Requests;
using ZYTreeHole.Controllers;

namespace ZYTreeHole.Tests.Controllers;

[TestSubject(typeof(CommentController))]
public class CommentControllerTest:IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public CommentControllerTest(WebApplicationFactory<Program> factory) {
        _factory = factory;
    }

    [Fact] // 使用Fact而不是Theory，因为这里我们不需要测试多个输入  
    public async Task Post_Comment_ReturnsSuccessAndCorrectContentType()  
    {  
        var client = _factory.CreateClient();
        // 创建一个CommentDto实例  
        var commentDto = new CommentDto  
        { 
            ParentId = null,  
            Content = "集成测试评论",  
            Email = "test@example.com",  
            IsNeedAudit = true,  
            Visible = true  
        };  
        string url = "/Comment";
        for (int i = 0; i < 6; i++)
        {
            // 使用HttpClient发送POST请求  
            var response = await client.PostAsJsonAsync(url, commentDto);  
            // 确保请求成功  
            response.EnsureSuccessStatusCode();  
            // 如果需要，验证响应内容类型（这取决于你的API如何配置）  
            // 假设你的API返回JSON，但不一定包含特定的版本信息  
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString()); 
        }
  
        // 如果需要，还可以验证响应体内容  
        // 例如，如果API返回了创建的CommentDto，你可以这样验证  
        //var createdComment = await response.Content.ReadFromJsonAsync<CommentDto>();  ·
        // // 然后添加对createdComment的断言  
    }

    [Fact]
    public async Task Delete_Comment_ReturnsSuccess()
    {
        var client = _factory.CreateClient();
        string url = "/Comment/44";
        //发起DELETE请求，删除id为44的评论
        var response = await client.DeleteAsync(url);
        // 确保请求成功  
        response.EnsureSuccessStatusCode();  
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString()); 
    }

    [Fact]
    public async Task Accept_Comment_ReturnsSuccess()
    {
        var client = _factory.CreateClient();
        CommentAcceptDto commentAcceptDto = new CommentAcceptDto()
        {
            Reason = "测试通过"
        };
        string url = "/Comment/1/Accept";
        //发起POST请求，接受id为1的评论
        var response = await client.PostAsJsonAsync(url, commentAcceptDto);  
        response.EnsureSuccessStatusCode();  
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString()); 
    }

    [Fact]
    public async Task Reject_Comment_ReturnsSuccess()
    {
        var client = _factory.CreateClient();
        CommentRejectDto commentRejectDto = new CommentRejectDto()
        {
            Reason = "测试拒绝"
        };
        string url = "/Comment/22/Reject";
        var response = await client.PostAsJsonAsync(url, commentRejectDto);  
        response.EnsureSuccessStatusCode();  
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString()); 
    }
}