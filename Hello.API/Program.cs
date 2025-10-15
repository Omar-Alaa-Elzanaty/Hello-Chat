using Hello.API;
using Hello.Application.Extensions;
using Hello.Application.Hubs;
using Hello.Domain.Interfaces;
using Hello.Infrastructure.Extensions;
using Hello.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddPersistance(builder.Configuration);

builder.Services.AddAPIDependencies(builder.Configuration);// order after line 10 was important to work and accept authentication

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR()
    .AddStackExchangeRedis(builder.Configuration["Redis"]!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors(x =>
{
    x.AllowAnyHeader()
     .AllowAnyMethod()
     .AllowAnyOrigin();
});


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chat-hub");

var bloom = app.Services.GetRequiredService<IUserNameBloomServices>();
await bloom.EnsureCreatedAsync();

app.Run();
