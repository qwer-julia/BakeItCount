using BakeItCountWeb.Services;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var apiUrl = Environment.GetEnvironmentVariable("API_URL")
             ?? builder.Configuration["ApiSettings:BaseUrl"]
             ?? "https://localhost:7211/api/";

Console.WriteLine($"API URL configurada: {apiUrl}");

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<TokenHandler>();

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(apiUrl);
    client.Timeout = TimeSpan.FromSeconds(60);

    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (compatible; BakeItCountWeb/1.0)");
    client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
    client.DefaultRequestHeaders.Add("Pragma", "no-cache");
    client.DefaultRequestHeaders.ConnectionClose = false;
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    AllowAutoRedirect = true,
    MaxAutomaticRedirections = 3,
    UseCookies = false,
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
})
.AddHttpMessageHandler<TokenHandler>()
.SetHandlerLifetime(TimeSpan.FromMinutes(5));

builder.Services.AddScoped<CurrentUserService>();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("=== CONFIGURA��O DA APLICA��O ===");
logger.LogInformation($"Environment: {app.Environment.EnvironmentName}");
logger.LogInformation($"API URL: {apiUrl}");
logger.LogInformation("===================================");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();