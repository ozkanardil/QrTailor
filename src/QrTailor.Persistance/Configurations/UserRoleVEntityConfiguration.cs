using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QrTailor.Domain.Entities;

namespace QrTailor.Persistance.Configurations
{
    public class UserRoleVEntityConfiguration : IEntityTypeConfiguration<UserRoleVEntity>
    {
        public void Configure(EntityTypeBuilder<UserRoleVEntity> builder)
        {
            builder.ToTable("vuserclaims");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.UserId).IsRequired();
            builder.Property(u => u.ClaimId).IsRequired();
            builder.Property(r => r.OperationName).IsRequired().HasMaxLength(50);

        }
    }
}
