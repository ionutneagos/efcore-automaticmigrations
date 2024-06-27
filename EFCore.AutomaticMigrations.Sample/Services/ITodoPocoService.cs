using EFCore.AutomaticMigrations.Sample.Data.Entities;
namespace EFCore.AutomaticMigrations.Sample.Services;

public interface ITodoPocoService
{
    Task<List<TodoPoco>> GetAll();

    Task<List<TodoPoco>> GetIncompleteTodoPocos();

    ValueTask<TodoPoco?> Find(int id);

    Task Add(TodoPoco todo);

    Task Update(TodoPoco todo);

    Task Remove(TodoPoco todo);
}
