using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UserManager.Domain.ValueObjects;
using Zack.Commons;

namespace UserManager.Domain.Entities
{
    public record User
    {
        public Guid Id { get; init; }
        public PhoneNumber  PhoneNumber { get; private set; }
        //hash加密密码
        private string? PasswordHash;
        public UserAccessFail UserAccessFail { get; private set; }
        private User() { }
        public User(PhoneNumber phoneNumber)
        {
            Id = Guid.NewGuid();
            PhoneNumber = phoneNumber;
            UserAccessFail = new UserAccessFail(this);
        }

        //user 是否设置了密码
        public bool HasPassWord()
        {
            return !string.IsNullOrEmpty(PasswordHash);
        }

        //修改密码
        public void ChangePassword(string value)
        {
            if (value.Length < 3)
            {
                throw new ArgumentOutOfRangeException("密码长度小于3");
            }
            this.PasswordHash = HashHelper.ComputeMd5Hash(value);
        }

        //检查密码是否正确
        public bool CheckPassword(string passWord)
        {
            return this.PasswordHash == HashHelper.ComputeMd5Hash(passWord); 
        }

        //修改手机号
        public void ChangePhoneNumber(PhoneNumber phoneNumber)
        {
            this.PhoneNumber = phoneNumber;
        }
    }
}
