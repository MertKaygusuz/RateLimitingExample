using RateLimitingApi.Extensions;
using RateLimitingApi.AppSettings;
using RateLimitingApi.RequestLogging;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.Configure<AppSetting>(configuration);
var appSetting = configuration.Get<AppSetting>();

builder.Services.BuildRateLimit(appSetting);
builder.Services.AddDistributedMemoryCache();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseApiRequestResponseCustomMiddleware();

app.UseRateLimiter();

app.MapControllers();

app.Run();
