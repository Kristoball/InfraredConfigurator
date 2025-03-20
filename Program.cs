using InfraredConfigurator;
using InfraredConfigurator.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connections = builder.Configuration.GetSection("ConnectionStrings");
var connectionString = connections["DefaultConnection"];

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite(connectionString));

builder.Services.AddScoped<InfraredConfigEditorService>(x => new InfraredConfigEditorService(
    x.GetRequiredService<DatabaseContext>(),
    builder.Configuration.GetSection("ConfigSection")["ConfigPath"]
        ?? "/app/infrared_configurator/proxy/"
));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    databaseContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
