using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using WebApplicationGoogleNewss.Services;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddMemoryCache(); // import match to cache
        services.AddSingleton<RssService>();
        services.AddScoped<DalService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // Adding integrity and data protection
            app.UseHsts();
        }

        app.UseStaticFiles();  // Static files like CSS and JavaScript can be accessed
        app.UseHttpsRedirection(); 
        app.UseDefaultFiles(); 

        app.UseRouting(); 

        app.UseAuthorization();  

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Add path for HTML file
            endpoints.MapGet("/", async context =>
            {
                await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "index.html"));
            });

            // Add path for JavaScript files
            endpoints.MapGet("/JavaScript.js", async context =>
            {
                await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "JavaScript.js"));
            });
        });

        // Update RSS data from the original source with regular frequency
        var rssDataUpdater = app.ApplicationServices.GetRequiredService<RssDataUpdater>();
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        Task.Run(async () =>
        {
            await rssDataUpdater.StartUpdatingRssDataAsync(cancellationToken);
        }, cancellationToken);
    }
}
