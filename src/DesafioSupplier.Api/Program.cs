using DesafioSupplier.Application.Auth;
using DesafioSupplier.Application.Services;
using DesafioSupplier.ServicesAsync.Consumers;
using DesafioSupplier.ServicesAsync.Publishers;
using DesafioSupplier.Domain.Interfaces.Services;
using DesafioSupplier.ServicesAsync.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISignInService, SignInService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();

builder.Services.Configure<ServerSettingsRabbitMQ>(builder.Configuration.GetSection("RabbitServerConfig"));
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("AuthConfig"));

builder.Services.AddSingleton<TransactionPublisher>();
builder.Services.AddHostedService<TransactionConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
