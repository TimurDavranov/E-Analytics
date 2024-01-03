using EAnalytics.Common.Configurations;
using OL.Query.Api.Infrastructure;
using EAnalytics.Common;

Console.Title = System.Reflection.Assembly.GetExecutingAssembly().FullName ?? string.Empty;
const string corsKey = "AllowAll";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddOptions<AppConfig>(builder, nameof(AppConfig));
builder.Services
    .AddOptions<DataBaseConfiguration>(builder, nameof(DataBaseConfiguration));
builder.Services.AddCors(options => options.AddPolicy(corsKey,
    policyBuilder => policyBuilder.SetIsOriginAllowed(a => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

builder.Services.AddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsKey);
app.UseAuthorization();

app.MapControllers();

app.Run();