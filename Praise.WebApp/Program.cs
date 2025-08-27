
// TODO
// Get into GIT WED
// Email notification for sent praise WED
// Build Badge checker for sent praise WED
// Log all activity to AWS CloudWatch? in Praise App THU
// Deploy to EC2 THU


using Auth0.AspNetCore.Authentication;
using Praise.WebApp.Components;
using Praise.WebApp.Logic;
using Praise.WebApp.Logic.Events;
using Praise.WebApp.Logic.Events.EventBindings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IPraiseAuthentication, PraiseAuthentication>();
builder.Services.AddSingleton<IPraiseLogic, PraiseLogic>();
builder.Services.AddSingleton<IPraiseTypeLogic, PraiseTypeLogic>();

// event bindings
builder.Services.AddSingleton<ISendPraiseEventBindings, SendPraiseEventBindings>();
builder.Services.AddSingleton<ISendPraiseReceivedEvent, SubscribeSendPraiseBadgeEvent>();
builder.Services.AddSingleton<ISendPraiseReceivedEvent, SubscribeSendPraiseNotificationEvent>();

builder.Services.AddBlazorBootstrap();

builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization(); 

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

//var sendPraiseEvents = app.Services.GetService<ISendPraiseEventBindings>();
//await sendPraiseEvents.Bind();

//var httpContext = app.Services.GetService<IHttpContextAccessor>();
var eventManager = new AppEventManager();
await eventManager.Initialise(app);

app.Run();



//public static class WebApplicationExtensions
//{
//    public static WebApplication ConfigureCustomEvents(this WebApplication app)
//    {
//        app.Services.GetService<IHttp>
//        return app;
//    }
//}