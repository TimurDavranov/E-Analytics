using Application;
using Application.Configurations;
using Infrastructure;
using Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddPresentation();
builder.Services
    .AddOptions<AppConfig>(builder, nameof(AppConfig));
builder.Services
    .AddOptions<DataBaseConfiguration>(builder, nameof(DataBaseConfiguration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
