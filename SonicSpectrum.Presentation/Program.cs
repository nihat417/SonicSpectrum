using SonicSpectrum.Infrastructure.Extensions;
using SonicSpectrum.Presentation;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

var app = builder.Build();

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);
startup.Configure(app, app.Environment);

app.Run();
