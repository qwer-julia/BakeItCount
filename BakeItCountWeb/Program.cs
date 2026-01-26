using BakeItCountWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Detecta o ambiente
var isProduction = builder.Environment.IsProduction();
var isRender = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RENDER"));

// Configura a URL da API
string apiUrl;

if (isProduction || isRender)
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
    apiUrl = $"http://localhost:{port}/api/";

    Console.WriteLine($"[PRODUÇÃO] Usando API em: {apiUrl}");
}
else
{
    apiUrl = builder.Configuration["ApiSettings:BaseUrl"]
             ?? Environment.GetEnvironmentVariable("API_URL")
             ?? "https://localhost:7211/api/";

    Console.WriteLine($"[DESENVOLVIMENTO] Usando API em: {apiUrl}");
}

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(apiUrl);
    client.Timeout = TimeSpan.FromSeconds(30);

    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("User-Agent", "BakeItCountWeb/1.0");
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<TokenHandler>();
builder.Services.AddScoped<CurrentUserService>();

var app = builder.Build();

var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("=== CONFIGURAÇÃO DA APLICAÇÃO ===");
logger.LogInformation($"Environment: {app.Environment.EnvironmentName}");
logger.LogInformation($"API URL: {apiUrl}");
logger.LogInformation($"Is Render: {isRender}");
logger.LogInformation($"PORT: {Environment.GetEnvironmentVariable("PORT")}");
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