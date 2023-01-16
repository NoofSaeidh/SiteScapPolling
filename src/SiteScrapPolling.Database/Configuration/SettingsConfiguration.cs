using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SiteScrapPolling.Database.Entities;

namespace SiteScrapPolling.Database.Configuration;

internal class SettingsConfiguration : IEntityTypeConfiguration<Settings>
{
    public void Configure(EntityTypeBuilder<Settings> builder)
    {
        builder.HasKey(s => new {s.Id, s.UserId});
        builder.HasOne(s => s.User).WithMany(u => u.Settings).HasForeignKey(s => s.UserId);
    }
}
