using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NOTE.APP.Server.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables
string server = Environment.GetEnvironmentVariable("SERVER", EnvironmentVariableTarget.User)!;
string db = Environment.GetEnvironmentVariable("DB", EnvironmentVariableTarget.User)!;
string user = Environment.GetEnvironmentVariable("USER", EnvironmentVariableTarget.User)!;
string password = Environment.GetEnvironmentVariable("PASSWORD", EnvironmentVariableTarget.User)!;

// Build connection string
string connectionString = $"Server={server};Database={db};User Id={user};Password={password};TrustServerCertificate=True";

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the connection string in DI
builder.Services.AddSingleton<string>(connectionString);

// Register NoteRepository with DI using the interface
builder.Services.AddScoped<INoteRepository, NoteRepository>();

// Register SqlConnection
builder.Services.AddScoped(provider => new SqlConnection(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("AllowAllOrigins");

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Run();
