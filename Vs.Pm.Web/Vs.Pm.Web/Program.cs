using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MudBlazor.Services;
using System.Globalization;
using Vs.Pm.Pm.Db;
using Vs.Pm.Web.Data.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");
builder.Services.AddSignalR();
builder.Services.AddMudServices();
builder.Services.AddScoped<LogApplicationService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<StatusService>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<TaskTypeService>();
//builder.AddDbContext<HostelDbSecond>(options => options.UseMySql(Configuration.GetConnectionString("HostelContext"), ServerVersion.AutoDetect(Configuration.GetConnectionString("HostelContext"))));
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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
