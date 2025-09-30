// TODO:
// DONE Get into GIT WED
// DONE Move logic to Praise.API out of PraiseWeb to benefit MobileApp
// DONE Security of APIS - https://auth0.com/blog/securing-aspnet-minimal-webapis-with-auth0/
// DONE auth call based on Auth0 API
// DONE Add cosmos db and save praise
// DONE Email notification for sent praise
// DONE Read praises sent
// DONE Read praises received
// Transform when publishng
// All keys in Vault
// OpenAPI for documentation
// Build Badge checker for sent praise
// All security checks for pages and apis

using Auth0.AspNetCore.Authentication;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using GraphQL;
using GraphQL.Types;
using Praise.Models.Configuration;
using Praise.WebApp.Components;
using Praise.WebApp.GraphQL;
using Praise.WebApp.Logic;
using Praise.WebApp.Logic.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IPraiseUserLogic, PraiseUserLogic>();
builder.Services.AddSingleton<IPraiseApiWrapper, PraiseApiWrapper>();
builder.Services.AddSingleton<IPraiseAuthentication, PraiseAuthentication>();
builder.Services.AddSingleton<IPraiseLogic, PraiseLogic>();
builder.Services.AddSingleton<IPraiseTypeLogic, PraiseTypeLogic>();
builder.Services.AddSingleton<IRecentActivityLogic, RecentActivityLogic>();
builder.Services.AddSingleton<IBadgeLogic, BadgeLogic>();

builder.Services.AddBlazorBootstrap();

builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Authorisation:Domain"];
    options.ClientId = builder.Configuration["Authorisation:ClientId"];
    options.Scope = "openid profile email";
});

builder.Services.AddDistributedMemoryCache();

// configure options 
builder.Services.Configure<AuthorisationOptions>(
    builder.Configuration.GetSection(AuthorisationOptions.ConfigNode));
builder.Services.Configure<PraiseApiOptions>(
    builder.Configuration.GetSection(PraiseApiOptions.ConfigNode));

// Add the Azure Monitor telemetry service to the application.
// This service will collect and send telemetry data to Azure Monitor.
builder.Services.AddOpenTelemetry().UseAzureMonitor();

// add graphql
builder.Services.AddSingleton<IEmployeeService, EmployeeService>();
builder.Services.AddSingleton<EmployeeDetailsType>();
builder.Services.AddSingleton<EmployeeQuery>();
builder.Services.AddSingleton<ISchema, EmployeeDetailsSchema>();

//builder.Services.AddGraphQL(b => b
//    .AddAutoSchema<EmployeeQuery>()  // schema
//    .AddSystemTextJson());   // serializer


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

//app.UseSession();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

//// graphql
//app.UseGraphQL<ISchema>("/graphql");            // url to host GraphQL endpoint
//app.UseGraphQLPlayground(
//    "/",                               // url to host Playground at
//    new GraphQL.Server.Ui.Playground.PlaygroundOptions
//    {
//        GraphQLEndPoint = "/graphql",         // url of GraphQL endpoint
//        SubscriptionsEndPoint = "/graphql",   // url of GraphQL endpoint
//    });

app.Run();



//public static class WebApplicationExtensions
//{
//    public static WebApplication ConfigureCustomEvents(this WebApplication app)
//    {
//        app.Services.GetService<IHttp>
//        return app;
//    }
//}