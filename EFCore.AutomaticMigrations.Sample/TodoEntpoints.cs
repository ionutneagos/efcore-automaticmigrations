using EFCore.AutomaticMigrations.Sample.Data;
using EFCore.AutomaticMigrations.Sample.Data.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace EFCore.AutomaticMigrations.Sample;

public static class TodoEntpoints
{
    public static RouteGroupBuilder MapTodosApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllTodos);
        group.MapGet("/{id}", GetTodo);
        group.MapPost("/", CreateTodo).AddEndpointFilter(async (efiContext, next) =>
            {
                var param = efiContext.GetArgument<TodoDto>(0);

                var validationErrors = Utilities.IsValid(param);

                if (validationErrors.Any())
                {
                    return Results.ValidationProblem(validationErrors);
                }

                return await next(efiContext);
            });

        group.MapPut("/{id}", UpdateTodo);
        group.MapDelete("/{id}", DeleteTodo);

        return group;
    }

    // get all todos
    // <snippet_1>
    public static async Task<Ok<Data.Entities.Todo[]>> GetAllTodos(TodoDbContext database)
    {
        var todos = await database.Todos.ToArrayAsync();
        return TypedResults.Ok(todos);
    }
    // </snippet_1>

    // get todo by id
    public static async Task<Results<Ok<Data.Entities.Todo>, NotFound>> GetTodo(int id, TodoDbContext database)
    {
        var todo = await database.Todos.FindAsync(id);

        if (todo != null)
        {
            return TypedResults.Ok(todo);
        }

        return TypedResults.NotFound();
    }

    // create todo
    public static async Task<Created<Data.Entities.Todo>> CreateTodo(TodoDto todo, TodoDbContext database)
    {
        var newTodo = new Data.Entities.Todo
        {
            Title = todo.Title,
            Description = todo.Description,
            IsDone = todo.IsDone
        };

        await database.Todos.AddAsync(newTodo);
        await database.SaveChangesAsync();

        return TypedResults.Created($"/todos/v1/{newTodo.Id}", newTodo);
    }

    // update todo
    public static async Task<Results<Created<Data.Entities.Todo>, NotFound>> UpdateTodo(Data.Entities.Todo todo, TodoDbContext database)
    {
        var existingTodo = await database.Todos.FindAsync(todo.Id);

        if (existingTodo != null)
        {
            existingTodo.Title = todo.Title;
            existingTodo.Description = todo.Description;
            existingTodo.IsDone = todo.IsDone;

            await database.SaveChangesAsync();

            return TypedResults.Created($"/todos/v1/{existingTodo.Id}", existingTodo);
        }

        return TypedResults.NotFound();
    }

    // delete todo
    public static async Task<Results<NoContent, NotFound>> DeleteTodo(int id, TodoDbContext database)
    {
        var todo = await database.Todos.FindAsync(id);

        if (todo != null)
        {
            database.Todos.Remove(todo);
            await database.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }
}
