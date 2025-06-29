using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using KarmaWebAPI.Data;
using KarmaWebAPI.Configurations;
using KarmaWebAPI.Models;


var builder = WebApplication.CreateBuilder(args);
//builder.WebHost.UseUrls("https://0.0.0.0:443");
builder.WebHost.UseUrls("http://0.0.0.0:80");

// A�adir pol�tica de cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});


builder.Services.AddDbContext<DatabaseContext>(opt => {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity(); //La forma de infocar la clase ConfigureIdentity que hemos creado
builder.Services.ConfigureAuthentication();
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureServices();

//para evitar el bucle al recuperar las instancias
builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApiUser>>();

    string[] roleNames = { "AG_Professor", "AG_Alumne", "AG_Admin" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

//Configurar Swagger para que siempre est�n disponibles las definiciones
app.UseSwagger();
app.UseSwaggerUI();


//app.UseHttpsRedirection();
app.UseRouting();

//Middleware para que los errores de autentificaci�n no devuelvan un error 405 y ya est�
app.UseMiddleware<CustomErrorHandlingMiddleware>(); 
app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();
