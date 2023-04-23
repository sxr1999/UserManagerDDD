namespace UserManager.Domain;

public enum UserAccessResult
{
    //登录成功
    Ok,
    //手机号不存在
    PhoneNumberNotFound,
    //锁定
    LockOut,
    //没有密码
    NoPassword,
    //密码错误
    PasswordError
}