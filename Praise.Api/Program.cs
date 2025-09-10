using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Praise.Api.DataLayer;
using Praise.Api.DataLayer.Interfaces;
using Praise.Api.Logic;
using Praise.Api.Logic.Events;
using Praise.Api.Logic.Interfaces;
using Praise.Models.Configuration;
using Praise.WebApp.Logic;
using Praise.WebApp.Logic.Events;
using Praise.WebApp.Logic.Events.EventBindings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IPraiseLogic, PraiseLogic>();

// event bindings
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IPraiseDetailsDataLayer, PraiseDetailsDataLayer>();
builder.Services.AddSingleton<IEmailManagerLogic, EmailManagerLogic>();
builder.Services.AddSingleton<ISendPraiseEventBindings, SendPraiseEventBindings>();
builder.Services.AddSingleton<ISendPraiseReceivedEvent, SubscribeSendPraiseBadgeEvent>();
builder.Services.AddSingleton<ISendPraiseReceivedEvent, SubscribeSendPraiseNotificationEvent>();

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(jwtOptions =>
    {
        jwtOptions.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
        jwtOptions.Audience = builder.Configuration["Auth0:Audience"];
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.Configure<DatabaseOptions>(
    builder.Configuration.GetSection(DatabaseOptions.ConfigNode));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var eventManager = new AppEventManager();
await eventManager.Initialise(app);

app.Run();
