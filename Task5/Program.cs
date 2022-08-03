using Microsoft.EntityFrameworkCore;
using Task5.Domain.Interfaces;
using Task5.Domain.Mapping;
using Task5.Hubs;
using Task5.Services;
using Task5.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

var connectionString = builder.Configuration.GetConnectionString("ApplicationContextConnection") ??
                       throw new InvalidOperationException(
                           "Connection string 'ApplicationContextConnection' not found.");

builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IUserService, UserService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapHub<MailHub>("/mail");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Mail}/{action=Index}/{id?}");

app.Run();