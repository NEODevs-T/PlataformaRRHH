using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

using NeoRH;
using NeoRH.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ BLAZOR SERVER
builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor();

// ✅ LOCAL STORAGE
builder.Services.AddBlazoredLocalStorage();

// ✅ AUTH BLAZOR
builder.Services.AddAuthorizationCore();

// ✅ CUSTOM AUTH PROVIDER
builder.Services.AddScoped<CustomAuthStateProvider>();

builder.Services.AddScoped<
    AuthenticationStateProvider,
    CustomAuthStateProvider>();

// ✅ HTTP CLIENT
var apiBaseUrl =
    builder.Configuration["ApiSettings:BaseUrl"];

builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri(apiBaseUrl!)
    });

// ✅ API SERVICE
builder.Services.AddScoped<
    IRRHHApiService,
    RRHHApiService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

// ✅ STATIC FILES
app.UseStaticFiles();

// ✅ ROUTING
app.UseRouting();

// ✅ BLAZOR HUB
app.MapBlazorHub();

app.MapFallbackToPage("/_Host");

app.Run();