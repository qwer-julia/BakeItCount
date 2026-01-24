using BakeItCountApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using BakeItCountApi.Cn.Users;
using BakeItCountApi.Cn.Login;
using BakeItCountApi.Cn.Pairs;
using BakeItCountApi.Cn.Flavors;
using BakeItCountApi.Cn.Purchases;
using BakeItCountApi.Cn.Schedules;
using BakeItCountApi.Cn.Swaps;
using BakeItCountApi.Cn.UserAchievements;
using BakeItCountApi.Cn.Achievements;
using BakeItCountApi.Cn.FlavorVotes;
using BakeItCountApi.Models;
using BakeItCountApi.Services;
using Quartz;

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
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddEntityFrameworkNpgsql()
    .AddDbContext<Context>(options =>
    options.UseNpgsql(connectionString));

// DAO
builder.Services.AddScoped<DaoPair>();
builder.Services.AddScoped<DaoPurchase>();
builder.Services.AddScoped<DaoSchedule>();
builder.Services.AddScoped<DaoFlavor>();
builder.Services.AddScoped<DaoUser>();
builder.Services.AddScoped<DaoSwap>();
builder.Services.AddScoped<DaoUserAchievement>();
builder.Services.AddScoped<DaoAchievement>();
builder.Services.AddScoped<DaoFlavorVote>();

// CN
builder.Services.AddScoped<CnPair>();
builder.Services.AddScoped<CnPurchase>();
builder.Services.AddScoped<CnSchedule>();
builder.Services.AddScoped<CnFlavor>();
builder.Services.AddScoped<CnLogin>();
builder.Services.AddScoped<CnUser>();
builder.Services.AddScoped<CnSwap>();
builder.Services.AddScoped<CnUserAchievement>();
builder.Services.AddScoped<CnAchievement>();
builder.Services.AddScoped<CnFlavorVote>();

//Services 
builder.Services.AddScoped<EmailService>();

// JWT Auth
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("Smtp"));
builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("SendEmailJob", "mail");

    q.AddJob<SendEmailJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(t => t
        .WithIdentity("Trigger-MonFri-09", "mail")
        .ForJob(jobKey)
        .WithCronSchedule("0 0 09 ? * MON,FRI", x =>
            x.InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"))
        ));

});
var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

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
            var token = context.Request?.Cookies["AuthToken"];
            if (!string.IsNullOrEmpty(token))
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
