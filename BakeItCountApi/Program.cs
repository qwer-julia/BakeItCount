using BakeItCountApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BakeItCountApi.Cn.Users;
using BakeItCountApi.Cn.Login;
using BakeItCountApi.Cn.Pairs;
using BakeItCountApi.Dao.Pairs;
using BakeItCountApi.Cn.Flavors;
using BakeItCountApi.Cn.Purchases;
using BakeItCountApi.Cn.Schedules;
using BakeItCountApi.Dao.Purchases;
using BakeItCountApi.Dao.Schedules;
using BakeItCountApi.Dao.Swaps;
using BakeItCountApi.Cn.Swaps;
using BakeItCountApi.Cn.UserAchievements;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true; // opcional
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<Context>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Pooling=true;Database=BakeItCountDatabase;User Id=postgres;Password=5822;"));

// DAO
builder.Services.AddScoped<DaoPair>();
builder.Services.AddScoped<DaoPurchase>();
builder.Services.AddScoped<DaoSchedule>();
builder.Services.AddScoped<DaoFlavor>();
builder.Services.AddScoped<DaoUser>();
builder.Services.AddScoped<DaoSwap>();
builder.Services.AddScoped<DaoPurchase>();
builder.Services.AddScoped<DaoUserAchievement>();

// CN
builder.Services.AddScoped<CnPair>();
builder.Services.AddScoped<CnPurchase>();
builder.Services.AddScoped<CnSchedule>();
builder.Services.AddScoped<CnFlavor>();
builder.Services.AddScoped<CnLogin>();
builder.Services.AddScoped<CnUser>();
builder.Services.AddScoped<CnSwap>();
builder.Services.AddScoped<CnPurchase>();
builder.Services.AddScoped<CnUserAchievement>();


// Controllers e JWT Auth
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
                 Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.TryGetValue("AuthToken", out var token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
