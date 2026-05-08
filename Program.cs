using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Blazored.LocalStorage;
using Radzen;

using Inspecciones.Data;
using Inspecciones.Services;
using Inspecciones.Model;

var builder = WebApplication.CreateBuilder(args);

// Blazor Server clásico
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Radzen (servicios necesarios para componentes que usas)
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
// Ya NO usamos <RadzenTheme/>, por lo que no registramos Radzen.ThemeService.
// Si en el futuro vuelves a usar <RadzenTheme>, agrega:
// builder.Services.AddScoped<Radzen.ThemeService>();

// Servicios propios
builder.Services.AddScoped<IEmailServices, EmailServices>();
builder.Services.AddScoped<IDataInspeccion, DataInspeccion>();
builder.Services.AddScoped<IDataPregunta, DataPregunta>();
builder.Services.AddScoped<IDataMaquina, DataMaquina>();

// Utilidades
builder.Services.AddHttpClient();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

// DbContext
builder.Services.AddDbContext<DbNeoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionDbNeo")),
    ServiceLifetime.Transient
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Detalle de excepción en navegador durante desarrollo
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();