using CardGame.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardGame.Core.DomainConfigurations;

public class GameEntityConfiguration:IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(a => a.Id);
        builder.HasMany(a => a.GivenCards);
    }
}