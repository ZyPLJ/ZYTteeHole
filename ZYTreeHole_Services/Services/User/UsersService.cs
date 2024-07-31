using ZYTreeHole_Models;
using ZYTreeHole_Models.Entity;
using ZYTreeHole_Models.ViewModels.Requests;

namespace ZYTreeHole_Services.Services.User;

public class UsersService : IUsersService
{
    private readonly MyDbContext _myDbContext;

    public UsersService(MyDbContext myDbContext)
    {
        _myDbContext = myDbContext;
    }

    public async Task<ZyUsers> GetUserOrAddAsync(UserDto userDto)
    {
        //先查询用户是否存在;
        var user = _myDbContext.users.FirstOrDefault(u => u.Email == userDto.Email || u.IpAddress == userDto.IpAddress);
        //如果存在，则直接返回用户信息
        if (user != null)
        {
            return await Task.FromResult(user);
        }

        //如果不存在，则新增用户信息并返回
        var newUser = new ZyUsers
        {
            Email = userDto.Email,
            IpAddress = userDto.IpAddress,
            CreateTime = DateTime.Now
        };
        await _myDbContext.users.AddAsync(newUser);
        await _myDbContext.SaveChangesAsync();
        return newUser;
    }
}