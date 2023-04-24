using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Domain.Entities;
using UserManager.Domain.ValueObjects;

namespace UserManager.Infrastructure.EntityConfig
{
    internal class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //设置表名
            builder.ToTable("T_Users");
            //配置值类型
            builder.OwnsOne<PhoneNumber>(x => x.PhoneNumber, pn =>
            {
                pn.Property(x => x.RegionNumber).HasMaxLength(5).IsUnicode(false);
                pn.Property(x => x.Number).HasMaxLength(20).IsUnicode(false);
            });
            //设置私有变量
            builder.Property("PasswordHash").HasMaxLength(100).IsUnicode(false);
            //设置一对一关系
            builder.HasOne(x => x.UserAccessFail).WithOne(y => y.User).HasForeignKey<UserAccessFail>(z => z.UserId);


        }
    }
}
