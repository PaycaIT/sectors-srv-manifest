using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using sectors_service_orders.Auth;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Cargar la configuraci�n desde appsettings.json
var Configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// agrega cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", builder => builder.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader());
    options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
//agrega authorization
builder.Services.AddAuthorization();

//formOptions
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


//jwt auth
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
            ValidIssuer = Configuration["Jwt:Issuer"],
            // ValidAudience = Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false
        };
    });

builder.Services.AddSwaggerGen();

//redis consumer background service
//builder.Services.Configure<RedisConnectionOptions>(Configuration.GetSection("RedisConnection"));
//builder.Services.AddHostedService<RedisMessageConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAll");
    app.UseSwagger();
    app.UseSwaggerUI();

}

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

