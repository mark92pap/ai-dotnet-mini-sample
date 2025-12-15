using AiMiniSample.Apis;
using AiMiniSample.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApis();
builder.Services.AddPersistence();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
