using Microsoft.AspNetCore.Authentication.Cookies;
using Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<ClienteRepository>();
builder.Services.AddTransient<ProveedorRepository>();
builder.Services.AddTransient<UsuarioRepository>();
builder.Services.AddTransient<EmpleadoRepository>();

// Agrega autenticación por cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        // Opcional: options.AccessDeniedPath = "/Account/AccessDenied";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// IMPORTANTE: Usa autenticación antes que autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
