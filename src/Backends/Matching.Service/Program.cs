using Serilog;
using EAnalytics.Common.Helpers;
using Matching.Service;

Console.Title = System.Reflection.Assembly.GetExecutingAssembly().FullName ?? string.Empty;
const string corsKey = "AllowAll";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDI();
builder.ConfigureSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseCors(corsKey);
app.MapControllers();
app.Run();
