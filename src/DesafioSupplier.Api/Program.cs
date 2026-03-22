using System.Data;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.OpenApi.Models;
using DesafioSupplier.Api.Shared;
using Microsoft.IdentityModel.Tokens;
using DesafioSupplier.Application.Auth;
using DesafioSupplier.Application.Services;
using DesafioSupplier.ServicesAsync.Consumers;
using DesafioSupplier.ServicesAsync.Publishers;
using DesafioSupplier.Persistence.Repositories;
using DesafioSupplier.Persistence.Configuration;
using DesafioSupplier.Domain.Interfaces.Services;
using DesafioSupplier.ServicesAsync.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DesafioSupplier.Domain.Interfaces.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite: {seu token}"
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISignInService, SignInService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddSingleton<PasswordHasher>();

builder.Services.Configure<ServerSettingsRabbitMQ>(builder.Configuration.GetSection("RabbitServerConfig"));
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthConfig"));

builder.Services.AddSingleton<TransactionPublisher>();
builder.Services.AddHostedService<TransactionConsumer>();

/**************************** Autorizacao *********************************/

var authSection = builder.Configuration.GetSection("AuthConfig");

var key = Encoding.ASCII.GetBytes(authSection["SecretKey"] ?? string.Empty);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,

        ValidIssuer = authSection["Issuer"] ?? string.Empty,
        ValidAudience = authSection["Audience"] ?? string.Empty,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

/**************************************************************************/

var connection = new SqliteConnection("DataSource=:memory:");
connection.Open();

builder.Services.AddSingleton<IDbConnection>(connection);

builder.Services.AddMemoryCache();

var app = builder.Build();

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

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IDbConnection>();
    SqLiteDbInitializer.Initialize(db);
}

app.Run();
