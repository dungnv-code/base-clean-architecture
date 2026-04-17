using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Health check cho Render (và các load balancer khác)
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));

app.UseHttpsRedirection();

app.Run();
