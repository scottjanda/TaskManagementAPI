using Azure.Core.Diagnostics;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskManagementAPI.Config;
using TaskManagementAPI.Models;
using AzureEventSourceListener listener = AzureEventSourceListener.CreateConsoleLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Azure Key Vault
var vaultConfig = builder.Configuration.GetSection("KeyVault").Get<VaultConfiguration>();
builder.Configuration.AddAzureKeyVault(new Uri(vaultConfig.Endpoint), new ClientSecretCredential(vaultConfig.TenantId, vaultConfig.ClientId, vaultConfig.ClientSecret));
var DBConnectionString = builder.Configuration.GetConnectionString("TaskDBConnectionString");
builder.Services.AddSingleton(DBConnectionString);
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlServer(DBConnectionString)
);

// Add Cors policy
builder.Services.AddCors(
    options =>
    {
        options.AddPolicy(
            name: "AllowAny",
            builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            );
    }
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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