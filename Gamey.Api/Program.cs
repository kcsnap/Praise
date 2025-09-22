// TODO
// rate limit, throttling
// 

using Gamey.Api.Handlers;
using Gamey.Api.Interfaces;
using Gamey.Api.Logic;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// add swagger docs
builder.Services.AddSwaggerGen(options =>
{
    // Add API Key input in Swagger UI
    options.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "X-API-KEY",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Description = "Enter your API key here"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "X-API-KEY",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] { }
        }
    });
});

// Add authentication
builder.Services.AddAuthentication("ApiKeyScheme")
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKeyScheme", null);

// DI
builder.Services.AddScoped<IGameEventsLogic, GameEventsLogic>();
builder.Services.AddScoped<IBadgeLogic, BadgeLogic>();
builder.Services.AddScoped<IGamerLogic, GamerLogic>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();            // Serve Swagger JSON at /swagger/v1/swagger.json
app.UseSwaggerUI();          // Serve Swagger UI at /swagger

app.Run();
