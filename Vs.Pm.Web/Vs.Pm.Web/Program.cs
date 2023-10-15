using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MudBlazor.Services;
using System.Globalization;
using Vs.Pm.Pm.Db;
using Vs.Pm.Web.Data;
using Vs.Pm.Web.Data.Service;
using Vs.Pm.Web.Data.SharedService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
builder.Services.AddSignalR();
builder.Services.AddMudServices();
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(
            ConstField.CookieScheme)
            .AddCookie(ConstField.CookieScheme,
            op => { op.LoginPath = "/login"; });

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddHttpClient();
builder.Services.AddScoped(r =>
{
    var client = new HttpClient(new HttpClientHandler()
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator //()=> { true}
    });
    return client;
});
builder.Services.AddAuthorization(config =>
{
    config.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    config.AddPolicy("AuthUserRoles", policy => policy.RequireRole("User", "Admin"));
});


builder.Services.AddScoped<LogApplicationService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<StatusService>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<TaskTypeService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<SecurityService>();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

string connection = builder.Configuration.GetConnectionString("ConnectionString");
builder.Services.AddDbContext<VsPmContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("VsPmContext")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.MapControllerRoute("default", "{controller=Account}/{action=Index}/{id?}");



app.Run();
