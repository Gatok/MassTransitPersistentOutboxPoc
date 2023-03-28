﻿using MassTransit;

namespace MassTransitPersistentOutbox.Saga.Events;

public sealed class FinalizeSagaEvent : CorrelatedBy<Guid>
{
    public Guid CorrelationId { get; set; }
}