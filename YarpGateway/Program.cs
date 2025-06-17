var builder = WebApplication.CreateBuilder(args);



builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();
app.MapGet("/", () => "Concert Ticket Management System!");
app.MapReverseProxy();

app.Run();
