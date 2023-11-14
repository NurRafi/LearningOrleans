using GrainInterfaces;

var builder = WebApplication.CreateBuilder(args);

// TODO: Add Application Insights (ASP + Orleans)

if (builder.Environment.IsDevelopment())
{
    builder.Host
        .UseOrleans(siloBuilder =>
        {
            siloBuilder
                .AddAdoNetGrainStorage("RobotStore", options =>
                {
                    // Use appsettings.Development.json
                    options.ConnectionString = "Server=localhost;Database=LearningOrleans;User ID=NurRafi;Password=NurRafi;Trusted_Connection=False;Encrypt=False;MultipleActiveResultSets=True;";
                })
                .UseLocalhostClustering()
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Information);
                })
                .UseDashboard(); // TODO: Add Linux/Windows metrics and custom config
        })
        .UseConsoleLifetime();
}

// TODO: Add prod environment

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

using var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/robot/{name}/instruction", async (IGrainFactory grains, string name) =>
    {
        var robotGrain = grains.GetGrain<IRobotGrain>(name);
        return await robotGrain.GetNextInstruction();
    })
    .WithName("GetRobotNextInstruction")
    .WithOpenApi();

app.MapPost("/robot/{name}/instruction", async (IGrainFactory grains, string name, string instruction) =>
    {
        var grain = grains.GetGrain<IRobotGrain>(name);
        await grain.AddInstruction(instruction);
        return Results.Ok();
    })
    .WithName("AddRobotInstruction")
    .WithOpenApi();

app.Run();