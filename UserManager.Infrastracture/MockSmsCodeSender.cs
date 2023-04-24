using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Domain.IRepository;
using UserManager.Domain.ValueObjects;

namespace UserManager.Infrastracture
{
    public class MockSmsCodeSender : ISmsCodeSender
    {
        public Task SendAsync(PhoneNumber phoneNumber, string code)
        {
            Console.WriteLine($"向手机号{phoneNumber.Number}发送验证码: {code}");
           return Task.CompletedTask;
        }
    }
}
