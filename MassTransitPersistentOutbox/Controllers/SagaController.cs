using MassTransit;
using MassTransitPersistentOutbox.Saga.Events;
using Microsoft.AspNetCore.Mvc;

namespace MassTransitPersistentOutbox.Controllers;

[ApiController]
[Route("[controller]")]
public class SagaController : ControllerBase
{
    private readonly IPublishEndpoint publishEndpoint;
    private readonly MassTransitDbContext massTransitDbContext;

    public SagaController(IPublishEndpoint publishEndpoint, MassTransitDbContext massTransitDbContext)
    {
        this.publishEndpoint = publishEndpoint;
        this.massTransitDbContext = massTransitDbContext;
    }

    [HttpPost()]
    public async Task<IActionResult> StartSaga([FromBody] bool exception = false)
    {
        await this.publishEndpoint.Publish(new StartSagaEvent
        {
            CorrelationId = Guid.NewGuid(),
        });

        if (exception)
        {
            throw new Exception();
        }

        //saves message to outbox
        await this.massTransitDbContext.SaveChangesAsync();

        return Ok();
    }
}