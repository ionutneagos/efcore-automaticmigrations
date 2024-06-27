using EFCore.AutomaticMigrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using EFCore.AutomaticMigrations.Sample;
using EFCore.AutomaticMigrations.Sample.Data;
using EFCore.AutomaticMigrations.Sample.Services;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddTransient<ITodoService, TodoService>();
    builder.Services.AddTransient<ITodoPocoService, TodoPocoService>();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //add todo database
    builder.Services.AddDbContext<TodoDbContext>(options =>
    {
        //read connection string from appsettings.json
        options.UseSqlServer(builder.Configuration["ConnectionStrings:ToDosDatabase"], providerOptions =>
        {
            providerOptions.EnableRetryOnFailure();
        });
    });

    var app = builder.Build();

    // begin apply automatic migration database to latest version
    await using AsyncServiceScope serviceScope = app.Services.CreateAsyncScope();

    await using TodoDbContext? dbContext = serviceScope.ServiceProvider.GetService<TodoDbContext>();

    if (dbContext is not null)
    {
        // If the database context was successfully resolved from the service provider, we apply migrations.
        // The DbMigrationsOptions object is used to configure automatic data loss prevention and offers other tools like viewing raw SQL scripts for migrations.
        // The database is created automatically if it does not exist, if exist will be updated to latest model changes
        // Pay attention if you are using a PaaS database, like Azure; it will be created automatically using the default SKU and this might affect your costs.

        await dbContext.MigrateToLatestVersionAsync(new DbMigrationsOptions { AutomaticMigrationDataLossAllowed = true });

        //at this stage dabatabase containse latest changes
        // Todo entity was mapped via Fluent API
        // TodoPoco entity was mapped via data annotations
    }

    app.UseSwagger();
    app.UseSwaggerUI();

    // todoV1 endpoints
    app.MapGroup("/todos")
        .MapTodosApi()
        .WithTags("Todo Endpoints")
        .WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Todo Endpoints",
            Description = "Sample endpoint for Todo entity migrated using Fluent API.",
        });

    app.MapGroup("/todopocs")
       .MapTodoPocosApi()
       .WithTags("TodoPoco Endpoints")
       .WithOpenApi(x => new OpenApiOperation(x)
       {
           Summary = "TodoPoco Endpoints",
           Description = "Sample endpoint for TodoPoco entity migrated using data annotations.",
       });

    await app.RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Unhandled exception: {ex}");
}
finally
{
    Console.WriteLine("Shut down complete");
}

