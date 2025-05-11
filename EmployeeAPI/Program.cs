using EmployeeDAL.Services;
using EmployeeBAL.Interfaces;
using EmployeeBAL.Factories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repositories
builder.Services.AddSingleton<DbService>();
builder.Services.AddScoped<EmployeeRepository>();
builder.Services.AddSingleton<DepartmentRepository>();

// Logging
builder.Services.AddSingleton<ILoggerService, FileLoggerService>();

// Abstract factories
builder.Services.AddScoped<IndoorFactory>();
builder.Services.AddScoped<OutdoorFactory>();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
