using Apino.Application.Services;
using Apino.Application.Services.Auth;
using Apino.Application.Services.Cart;
using Apino.Application.Services.Notif;
using Apino.Application.Services.Order;
using Apino.Infrastructure;
using Apino.Infrastructure.Extensions;
using Apino.Infrastructure.Messaging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Parbad.Builder;
using Parbad.Gateway.Mellat;
using Parbad.Storage.EntityFrameworkCore.Builder;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

// -----------------
// MVC
// -----------------
builder.Services.AddControllersWithViews();

// -----------------
// Infrastructure (DbContext, etc.)
// -----------------
builder.Services.AddInfrastructure(builder.Configuration);

// -----------------
// Application + Infrastructure DI
// -----------------
builder.Services.AddScoped<IOtpService, OtpService>();
builder.Services.AddScoped<ISmsSender, KavenegarSmsSender>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IToolsService, Tools>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// ===================================================
// 🔐 AUTHENTICATION (اصلاح حیاتی)
// ===================================================
builder.Services
    .AddAuthentication(options =>
    {
        // 👇 مهم‌ترین خط این فایل
        options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    // -----------------
    // 🍪 Cookie Auth (برای MVC)
    // -----------------
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/auth/login";
        options.AccessDeniedPath = "/auth/access-denied";

        options.Cookie.Name = "Apino.Auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
    })
    // -----------------
    // 🔑 JWT Auth (فقط برای Ajax)
    // -----------------
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            ),

            ClockSkew = TimeSpan.Zero
        };
    });

// ===================================================
// 💳 PARBAD – Mellat Gateway
// ===================================================
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

// ===================================================
// 🚀 APP PIPELINE
// ===================================================
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🔐 ترتیب بسیار مهم
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
