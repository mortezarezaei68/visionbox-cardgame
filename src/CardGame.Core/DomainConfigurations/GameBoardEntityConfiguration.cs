using CardGame.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardGame.Core.DomainConfigurations;

public class GameBoardEntityConfiguration:IEntityTypeConfiguration<GameBoard>
{
    public void Configure(EntityTypeBuilder<GameBoard> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.FullName).HasMaxLength(200);
        builder.HasMany(a => a.BoardDetails);
    }
}