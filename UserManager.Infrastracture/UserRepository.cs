using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Domain.Entities;
using UserManager.Domain.IRepository;
using UserManager.Domain.ResultEvent;
using UserManager.Domain.ValueObjects;

namespace UserManager.Infrastracture
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _dbcontext;
        private readonly IDistributedCache _distributedCache;
        private readonly IMediator _mediator;


        public UserRepository(ApplicationDbContext dbcontext,IDistributedCache distributedCache, IMediator mediator)
        {
            _dbcontext = dbcontext;
            _distributedCache = distributedCache;
            _mediator = mediator;
        }

        //添加登录信息
        public async Task AddNewLoginHistoryAsync(PhoneNumber phoneNumber, string msg)
        {
            var user = await FindOneAsync(phoneNumber);
            Guid userid ;
            if (user != null)
            {
                userid = user.Id;
                await _dbcontext.userLoginHistories.AddAsync(new UserLoginHistory(userid,phoneNumber,msg));
            }
        }

        //根据手机号查找用户
        public async Task<User?> FindOneAsync(PhoneNumber phoneNumber)
        {
            if(phoneNumber != null)
            {
                var user = await _dbcontext.users.Where(x => (x.PhoneNumber.RegionNumber==phoneNumber.RegionNumber) && (x.PhoneNumber.Number == phoneNumber.Number)).FirstOrDefaultAsync();
                return user;
            }

            return null;
        }


        //根据userid查找用户
        public async Task<User?> FindOneAsync(Guid userId)
        {
            var user = await _dbcontext.users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            return user;
        }

        //获取手机验证码
        public async Task<string?> FindPhoneNumberCodeAsync(PhoneNumber phoneNumber)
        {
            var key = phoneNumber.RegionNumber + phoneNumber.Number;
            var code = await _distributedCache.GetStringAsync(key);
            await _distributedCache.RemoveAsync(key);
            return code;
        }

        //发布领域事件
        public Task PublishEventAsync(UserAccessResultEvent userAccessResultEvent)
        {
            return _mediator.Publish(userAccessResultEvent);
        }


        //当方法中只是调用一个异步方法时，不使用async更优
        public  Task SavePhoneNumberCodeAsync(PhoneNumber phoneNumber, string code)
        {
            //在分布式缓存中保存手机号验证码
            var key = phoneNumber.RegionNumber + phoneNumber.Number;
            //存入缓存中，绝对过期时间五分钟
            return _distributedCache.SetStringAsync(key, code, new DistributedCacheEntryOptions() {AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)});
        }
    }
}
