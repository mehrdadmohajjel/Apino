using Apino.Application.Services;
using Apino.Application.Services.Auth;
using Apino.Application.Services.Cart;
using Apino.Application.Services.Order;
using Apino.Infrastructure;
using Apino.Infrastructure.Extensions;
using Apino.Infrastructure.Messaging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Parbad.Builder;
using Parbad.Gateway.Mellat;
using Parbad.Storage.EntityFrameworkCore.Builder;
using System.Text;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddInfrastructure(builder.Configuration);
// -----------------
// Application + Infrastructure DI
// -----------------
builder.Services.AddScoped<IOtpService, OtpService>();           // Application
builder.Services.AddScoped<ISmsSender, KavenegarSmsSender>();    // Infrastructure
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IToolsService, Tools>();

// JWT Authentication (ãËÇá ÓÇÏå)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddParbad()
    .ConfigureGateways(gateways =>
    {
        gateways.AddMellat()
            .WithAccounts(source =>
            {
                source.AddInMemory(account =>
                {
                    account.TerminalId =
                        builder.Configuration.GetValue<long>("Payment:Mellat:TerminalId");

                    account.UserName =
                        builder.Configuration["Payment:Mellat:UserName"];

                    account.UserPassword =
                        builder.Configuration["Payment:Mellat:UserPassword"];
                });
            });
    })
    .ConfigureHttpContext(ctx => ctx.UseDefaultAspNetCore())
    .ConfigureStorage(storage =>
    {
        storage.UseEfCore(options =>
        {
            var assemblyName = typeof(Program).Assembly.GetName().Name;

            options.ConfigureDbContext = db =>
                db.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    sql => sql.MigrationsAssembly(assemblyName)
                );

            options.PaymentTableOptions.Name = "ParbadPayments";
            options.TransactionTableOptions.Name = "ParbadTransactions";
        });
    });


var app = builder.Build();

// -----------------
// Configure the HTTP request pipeline
// -----------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // JWT
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();