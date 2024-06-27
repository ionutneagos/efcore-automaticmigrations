using Microsoft.AspNetCore.Http.HttpResults;
using EFCore.AutomaticMigrations.Sample.Data.Dto;
using EFCore.AutomaticMigrations.Sample.Data.Entities;
using EFCore.AutomaticMigrations.Sample.Services;
namespace EFCore.AutomaticMigrations.Sample;

public static class TodoPocoEndpoints
{
    public static RouteGroupBuilder MapTodoPocosApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllTodoPocos);
        group.MapGet("/incompleted", GetAllIncompletedTodoPocos);
        group.MapGet("/{id}", GetTodoPoco);

        group.MapPost("/", CreateTodoPoco)
            .AddEndpointFilter(async (efiContext, next) =>
            {
                var param = efiContext.GetArgument<TodoDto>(0);

                var validationErrors = Utilities.IsValid(param);

                if (validationErrors.Any())
                {
                    return Results.ValidationProblem(validationErrors);
                }

                return await next(efiContext);
            });

        group.MapPut("/{id}", UpdateTodoPoco);
        group.MapDelete("/{id}", DeleteTodoPoco);

        return group;
    }

    // get all todos
    public static async Task<Ok<List<TodoPoco>>> GetAllTodoPocos(ITodoPocoService todoService)
    {
        var todos = await todoService.GetAll();
        return TypedResults.Ok(todos);
    }

    public static async Task<Ok<List<TodoPoco>>> GetAllIncompletedTodoPocos(ITodoPocoService todoService)
    {
        var todos = await todoService.GetIncompleteTodoPocos();
        return TypedResults.Ok(todos);
    }

    // get todo by id
    public static async Task<Results<Ok<TodoPoco>, NotFound>> GetTodoPoco(int id, ITodoPocoService todoService)
    {
        var todo = await todoService.Find(id);

        if (todo != null)
        {
            return TypedResults.Ok(todo);
        }

        return TypedResults.NotFound();
    }

    // create todo
    public static async Task<Created<TodoPoco>> CreateTodoPoco(TodoDto todo, ITodoPocoService todoService)
    {
        var newTodoPoco = new TodoPoco
        {
            Title = todo.Title,
            Description = todo.Description,
            IsDone = todo.IsDone
        };

        await todoService.Add(newTodoPoco);

        return TypedResults.Created($"/todos/v1/{newTodoPoco.Id}", newTodoPoco);
    }

    // update todo
    public static async Task<Results<Created<TodoPoco>, NotFound>> UpdateTodoPoco(TodoPoco todo, ITodoPocoService todoService)
    {
        var existingTodoPoco = await todoService.Find(todo.Id);

        if (existingTodoPoco != null)
        {
            existingTodoPoco.Title = todo.Title;
            existingTodoPoco.Description = todo.Description;
            existingTodoPoco.IsDone = todo.IsDone;

            await todoService.Update(existingTodoPoco);

            return TypedResults.Created($"/todos/v1/{existingTodoPoco.Id}", existingTodoPoco);
        }

        return TypedResults.NotFound();
    }

    // delete todo
    public static async Task<Results<NoContent, NotFound>> DeleteTodoPoco(int id, ITodoPocoService todoService)
    {
        var todo = await todoService.Find(id);

        if (todo != null)
        {
            await todoService.Remove(todo);
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }
}
