namespace UserManager.Domain;

//检查手机号验证码状态
public enum CheckCodeResult
{
    OK,
    PhoneNumberNotFound,
    LockOut,
    CodeError
}