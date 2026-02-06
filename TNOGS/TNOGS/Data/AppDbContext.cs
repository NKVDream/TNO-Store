using Microsoft.EntityFrameworkCore;
using TNOGS.Models;

namespace TNOGS.Data
{
    public class AppDbContext : DbContext
    {

        /*
        public AppDbContext(DbSet<Products> products, DbSet<Type> types, DbSet<Players> players)
        {
            Products = products;
            Types = types;
            Players = players;
        }
        */
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
            /*Этот фрагмент кода часто вызывает вопросы, потому что он выглядит как «магия» Boilerplate-кода (шаблонного кода). На самом деле, это входная дверь для конфигурации вашей базы данных.
Что это делает?
Этот конструктор позволяет внешнему миру (обычно это файл Program.cs) передать в класс AppDbContext важные настройки:
Какую базу данных использовать (SQL Server, PostgreSQL, SQLite и т.д.).
Строку подключения (адрес сервера, имя базы, логин и пароль).
Дополнительные фишки (например, включение ленивой загрузки или логирования SQL-запросов).
Конструкция : base(options) просто пробрасывает эти настройки «наверх» — в базовый класс DbContext, который уже знает, что с ними делать.

             */
        }
        public DbSet<Products> Products { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Players> Players { get; set; }

    }
}
