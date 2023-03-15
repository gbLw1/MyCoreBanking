using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyCoreBanking.API.Data.Entities;

namespace MyCoreBanking.API.Data.Configurations;

internal sealed class TransacaoEntityTypeConfiguration : IEntityTypeConfiguration<Transacao>
{
    public void Configure(EntityTypeBuilder<Transacao> builder)
    {
        // TODO: Configure the entity
    }
}
