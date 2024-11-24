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
    public class UserRoleEntityConfiguration : IEntityTypeConfiguration<UserRoleEntity>
    {
        public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
        {
            builder.ToTable("useroperationclaims");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.UserId).IsRequired();
            builder.Property(u => u.OperationClaimId).IsRequired();
            builder.HasOne(p => p.Role).WithMany(c => c.UserRole).HasForeignKey(p => p.OperationClaimId);
        }
    }
}
