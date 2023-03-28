using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using MassTransitPersistentOutbox.Saga;
using Microsoft.EntityFrameworkCore;

namespace MassTransitPersistentOutbox;

public class MassTransitDbContext : SagaDbContext
{
    public MassTransitDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get { yield return new TestSagaStateMap(); }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}