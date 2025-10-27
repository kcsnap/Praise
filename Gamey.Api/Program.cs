// TODO
// rate limit, throttling
// 

using Gamey.Api.Configuration;
using Gamey.Api.DataLayer;
using Gamey.Api.DataLayer.Interfaces;
using Gamey.Api.Handlers;
using Gamey.Api.Interfaces;
using Gamey.Api.Logic;
using Gamey.Api.Logic.Achievements;
using Gamey.Api.Logic.Badges;
using Gamey.Api.Logic.Prizes;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

#region Swagger config
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
    options.AddSecurityDefinition("ClientId", new Microsoft.OpenApi.Models.OpenApiSecurityScheme

    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "X-CLIENT-ID",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Description = "Enter your Client Id here"
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
        },
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "X-CLIENT-ID",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                //Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ClientId"
                }
            },
            new string[] { }
        }
    });
});
#endregion

// Add authentication
builder.Services.AddAuthentication("ApiKeyScheme")
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>("ApiKeyScheme", null);

// DI
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IAchievementsFactory, AchievementsFactory>();
builder.Services.AddSingleton<IAchievementsLogic, AchievementsLogic>();
builder.Services.AddSingleton<IActionHistoryLogic, ActionHistoryLogic>();
builder.Services.AddSingleton<IBadgeLogic, BadgeLogic>();
builder.Services.AddSingleton<IGameEventsLogic, GameEventsLogic>();
builder.Services.AddSingleton<IGamerLogic, GamerLogic>();
builder.Services.AddSingleton<IGameLogic, GameLogic>();
builder.Services.AddSingleton<ISpecialBadgeGroupLogic, SpecialBadgeGroupAchievementLogic>();
builder.Services.AddSingleton<ICountBadgeGroupLogic, CountBadgeGroupLogic>();
builder.Services.AddSingleton<IStreakBadgeGroupLogic, StreakBadgeGroupLogic>();
builder.Services.AddSingleton<IDeterministicEncryption, DeterministicEncryption>();
builder.Services.AddSingleton<ISecureEncryption, SecureEncryption>();
builder.Services.AddSingleton<ITokenWalletLogic, TokenWalletLogic>();
builder.Services.AddSingleton<IPrizeGroupLogic, GamePrizeLogic>();

// Data layer DI
builder.Services.AddSingleton<IAchievementDataLayer, AchievementDataLayer>();
builder.Services.AddSingleton<IActionHistoryDataLayer, ActionHistoryDataLayer>();
builder.Services.AddSingleton<IBadgeGroupDataLayer, BadgeGroupDataLayer>();
builder.Services.AddSingleton<IBadgeDataLayer, BadgeDataLayer>();
builder.Services.AddSingleton<IGameDataLayer, GameDataLayer>();
builder.Services.AddSingleton<IGamerDataLayer, GamerDataLayer>();
builder.Services.AddSingleton<IGameEventDataLayer, GameEventDataLayer>();
builder.Services.AddSingleton<IGamerEventDataDataLayer, GamerEventDataDataLayer>();
builder.Services.AddSingleton<IGameyDbConnection, GameyDbConnection>();
builder.Services.AddSingleton<ITokenWalletDataLayer, TokenWalletDataLayer>();
builder.Services.AddSingleton<IPrizeGroupDataLayer, PrizeGroupDataLayer>();

// Config options
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(DatabaseOptions.ConfigNode));
builder.Services.Configure<TokenContractOptions>(builder.Configuration.GetSection(TokenContractOptions.ConfigNode));
builder.Services.Configure<EncryptionOptions>(builder.Configuration.GetSection(EncryptionOptions.ConfigNode));

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
