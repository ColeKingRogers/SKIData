using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using SKIData.Components;
using SKIData.Model;
using SKIData.Service;

namespace SKIData
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddGeolocationServices();
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContextFactory<Data.SkiResortContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("SkiResorts"));
            });

            builder.Services.AddQuickGridEntityFrameworkAdapter();

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddScoped<SkiResortService>();




            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
    app.UseMigrationsEndPoint();
            }

            app.UseHttpsRedirection();
            using (var scope = app.Services.CreateScope())//create database and seed data
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<Data.SkiResortContext>();
                dbContext.Database.EnsureCreated();
                Service.SkiResortService scraper = new Service.SkiResortService(dbContext);
                scraper.ScrapeAndSaveSkiResortsAsync().Wait();
            }


            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
