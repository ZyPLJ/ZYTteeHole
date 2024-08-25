using CodeLab.Share.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;
using ZYTreeHole_Models.ViewModels.Auth;
using ZYTreeHole_Services.Services;

namespace ZYTreeHole.Controllers.API;

[ApiController]
[Route("Api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService) {
        _authService = authService;
    }

    /// <summary>
    /// 登录
    /// </summary>
    [HttpPost]
    public async Task<ApiResponse> Login(LoginUser loginUser) {
        var user = await _authService.GetUserByName(loginUser.UserName);
        if (user == null) return ApiResponse.Unauthorized("用户名不存在");
        if (loginUser.Password != user.Password) return ApiResponse.Unauthorized("用户名或密码错误");
        return ApiResponse.Ok(_authService.GenerateLoginToken("1",user.UserName));
    }
}