using SonicSpectrum.Application.WebSockets;
using SonicSpectrum.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCustomServices(builder.Configuration);
builder.Services.AddCustomIdentity();
builder.Services.AddCustomCors();
builder.Services.AddCustomDbContext(builder.Configuration);
builder.Services.AddCustomSwagger();
builder.Services.AddCustomAuthentication(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};

app.UseWebSockets(webSocketOptions);

app.UseMiddleware<WebSocketHandler>();

app.MapControllers();

app.Run();
