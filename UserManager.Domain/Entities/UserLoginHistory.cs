using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Domain.ValueObjects;

namespace UserManager.Domain.Entities
{
    public record UserLoginHistory : IAggregateRoot
    {
        public Guid Id { get; init; }
        //用户Id,在此并没有像EF core 一对一关系那样再添加一个User实体，
        //此处的UserId仅仅是一个概念上的外键，并不是一个实际的数据库中的外键
        //User和UserLoginHistory为两个聚合根，他们只通过id关联
        public Guid? UserId { get; init; }
        //手机号
        public PhoneNumber PhoneNumber { get; init; }
        //登录时间
        public DateTime CreateDateTime { get; init; }
        //消息
        public string Message { get; init; }
        //ef core 使用私有无参构造函数
        private UserLoginHistory() { }

        public UserLoginHistory(Guid? UserId,PhoneNumber phoneNumber,string message)
        {
            Id = Guid.NewGuid();
            this.UserId = UserId;
            this.PhoneNumber = phoneNumber;
            this.CreateDateTime = DateTime.Now;
            this.Message = message;
        }
    }
}
