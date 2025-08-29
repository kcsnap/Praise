using Praise.Api.Logic.Events;
using Praise.Api.Logic.Interfaces;
using Praise.WebApp.Logic;
using Praise.WebApp.Logic.Events;
using Praise.WebApp.Logic.Events.EventBindings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IPraiseLogic, PraiseLogic>();

// event bindings
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<ISendPraiseEventBindings, SendPraiseEventBindings>();
builder.Services.AddSingleton<ISendPraiseReceivedEvent, SubscribeSendPraiseBadgeEvent>();
builder.Services.AddSingleton<ISendPraiseReceivedEvent, SubscribeSendPraiseNotificationEvent>();

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

var eventManager = new AppEventManager();
await eventManager.Initialise(app);

app.Run();
