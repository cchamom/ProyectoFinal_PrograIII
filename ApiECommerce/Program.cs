using Microsoft.EntityFrameworkCore;
using ProyectoFinal_PrograIII.Data;
using ProyectoFinal_PrograIII.IServices;
using ProyectoFinal_PrograIII.Servicio;
using ProyectoFinal_PrograIII.Modelo;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar la conexión a MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        connectionString, 
        ServerVersion.AutoDetect(connectionString),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure()
    )
);

builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation(); 

// Registrar servicio de productos
builder.Services.AddScoped<IProductoService, ProductoServicio>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Definición de la clase WeatherForecast (si la necesitas para pruebas)
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}