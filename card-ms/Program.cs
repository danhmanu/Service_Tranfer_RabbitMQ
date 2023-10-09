using card_ms;
using MassTransit;
using rabbitmq;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddMassTransit(cfg =>
        {
            cfg.AddConsumer<OrderCardNumberValidateConsumer>();

            cfg.AddBus(provider => RabbitMqBus.ConfigureBus(provider, (cfg, host) =>
            {
                cfg.ReceiveEndpoint(BusConstants.OrderQueue, ep =>
                {
                    ep.ConfigureConsumer<OrderCardNumberValidateConsumer>(provider);
                });
            }));
        });

        builder.Services.AddMassTransitHostedService();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}