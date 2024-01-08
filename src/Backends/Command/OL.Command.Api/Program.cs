using EAnalytics.Common.Configurations;
using EAnalytics.Common;
using EAnalytics.Common.Helpers;
using OL.Infrastructure;
using Serilog;

Console.Title = System.Reflection.Assembly.GetExecutingAssembly().FullName ?? string.Empty;
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
builder.ConfigureSerilog();
builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSerilogRequestLogging(); 
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();