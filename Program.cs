using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using payca_lib_logging;
using sectors_service_orders.Auth;
using sectors_srv_manifest.Configuration;
using Serilog;
using System.Text;

IConfiguration configuration = CustomConfigurationManager.configuration;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = PaycaLogging.CreateLogger(configuration);
Log.Logger.Information("Starting up sectors-srv-manifest");
builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", builder => builder.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader());
    options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddAuthorization();


builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 100 * 1024 * 1024; // Set the maximum request length at 100MB
});
builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue; // if don't set the default value is: 128 MB
    x.MultipartHeadersLengthLimit = int.MaxValue;
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                var claims = context?.Principal?.Claims;
                if (!JWTUtils.ValidateBussinesData(claims))
                {
                    context?.Fail("Invalid Auth token");
                }
                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = configuration["Jwt:Issuer"],
            // ValidAudience = Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false
        };
    });

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UsePathBase(new PathString("/api"));

app.UseAuthentication();

app.MapControllers();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "api/{version}/{controller=Home}/{action=Index}"
    );
});

app.Run();

