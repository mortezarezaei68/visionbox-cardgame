using CardGame.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardGame.Core.DomainConfigurations;

public class GivenCardEntityConfiguration:IEntityTypeConfiguration<GivenCard>
{
    public void Configure(EntityTypeBuilder<GivenCard> builder)
    {
        builder.HasKey(a => a.Id);
    }
}