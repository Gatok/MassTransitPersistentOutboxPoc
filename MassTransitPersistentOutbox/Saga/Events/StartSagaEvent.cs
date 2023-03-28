using MassTransit;

namespace MassTransitPersistentOutbox.Saga.Events;

public sealed class StartSagaEvent : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
}