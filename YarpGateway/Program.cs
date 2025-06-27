using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));






//1 minute k andar 5 request allowed hai .
builder.Services.AddRateLimiter(options =>
{

    //handle the error 
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 503;
        context.HttpContext.Response.ContentType = "application/json";

        context.HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        context.HttpContext.Response.Headers.Add("Access-Control-Allow-Headers", "*");
        context.HttpContext.Response.Headers.Add("Access-Control-Allow-Methods", "*");

        await context.HttpContext.Response.WriteAsync(
            "{\"error\":\"Rate limit exceeded. Please try again later.\"}");
    };


    options.AddPolicy("gateway-policy", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown", // if from same ip address user is hitting the api , then he will get blocked.
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 1000, // 5 requests max
                Window = TimeSpan.FromMinutes(1),// per 1 min
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0 // extra request after 100 request . agar add krna h to 
            }));
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()    // Allow requests from any origin
              .AllowAnyHeader()    // Allow any headers (e.g. Authorization)
              .AllowAnyMethod();   // Allow GET, POST, PUT, DELETE, etc.
    });
});

var app = builder.Build();
app.UseCors("AllowAll");
app.MapGet("/", () => "Concert Ticket Management System!");
app.UseRateLimiter();
app.MapReverseProxy().RequireRateLimiting("gateway-policy");

app.Run();
