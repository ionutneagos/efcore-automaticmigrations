using Microsoft.EntityFrameworkCore;
using EFCore.AutomaticMigrations.Sample.Data;
using EFCore.AutomaticMigrations.Sample.Data.Entities;

namespace EFCore.AutomaticMigrations.Sample.Services;

public class TodoPocoService(TodoDbContext dbContext) : ITodoPocoService
{
    private readonly TodoDbContext _dbContext = dbContext;

    public async ValueTask<TodoPoco?> Find(int id)
    {
        return await _dbContext.TodosPoco.FindAsync(id);
    }

    public async Task<List<TodoPoco>> GetAll()
    {
        return await _dbContext.TodosPoco.ToListAsync();
    }

    public async Task Add(TodoPoco todo)
    {
        await _dbContext.TodosPoco.AddAsync(todo);
    }

    public async Task Update(TodoPoco todo)
    {
        _dbContext.TodosPoco.Update(todo);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Remove(TodoPoco todo)
    {
        _dbContext.TodosPoco.Remove(todo);
        await _dbContext.SaveChangesAsync();
    }

    public Task<List<TodoPoco>> GetIncompleteTodoPocos()
    {
        return _dbContext.TodosPoco.Where(t => t.IsDone == false).ToListAsync();
    }
}
