using DropShipping.Data;
using Microsoft.EntityFrameworkCore;
using DropShipping.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using DropShipping.DAOs;
using DropShipping.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers()
.AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("PostgreSQL");

builder.Services.AddDbContext<DropshippingContext>(options => options.UseNpgsql(connectionString));

var MyAllowSpecificOrigins = "MyAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
                      });
});


builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddTransient<DropshippingContext>();
builder.Services.AddScoped<ItemDAO>();
builder.Services.AddScoped<CityDAO>();
builder.Services.AddScoped<OfficeDAO>();
builder.Services.AddScoped<OrderDAO>();
builder.Services.AddScoped<PaymentStatusDAO>();
builder.Services.AddScoped<ProductDAO>();
builder.Services.AddScoped<RoleDAO>();
builder.Services.AddScoped<ShipmentAgencyDAO>();
builder.Services.AddScoped<ShipmentStateDAO>();
builder.Services.AddScoped<UserAddressDAO>();
builder.Services.AddScoped<TransportDAO>();
builder.Services.AddScoped<UserDAO>();
builder.Services.AddScoped<UserDataDAO>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<AddOrderToShipmentManagerService>();
builder.Services.AddScoped<OfficeManagerService>();
builder.Services.AddScoped<OrderManagerService>();
builder.Services.AddScoped<PurchaseService>();
builder.Services.AddScoped<UserRegistrationService>();
builder.Services.AddScoped<ShipmentStateManagerService>();


builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(jwt => {
        var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value!);
        jwt.SaveToken = true;
        jwt.TokenValidationParameters = new TokenValidationParameters(){
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false, // for development porpoise
            ValidateAudience = false,
            RequireExpirationTime = false, // for development
            ValidateLifetime = true,
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);
app.MapControllers();
// app.UseHttpsRedirection();

app.Run();
