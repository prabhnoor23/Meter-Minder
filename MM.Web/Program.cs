using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pspcl.Core.Domain;
using Pspcl.DBConnect;
using Pspcl.DBConnect.Install;
using Pspcl.Services.Options;
using Pspcl.Web.Lamar;
using Pspcl.Web.Mapping;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DBConnectionString");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, Role>(cfg =>
{
    cfg.User.RequireUniqueEmail = true;
    cfg.Password.RequiredUniqueChars = 0;
    cfg.Password.RequireNonAlphanumeric = false;
    cfg.Password.RequireDigit = false;
    cfg.Password.RequireLowercase = false;
    cfg.Password.RequireUppercase = false;
    cfg.Password.RequiredLength = 4;
    cfg.Password.RequireUppercase = false;
}).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(cfg=>cfg.AddProfile<StockMappingProfilecs>());

builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = true;
}).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

builder.Services.Configure<AzureOptions>(builder.Configuration.GetSection("Azure"));


builder.Host.UseLamar((context, registry) =>
{
    registry.IncludeRegistry<ApplicationRegistry>();
    registry.AddControllers();
});

var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

var path = configuration["LogFilePath"];
var logger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(configuration)
    .WriteTo.Map("DateTime", DateTime.Now.ToString("ddMMyyyy"), (DateTime, wt) => wt.File($"{configuration["LogFilePath"]}\\Logs_{DateTime}.txt"))
    .CreateLogger();
Log.Logger = logger;
builder.Host.UseSerilog();


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
app.UseAuthentication();
app.Services.GetService<IDbInitializer>().Initialize().Wait();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}");

    endpoints.MapControllers();
});

app.Run();
