using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using VendingMachine.DAL.EF;
using VendingMachine.DAL.Interfaces;
using VendingMachine.DAL.Repositories;
using VendingMachine.BLL.Services;
using VendingMachine.BLL.Interfaces;
using VendingMachine.Presentation.Mapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
sqlConnectionStringBuilder.AttachDBFilename = @"D:\Database\VendingMachine.mdf";

builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(sqlConnectionStringBuilder.ConnectionString));

builder.Services.AddAutoMapper(typeof(AppMappingProfile));

builder.Services.AddTransient<IVendingMachineService, VendingMachineService>();
builder.Services.AddTransient<IDrinkRepository, DrinkRepository>();
builder.Services.AddTransient<ICoinRepository, CoinRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.MapControllerRoute(
    name: "maintain",
    pattern: "{controller=Home}/{action=Maintain}");

app.Run();
