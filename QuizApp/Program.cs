using DataAccess;
using DataAccess.IRepo;
using DataAccess.Repo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
));




builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
//builder.Services.AddScoped<IBaseRepository<Answer>, BaseRepository<Answer>>();
//builder.Services.AddScoped<IBaseRepository<Quiz>, BaseRepository<Quiz>>();
//builder.Services.AddScoped<IBaseRepository<Question>, BaseRepository<Question>>();
//builder.Services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
//builder.Services.AddScoped<IBaseRepository<UserAnswer>, BaseRepository<UserAnswer>>();
//builder.Services.AddScoped<IBaseRepository<UserQuiz>, BaseRepository<UserQuiz>>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

