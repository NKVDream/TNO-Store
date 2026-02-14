/*
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
*/
using Microsoft.AspNetCore.Builder; // Для WebApplication
using Microsoft.Extensions.DependencyInjection; // Для сервисов
using Microsoft.Extensions.Hosting; // Для окружения
using Microsoft.EntityFrameworkCore;
using TNOGS.Data;

var builder = WebApplication.CreateBuilder(args);

// Добавление контроллеров
builder.Services.AddControllers();

// Подключаем БД - ИСПРАВЛЕНО
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger только в разработке
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();