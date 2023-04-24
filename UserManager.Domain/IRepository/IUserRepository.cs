using UserManager.Domain.Entities;
using UserManager.Domain.ResultEvent;
using UserManager.Domain.ValueObjects;

namespace UserManager.Domain.IRepository;

public interface IUserRepository
{
    //根据手机号查找用户
    Task<User?> FindOneAsync(PhoneNumber phoneNumber);
    //根据用户id查找用户
    Task<User?> FindOneAsync(Guid userId);
    //添加用户登录记录
    Task AddNewLoginHistoryAsync(PhoneNumber phoneNumber,string msg);
    //添加手机验证码
    Task SavePhoneNumberCodeAsync(PhoneNumber phoneNumber, string code);
    //查找手机验证码
    Task<string?> FindPhoneNumberCodeAsync(PhoneNumber phoneNumber);
    //发布领域事件
    Task PublishEventAsync(UserAccessResultEvent userAccessResultEvent);

}