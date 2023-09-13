using EA.Application;
using EA.Infrastructure;
using EA.Presentation;
using EAnalytics.Common.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddOptions<AppConfig>(builder, nameof(AppConfig));
builder.Services
    .AddOptions<DataBaseConfiguration>(builder, nameof(DataBaseConfiguration));
builder.Services
    .AddOptions<RabbitMQConfiguration>(builder, nameof(RabbitMQConfiguration));
builder.Services
    .AddOptions<MongoDbConfiguration>(builder, nameof(MongoDbConfiguration));

builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddPresentation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
