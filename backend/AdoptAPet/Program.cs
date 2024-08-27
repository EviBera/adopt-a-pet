using AdoptAPet.Data;
using AdoptAPet.Models;
using AdoptAPet.Services.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IAdvertisementRepository, AdvertisementRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.SignIn.RequireConfirmedEmail = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
   // .AddDefaultUI();

   builder.Services.ConfigureApplicationCookie(options =>
   {
       options.Cookie.HttpOnly = true;
       options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
       options.LoginPath = "/api/auth/login";
       options.SlidingExpiration = true;
   });

var app = builder.Build();

// Apply migrations automatically
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

AddBasicActors();

app.Run();

void AddBasicActors()
{
    var tAdmin = CreateAdminIfNotExists();
    tAdmin.Wait();

    var tRescueTeamMember = CreateRescueTeamMemberIfNotExists();
    tRescueTeamMember.Wait();

    var tRegularUser = CreateRegularUserIfNotExists();
    tRegularUser.Wait();
}

async Task CreateAdminIfNotExists()
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var adminInDb = await userManager.FindByEmailAsync("admin@adoptapet.com");
    if (adminInDb == null)
    {
        var admin = new User
        {
            FirstName = "Administrator",
            LastName = "of the Application",
            Email = "admin@adoptapet.com",
            UserName = "admin@adoptapet.com"
        };
        var adminCreated = await userManager.CreateAsync(admin, "String!0");

        if (adminCreated.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}

async Task CreateRescueTeamMemberIfNotExists()
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var rescueTeamMemberInDb = await userManager.FindByEmailAsync("staff@adoptapet.com");
    if (rescueTeamMemberInDb == null)
    {
        var rescueTeamMember = new User
        {
            FirstName = "Francis",
            LastName = "of Assisi",
            Email = "staff@adoptapet.com",
            UserName = "staff@adoptapet.com"
        };
        var rescueTeamMemberCreated = await userManager.CreateAsync(rescueTeamMember, "String!1");

        if (rescueTeamMemberCreated.Succeeded)
        {
            await userManager.AddToRoleAsync(rescueTeamMember, "Rescue Team");
        }
    }
}

async Task CreateRegularUserIfNotExists()
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var regularUserInDb = await userManager.FindByEmailAsync("tom.taylor@email.com");
    if (regularUserInDb == null)
    {
        var regularUser = new User
        {
            FirstName = "Tom",
            LastName = "Taylor",
            Email = "tom.taylor@email.com",
            UserName = "tom.taylor@email.com"
        };
        var regularUserCreated = await userManager.CreateAsync(regularUser, "String!2");

        if (regularUserCreated.Succeeded)
        {
            await userManager.AddToRoleAsync(regularUser, "User");
        }
    }
}

