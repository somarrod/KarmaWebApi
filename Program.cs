using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using KarmaWebAPI.Data;
using KarmaWebAPI.Configurations;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

// Añadir política de cors
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
//builder.Services.AddDbContext<DatabaseContext>(opt => {
//    opt.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(9, 3, 0)));
//});

//builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity(); //La forma de infocar la clase ConfigureIdentity que hemos creado
builder.Services.ConfigureAuthentication();
builder.Services.ConfigureAuthorization();
builder.Services.ConfigureServices();

//para evitar el bucle al recuperar las instancias
builder.Services.AddControllers().AddJsonOptions(options =>
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



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

//Middleware para que los errores de autentificación no devuelvan un error 405 y ya está
app.UseMiddleware<CustomErrorHandlingMiddleware>(); 
app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();
