using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Pomelo.EntityFrameworkCore.MySql;
using rmt.Classes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 30)));
});

builder.Services.AddControllers();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options=>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "User API",
        Version = "v1",
        Description = "mrmt API for User Management",
        Contact = new OpenApiContact
        {
            Name = "s1dorftp",
            Email = "sidorkindanila@outlook.com",
            Url = new Uri("https://github.com/s1dorftp")
        }
    });
});
var app = builder.Build();
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "User API v1");
        c.RoutePrefix = string.Empty;
    }) ;
}

app.MapControllers();
app.Run();