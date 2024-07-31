using ZYTreeHole_Models.Entity;
using ZYTreeHole_Models.ViewModels.Requests;

namespace ZYTreeHole_Services.Services.User;

public interface IUsersService
{
    //查询或者新增用户信息
    
    Task<ZyUsers> GetUserOrAddAsync(UserDto userDto);
}