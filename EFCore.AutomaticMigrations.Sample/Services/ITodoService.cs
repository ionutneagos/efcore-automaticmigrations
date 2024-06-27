using EFCore.AutomaticMigrations.Sample.Data.Entities;

namespace EFCore.AutomaticMigrations.Sample.Services;

public interface ITodoService
{
    Task<List<Data.Entities.Todo>> GetAll();

    Task<List<Data.Entities.Todo>> GetIncompleteTodos();

    ValueTask<Data.Entities.Todo?> Find(int id);

    Task Add(Data.Entities.Todo todo);

    Task Update(Data.Entities.Todo todo);

    Task Remove(Data.Entities.Todo todo);
}
