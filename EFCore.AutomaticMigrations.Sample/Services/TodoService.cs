using EFCore.AutomaticMigrations.Sample.Data;
using EFCore.AutomaticMigrations.Sample.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCore.AutomaticMigrations.Sample.Services;

public class TodoService(TodoDbContext dbContext) : ITodoService
{
    private readonly TodoDbContext _dbContext = dbContext;

    public async ValueTask<Data.Entities.Todo?> Find(int id)
    {
        return await _dbContext.Todos.FindAsync(id);
    }

    public async Task<List<Data.Entities.Todo>> GetAll()
    {
        return await _dbContext.Todos.ToListAsync();
    }

    public async Task Add(Data.Entities.Todo todo)
    {
        await _dbContext.Todos.AddAsync(todo);
    }

    public async Task Update(Data.Entities.Todo todo)
    {
        _dbContext.Todos.Update(todo);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Remove(Data.Entities.Todo todo)
    {
        _dbContext.Todos.Remove(todo);
        await _dbContext.SaveChangesAsync();
    }

    public Task<List<Data.Entities.Todo>> GetIncompleteTodos()
    {
        return _dbContext.Todos.Where(t => t.IsDone == false).ToListAsync();
    }
}
