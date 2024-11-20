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
<<<<<<< HEAD
builder.Services.AddHttpClient<CryptoService>();

=======
>>>>>>> 968e6216e24e66284d61d01123721a6cc1b00e1c

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

<<<<<<< HEAD
//Initialization
=======
// Initialization
>>>>>>> 968e6216e24e66284d61d01123721a6cc1b00e1c
await InitializeDatabaseAsync(app);

app.Run();


async Task InitializeDatabaseAsync(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ChartDataContext>();
        context.Database.EnsureCreated();

        // Fetch and save dummy data
<<<<<<< HEAD
        var cryptoData = await GenerateDummyData.FetchCryptoDataAsync(context);
        context.ChartData.AddRange(cryptoData);
        await context.SaveChangesAsync();
=======
        var cryptoData = await GenerateDummyData.FetchCryptoDataAsync(context); 
        context.ChartData.AddRange(cryptoData); 
        await context.SaveChangesAsync(); 
>>>>>>> 968e6216e24e66284d61d01123721a6cc1b00e1c
    }
}
