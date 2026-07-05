using GymManagement.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.FluentConfigurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Ignore(X => X.Id);

            builder.HasOne(X => X.Session)
                   .WithMany(X => X.SessionMember)
                   .HasForeignKey(X => X.SessionId);

            builder.HasOne(X => X.Member)
                   .WithMany(X => X.MemberSession)
                   .HasForeignKey(X => X.MemberId);

            builder.HasKey(X => new { X.SessionId , X.MemberId });    //composite PK

            builder.Property(X=>X.CreatedAt)
                .HasColumnName("BookingDate")
                .HasDefaultValueSql("GETDATE()");
        }

    }
}