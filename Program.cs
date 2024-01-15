// https://juldhais.net/multiple-database-provider-migrations-in-entity-framework-fae069ff5f11
// https://github.com/juldhais/EFMultipleMigration
using EFMultipleMigration;
using EFMultipleMigration.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

switch (builder.Configuration.GetConnectionString("Provider"))
{
    case "SqlServer":
        builder.Services.AddDbContext<DataContext, SqlServerDataContext>();
        break;
    case "Sqlite":
        builder.Services.AddDbContext<DataContext, SqliteDataContext>();
        break;
    case "Postgresql":
        builder.Services.AddDbContext<DataContext, PostgresqlDataContext>();
        break;
}

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
    options.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date",
        Example = new OpenApiString("1990-01-01")
    })
);

var app = builder.Build();

var scope = app.Services.CreateScope();
var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
dataContext.Database.Migrate();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
