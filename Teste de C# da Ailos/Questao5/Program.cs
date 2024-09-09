using FluentAssertions.Common;
using MediatR;
using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Database;
using Questao5.Infrastructure.Sqlite;
using System.Data;
using System.Data.Common;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// sqlite
builder.Services.AddSingleton(new DatabaseConfig { Name = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite") });
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registra o ICommandStore e sua implementação CommandStore
// Configura o SQLite
var connectionString = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite");
builder.Services.AddSingleton(new SqliteConnectionFactory(connectionString));

builder.Services.AddTransient<ICommandStore>(provider =>
{
    var connectionFactory = provider.GetRequiredService<SqliteConnectionFactory>();
    var dbConnection = connectionFactory.CreateConnection();
    return new CommandStore(dbConnection);
});

builder.Services.AddTransient<IDbConnection>((sp) =>
{
    return new SqliteConnection(connectionString);
});

builder.Services.AddTransient<IIdempotencyStore, IdempotencyStore>();

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

// sqlite
#pragma warning disable CS8602 // Dereference of a possibly null reference.
app.Services.GetService<IDatabaseBootstrap>().Setup();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

app.Run();

// Informações úteis:
// Tipos do Sqlite - https://www.sqlite.org/datatype3.html


