using MassTransit;

namespace MassTransitPersistentOutbox.Saga.Events;

public sealed class ProgressSagaEvent : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
}