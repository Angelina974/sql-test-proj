using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using sql_test.Buisness;
using sql_test.DataAccesses;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string not found");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.Events.OnRedirectToLogin = context =>
        {
            // On redirige l'utilisateur vers la page de login
            context.Response.StatusCode = 401;
            return Task.CompletedTask;
        };
    })

    .AddGitHub(options =>
    {
        options.ClientId = builder.Configuration["GitHub:ClientId"] ?? string.Empty;
        options.ClientSecret = builder.Configuration["GitHub:ClientSecret"] ?? string.Empty;
        options.EnterpriseDomain = builder.Configuration["GitHub:EnterpriseDomain"] ?? string.Empty;
        options.Scope.Add("user:email");
        options.SaveTokens = true;
    });

builder.Services.AddAuthentication();

// Register services before building the app
builder.Services.AddAuthentication();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserDataAccess, UserDataAccess>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Génère la BDD si nécessaire 
    //var serviceScope = app.Services.CreateScope();
    //var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
    //context?.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(builder =>
{ 
    builder
    .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowCredentials()
        .AllowAnyMethod();

});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
