// TODO:
// DONE Get into GIT WED
// DONE Move logic to Praise.API out of PraiseWeb to benefit MobileApp
// Security of APIS - https://auth0.com/blog/securing-aspnet-minimal-webapis-with-auth0/
// auth call based on Auth0 API
// User management 
// Email notification for sent praise
// Build Badge checker for sent praise
// OpenAPI for documentation
// Log all activity to AWS CloudWatch? in Praise App
// Deploy to EC2

using Auth0.AspNetCore.Authentication;
using Praise.WebApp.Components;
using Praise.WebApp.Logic;
using Praise.WebApp.Logic.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IPraiseApi, PraiseApi>();
builder.Services.AddSingleton<IPraiseAuthentication, PraiseAuthentication>();
builder.Services.AddSingleton<IPraiseLogic, PraiseLogic>();
builder.Services.AddSingleton<IPraiseTypeLogic, PraiseTypeLogic>();
builder.Services.AddSingleton<IRecentActivityLogic, RecentActivityLogic>();

builder.Services.AddBlazorBootstrap();

builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
    options.Scope = "openid profile email";
});

builder.Services.AddDistributedMemoryCache();

//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromSeconds(10);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
//                {
//                    options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
//                    options.Audience = builder.Configuration["Auth0:Audience"];
//                }); ;

//builder.Services.AddAuthentication().AddJwtBearer();

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

app.Run();



//public static class WebApplicationExtensions
//{
//    public static WebApplication ConfigureCustomEvents(this WebApplication app)
//    {
//        app.Services.GetService<IHttp>
//        return app;
//    }
//}