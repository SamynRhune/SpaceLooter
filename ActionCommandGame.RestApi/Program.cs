using ActionCommandGame.Repository;
using ActionCommandGame.RestApi.Security;
using ActionCommandGame.RestApi.Security.Settings;
using ActionCommandGame.RestApi.Service;
using ActionCommandGame.Security.Model;
using ActionCommandGame.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Register the EntityFramework database In Memory as a Singleton
builder.Services.AddDbContext<ActionButtonGameDbContext>(options =>
{
    options.UseSqlServer("Server=(localdb)\\VivesGame;Database=ActionCommandGameDatabase;Trusted_Connection=True;TrustServerCertificate=True");

}, ServiceLifetime.Scoped);

var jwtSettings = new JwtSettings();
builder.Configuration.GetSection(nameof(JwtSettings)).Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ActionButtonGameDbContext>();

// within this section we are configuring the authentication and setting the default scheme
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(jwt => {
        if (!string.IsNullOrWhiteSpace(jwtSettings.Secret))
        {
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            jwt.SaveToken = true;
            jwt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey =
                    true, // this will validate the 3rd part of the jwt token using the secret that we added in the appsettings and verify we have generated the jwt token
                IssuerSigningKey = new SymmetricSecurityKey(key), // Add the secret key to our Jwt encryption
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };
        }
    });




/*builder.Services.Configure<IdentityOptions>(options =>
{
    options.ClaimsIdentity.RoleClaimType = "Admin";
});*/
builder.Services.AddAuthorization();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ActionCommandGame.Settings.AppSettings>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<PlayerService>();
builder.Services.AddScoped<PlayerItemService>();
builder.Services.AddScoped<NegativeGameEventService>();
builder.Services.AddScoped<PositiveGameEventService>();
builder.Services.AddScoped<GameService>();
builder.Services.AddScoped<IdentityService>();
builder.Services.AddScoped<UserRoleService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

