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
    internal class UserLoginHistoryConfig : IEntityTypeConfiguration<UserLoginHistory>
    {
        public void Configure(EntityTypeBuilder<UserLoginHistory> builder)
        {
            builder.ToTable("T_UserLoginHistorys");
            //配置值类型
            builder.OwnsOne<PhoneNumber>(x => x.PhoneNumber, pn =>
            {
                pn.Property(x => x.RegionNumber).HasMaxLength(5).IsUnicode(false);
                pn.Property(x => x.Number).HasMaxLength(20).IsUnicode(false);
            });
        }
    }
}
