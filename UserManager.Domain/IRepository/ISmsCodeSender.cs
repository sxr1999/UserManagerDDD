using UserManager.Domain.ValueObjects;

namespace UserManager.Domain.IRepository;

//短信发送接口
public interface ISmsCodeSender
{
    
    Task SendAsync(PhoneNumber phoneNumber,string code);
}