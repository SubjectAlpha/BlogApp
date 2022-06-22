using BlogApp.Data;
using BlogApp.Utility;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default"); //Add this so we can read the connection string from appsettings.json

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<BlogContext>(options => options.UseSqlServer(connectionString)); //add this so that we can connect to our database.
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options => //Implementing session https://docs.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-6.0
{
    options.IdleTimeout = TimeSpan.FromMinutes(1);
    options.Cookie.IsEssential = true;
});

builder.Services.AddAntiforgery(options => //Implementing antiforgery https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-6.0
{
    options.FormFieldName = "BlogApp";
    options.HeaderName = "X-XSRF-TOKEN";
    options.SuppressXFrameOptionsHeader = false;
}); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession(); //Tell the application to use the session service we setup earlier.

var antiforgery = app.Services.GetRequiredService<IAntiforgery>();

app.Use((context, next) =>
{
    var requestPath = context.Request.Path.Value;

    if (!string.IsNullOrWhiteSpace(requestPath) && requestPath.Equals("/"))
    {
        var tokenSet = antiforgery.GetAndStoreTokens(context);
        context.Response.Cookies.Append("XSRF-TOKEN", tokenSet.RequestToken!,
            new CookieOptions { HttpOnly = false });
    }

    return next(context);
});

app.MapRazorPages();
app.MapControllers(); //Add this so that we can use our api routes.

app.Run();