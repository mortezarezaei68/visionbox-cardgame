using CardGame.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardGame.Core.DomainConfigurations;

public class BoardDetailEntityConfiguration:IEntityTypeConfiguration<BoardDetail>
{
    public void Configure(EntityTypeBuilder<BoardDetail> builder)
    {
        builder.HasKey(a => a.Id);
    }
}