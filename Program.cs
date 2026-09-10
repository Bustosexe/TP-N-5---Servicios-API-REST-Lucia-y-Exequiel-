using System.Text;
using GestorComercial_API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

// 1. CREAR EL BUILDER PRIMERO
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
<<<<<<< HEAD
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
=======
builder.Services.AddControllers();
>>>>>>> dbb54449c0443ffe3f887af7f88f05eb229fdad0

// 2. CONFIGURAR BASE DE DATOS
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. CONFIGURAR CORS PERMISIVO
builder.Services.AddCors(options => {
    options.AddPolicy("PermitirTodo", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// 4. CONFIGURAR JWT
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

// 5. CONSTRUIR LA APP
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 6. MIDDLEWARES 
app.UseStaticFiles(); // Para leer imágenes en wwwroot/uploads

app.UseCors("PermitirTodo"); // Primero permitir que entren peticiones
app.UseAuthentication();     // Segundo identificar quién es
app.UseAuthorization();      // Tercero ver si tiene permisos

app.MapControllers();

app.Run();