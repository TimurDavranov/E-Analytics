using EAnalytics.Common;
using EAnalytics.Common.Configurations;
using Web.ApiGateway;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigureOptions.AddOptions<AppConfig>(builder.Services, builder, nameof(AppConfig));

builder.Services.AddDI();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
