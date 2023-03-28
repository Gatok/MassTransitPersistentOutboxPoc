using MassTransit;
using MassTransitPersistentOutbox.Saga;
using MassTransitPersistentOutbox.Saga.Consumers;
using MassTransitPersistentOutbox.Saga.Filters;
using Microsoft.EntityFrameworkCore;

namespace MassTransitPersistentOutbox;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddDbContext<MassTransitDbContext>(x =>
        {
            x.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
        });

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddMassTransit(cfg =>
        {
            cfg.AddSagaStateMachine<TestSagaStateMachine, TestSagaState>()
                .EntityFrameworkRepository(r =>
                {
                    r.ExistingDbContext<MassTransitDbContext>();
                    r.UseSqlServer();
                });

            cfg.AddEntityFrameworkOutbox<MassTransitDbContext>(options =>
            {
                options.UseSqlServer();
                options.UseBusOutbox();
            });

            cfg.AddConsumer<TestSagaConsumer1>();
            cfg.AddConsumer<TestSagaConsumer2>();

            cfg.UsingRabbitMq((context, opt) =>
            {
                opt.Host(builder.Configuration["RabbitMQ:Uri"], config =>
                {
                    config.Username(builder.Configuration["RabbitMQ:Username"]);
                    config.Password(builder.Configuration["RabbitMQ:Password"]);
                });

                //opt.UsePublishFilter(typeof(TestFilter<>), context);

                opt.UseMessageRetry(r => r.Intervals(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10), TimeSpan.FromMinutes(1)));

                opt.ReceiveEndpoint("TestSaga", config =>
                {
                    config.StateMachineSaga<TestSagaState>(context.GetService<IServiceProvider>());
                    config.SetQuorumQueue();
                    config.UseEntityFrameworkOutbox<MassTransitDbContext>(context);
                });

                opt.ReceiveEndpoint("TestSagaConsumer1", config =>
                {
                    config.ConfigureConsumer<TestSagaConsumer1>(context);
                    config.SetQuorumQueue();
                    config.UseEntityFrameworkOutbox<MassTransitDbContext>(context);
                });

                opt.ReceiveEndpoint("TestSagaConsumer2", config =>
                {
                    config.ConfigureConsumer<TestSagaConsumer2>(context);
                    config.SetQuorumQueue();
                    config.UseEntityFrameworkOutbox<MassTransitDbContext>(context);
                });
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}