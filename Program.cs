using BlogApp.Data;
using BlogApp.Utility;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default"); //Add this so we can read the connection string from appsettings.json

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<BlogContext>(options => options.UseSqlServer(connectionString)); //add this so that we can connect to our database.

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

app.MapRazorPages();
app.MapControllers(); //Add this so that we can use our api routes.

app.Run();