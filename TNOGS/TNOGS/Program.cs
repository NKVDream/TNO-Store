using Microsoft.EntityFrameworkCore;
using TNOGS.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Сервисы
builder.Services.AddControllers();

// Используем переменную connectionString, которую ты объявил выше (так чище)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Сборка приложения (вызывается ТОЛЬКО ОДИН РАЗ)
var app = builder.Build();

// 3. Настройка Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
