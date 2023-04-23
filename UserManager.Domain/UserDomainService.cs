using System.Formats.Asn1;
using UserManager.Domain.Entities;
using UserManager.Domain.IRepository;
using UserManager.Domain.ResultEvent;
using UserManager.Domain.ValueObjects;

namespace UserManager.Domain;

public class UserDomainService
{
    private readonly IUserRepository _userRepository;
    private readonly ISmsCodeSender _codeSender;

    public UserDomainService(IUserRepository userRepository,ISmsCodeSender smsCodeSender)
    {
        _userRepository = userRepository;
        _codeSender = smsCodeSender;
    }
    
    //检查手机号密码是否正确
    public async Task<UserAccessResult> CheckLoginAsync(PhoneNumber phoneNumber,string passWord)
    {
        User? user = await _userRepository.FindOneAsync(phoneNumber);
        UserAccessResult userAccessResult;
        //用户为空
        if (user==null)
        {
            userAccessResult = UserAccessResult.PhoneNumberNotFound;
        }
        //查看是否被锁定
        else if (IsLockOut(user))
        {
            userAccessResult =  UserAccessResult.LockOut;
        }
        //查看是否设计了密码
        else if (user.HasPassWord())
        {
            userAccessResult = UserAccessResult.NoPassword;
        }
        //查看密码是否正确
        else if (user.CheckPassword(passWord))
        {
            userAccessResult = UserAccessResult.Ok;
        }
        //密码错误
        else
        {
            userAccessResult = UserAccessResult.PasswordError;
        }

        if (user != null)
        {
            
            if (userAccessResult == UserAccessResult.Ok)
            {
                //重新设施用户状态
                ResetAccessFail(user);
            }
            else
            {
                //记录录用登录失败信息
                AccessFail(user);
            }
        }

        UserAccessResultEvent resultEvent = new UserAccessResultEvent(phoneNumber, userAccessResult);
        await _userRepository.PublishEventAsync(resultEvent);
        return userAccessResult;
    }
    
    
    //检查手机号验证码是否正确
    public async Task<CheckCodeResult> CheckCodeAsync(PhoneNumber phoneNumber,string code)
    {
        var user = await _userRepository.FindOneAsync(phoneNumber);
        
        if (user==null)
        {
           return  CheckCodeResult.PhoneNumberNotFound;
        }
        
        if (IsLockOut(user))
        {
           return  CheckCodeResult.LockOut;
        }
        
        //查询服务端验证码
        var codeInServer = await _userRepository.FindPhoneNumberCodeAsync(phoneNumber);

        //验证码匹配成功，则返回OK
        if (code == codeInServer)
        {
            return  CheckCodeResult.OK;
        }
        
        //如果服务器未找到验证码或验证码不匹配，则返回验证码错误
        AccessFail(user);
        return  CheckCodeResult.CodeError;
        
        
    }

    //重设用户状态
    private static void ResetAccessFail(User user)
    {
        user.UserAccessFail.Reset();
    }
    
    //判断用户是否已经被锁定
    private static bool IsLockOut(User user)
    {
        return user.UserAccessFail.IsLockOut();
    }
    
    //设置用户登录失败
    private static void AccessFail(User user)
    {
        user.UserAccessFail.Fail();
    }
}