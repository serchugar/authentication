using JWT.Configuration;
using JWT.Data;
using JWT.Middleware;
using Microsoft.EntityFrameworkCore;
using Serchugar.Base.Backend;
using Shared.Entities.User;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsDevelopment()) builder.Configuration.AddUserSecrets<Program>();
#region Services
builder.Services.AddDbContextPool<AppDbContext>(opts => 
    opts.UseNpgsql(
        builder.Configuration.GetConnectionString("testing"),
        o => o.MigrationsHistoryTable("__EFMigrationHistory", "authentication")));
builder.Services.AddJwtConfig(builder.Configuration);
builder.Services.AddDependencyInjectionConfig();
builder.Services.AddControllers();
if(builder.Environment.IsDevelopment()) builder.Services.AddSwaggerConfig();
#endregion

WebApplication app = builder.Build();
#region Middleware
if(app.Environment.IsDevelopment()) app.UseSwaggerConfig();
app.UseJwtConfig();
app.UseCustomExceptionHandler();
#endregion

app.DiscoverControllerRoutes();
app.DiscoverKeyedEntities(typeof(UserModel).Assembly);

app.MapControllers();
app.Run();