using eTickets.Data;
using eTickets.Data.Cart;
using eTickets.Data.Services;
using Microsoft.EntityFrameworkCore;

internal class Program {
    private static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        // DbContext configuration
        builder.Services.AddDbContext<AppDbContext>(options => options
                .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionsString")));

        // Services configuration
        builder.Services.AddScoped<IActorsService, ActorsService>();
        builder.Services.AddScoped<IProducersService, ProducersService>();
        builder.Services.AddScoped<ICinemasService, CinemasService>();
        builder.Services.AddScoped<IMoviesService, MoviesService>();

        builder.Services.AddSingleton<IHttpContextAccessor,  HttpContextAccessor>();
        builder.Services.AddScoped(sc => ShoppingCart.GetShoppingCart(sc));

        builder.Services.AddSession();

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment()) {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        
        app.UseSession();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        //Seed database
        AppDbInitializer.Seed(app);

        app.Run();
    }
}