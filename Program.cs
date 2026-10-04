using System.Text;
using GestorComercial_API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog; 

// 1. Inicialización temprana del registrador de arranque (Bootstrap Logger)
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Iniciando aplicación Gestor Comercial API...");

    //  CREAR EL BUILDER PRIMERO
    var builder = WebApplication.CreateBuilder(args);

    //  Conectar Serilog al Host de la aplicación leyendo appsettings.json
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    // Add services to the container.
    builder.Services.AddControllers();

    // CONFIGURAR BASE DE DATOS
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    // CONFIGURAR CORS PERMISIVO
    builder.Services.AddCors(options => {
        options.AddPolicy("PermitirTodo", policy =>
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    });

    // CONFIGURAR JWT
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "TuClaveSecretaSuperLargaYSegura12345"))
            };
        });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // CONSTRUIR LA APP
    var app = builder.Build();

    // 5. Auditoría HTTP de Serilog (Registra tiempo, método, URL y código HTTP de cada petición)
    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} respondió {StatusCode} en {Elapsed:0.0000} ms";
    });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    // MIDDLEWARES 
    app.UseStaticFiles(); // Para leer imágenes en wwwroot/uploads

    // 6. ORDEN CRÍTICO MANTENIDO PERFECTO
    app.UseCors("PermitirTodo"); // Primero permitir que entren peticiones
    app.UseAuthentication();     // Segundo identificar quién es
    app.UseAuthorization();      // Tercero ver si tiene permisos

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Fallo crítico no controlado durante el inicio de la aplicación.");
}
finally
{
    Log.CloseAndFlush(); // Asegura volcar los búferes de archivo a disco antes de apagar
}