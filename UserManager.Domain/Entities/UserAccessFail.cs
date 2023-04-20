using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManager.Domain.Entities
{
    public class UserAccessFail
    {
        public Guid Id { get; init; }
        //用户的Id
        public Guid UserId { get; init; }
        //用户
        public User User { get; init; }
        //是否被锁定
        private bool LockOut;
        //被锁定时间
        public DateTime? LockoutEnd { get; private set; }
        //登陆错误的次数
        public int AccessFailedCount { get; private set; }
        //给ef core使用的私有无参构构造函数
        private UserAccessFail() { }
        //给程序员使用的构造函数
        public UserAccessFail(User user)
        {
            Id = Guid.NewGuid();
            UserId = user.Id;
            User = user;
        }
        //重置账号
        public void Reset()
        {
            LockOut = false;
            AccessFailedCount = 0;
            LockoutEnd = null;
        }
        //登陆失败时的方法
        public void Fail()
        {
            AccessFailedCount++;
            //当登录三次失败时锁定账号,锁定时间为5分钟
            if (AccessFailedCount >= 3)
            {
                LockOut = true;
                LockoutEnd = DateTime.Now.AddMinutes(5);
            }
        }
        //检查账号是否已被锁定
        public bool IsLockOut()
        {
            if (LockOut)
            {
                //当锁定时间大于当前时间时，账号处于被锁定状态
                if (LockoutEnd >= DateTime.Now)
                {
                    return true;
                }
                else
                {
                    //当锁定时间小于当前时间时，账号锁定时间到期
                    Reset();
                    return false;

                }

            }

            return false;
        }

    }
}
