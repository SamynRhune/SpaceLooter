using ActionCommandGame.Sdk;
using ActionCommandGame.Settings;
using ActionCommandGame.Ui.WebApp.Settings;
using ActionCommandGame.Security.Model.Abstractions;
using Microsoft.AspNetCore.Authentication.Cookies;
using ActionCommandGame.Ui.Mvc.Stores;




var builder = WebApplication.CreateBuilder(args);


var appSettings = new AppSettings();
builder.Configuration.Bind(nameof(AppSettings), appSettings);

builder.Services.AddSingleton(appSettings);

var apiSettings = new ApiSettings();
builder.Configuration.GetSection(nameof(ApiSettings)).Bind(apiSettings);

builder.Services.AddHttpClient("ActionCommandGameApi", options =>
{
    if (!string.IsNullOrWhiteSpace(apiSettings.BaseAddress))
    {
        options.BaseAddress = new Uri(apiSettings.BaseAddress);
    }
});

/*builder.Services.AddTransient<Game>();*/
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<GameSdk>();
builder.Services.AddScoped<ItemSdk>();
builder.Services.AddScoped<PlayerItemSdk>();
builder.Services.AddScoped<NegativeGameEventSdk>();
builder.Services.AddScoped<PositiveGameEventSdk>();
builder.Services.AddScoped<PlayerSdk>();
builder.Services.AddScoped<IdentitySdk>();
builder.Services.AddScoped<UserRoleSdk>();
builder.Services.AddScoped<AppSettings>();
builder.Services.AddScoped<AccountSdk>();

builder.Services.AddScoped<ITokenStore, TokenStore>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Identity/SignIn";
});

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
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
