using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MassTransitPersistentOutbox.Saga;

public sealed class TestSagaStateMap : SagaClassMap<TestSagaState>
{
    protected override void Configure(EntityTypeBuilder<TestSagaState> entity, ModelBuilder model)
    {
        entity.Property(x => x.CorrelationId);
        entity.Property(x => x.CurrentState);
        entity.Property(x => x.Version);
    }
}