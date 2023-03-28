using MassTransit;

namespace MassTransitPersistentOutbox.Saga;

public class TestSagaState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }

    public string CurrentState { get; set; }

    public int Version { get; set; }
}