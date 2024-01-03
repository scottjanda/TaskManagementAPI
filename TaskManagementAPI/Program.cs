using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManagementAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Azure Key Vault
var keyVaultURL = builder.Configuration.GetSection("KeyVault:KeyVaultURL");
var keyVaultClientId = builder.Configuration.GetSection("KeyVault:ClientId");
var keyVaultClientSecret = builder.Configuration.GetSection("KeyVault:ClientSecret");
var keyVaultDirectoryID = builder.Configuration.GetSection("KeyVault:DirectoryID");
var credential = new ClientSecretCredential(keyVaultDirectoryID.Value!.ToString(), keyVaultClientId.Value!.ToString(), keyVaultClientSecret.Value!.ToString());
builder.Configuration.AddAzureKeyVault(keyVaultURL.Value!.ToString(), keyVaultClientId.Value!.ToString(), keyVaultClientSecret.Value!.ToString(), new DefaultKeyVaultSecretManager());
var client = new SecretClient(new Uri(keyVaultURL.Value!.ToString()), credential);

// Db Connection String
builder.Services.AddDbContext<TaskDbContext>(options =>
{
    options.UseSqlServer(client.GetSecret("TaskDbConnectionString").Value.Value.ToString());
});

// Api Key
var testSecret = client.GetSecret("PRODApiKey").Value.Value.ToString();
builder.Services.AddSingleton(_ => testSecret);

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

// Add Cors policy
app.UseCors("AllowAny");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Require api key
//app.UseMiddleware<ApiKeyMiddleware>();

app.Run();