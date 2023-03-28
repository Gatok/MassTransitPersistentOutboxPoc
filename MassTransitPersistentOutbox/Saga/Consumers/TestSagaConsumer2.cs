using MassTransit;
using MassTransitPersistentOutbox.Saga.Events;

namespace MassTransitPersistentOutbox.Saga.Consumers;

public sealed class TestSagaConsumer2 : IConsumer<ProgressSagaEvent>
{
    public async Task Consume(ConsumeContext<ProgressSagaEvent> context)
    {
        Console.WriteLine($"TestSagaConsumer2 for {context.Message.CorrelationId}");
        await Task.Delay(10000);

        await context.Publish(new FinalizeSagaEvent
        {
            CorrelationId = context.Message.CorrelationId,
        });
    }
}