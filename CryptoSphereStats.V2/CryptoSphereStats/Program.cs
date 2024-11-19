using CryptoSphereStats;
using CryptoSphereStats.DataAccess.DataContext;
using CryptoSphereStats.DataAccess;
using CryptoSphereStats.Domain;
using CryptoSphereStats.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//  dependency injection
builder.Services.AddDbContext<ChartDataContext>(options =>
    options.UseSqlServer(connectionString));



builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<CryptoService>();


var app = builder.Build();

// detailed error Development mode
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//Initialization
await InitializeDatabaseAsync(app);

app.Run();


async Task InitializeDatabaseAsync(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ChartDataContext>();
        context.Database.EnsureCreated();

        // Fetch and save dummy data
        var cryptoData = await GenerateDummyData.FetchCryptoDataAsync(context);
        context.ChartData.AddRange(cryptoData);
        await context.SaveChangesAsync();
    }
}
