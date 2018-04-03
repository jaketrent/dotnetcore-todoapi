using HelloDotnetCoreApi.Data;

namespace HelloDotnetCoreApi.Models
{
  public interface ITodoRepository
  {
    Todo create(Todo todo);
  }

  public class TodoRepository : ITodoRepository
  {
    private IPostgresConnection _conn;

    public TodoRepository(IPostgresConnection conn)
    {
      _conn = conn;
    }

    public Todo create(Todo todo)
    {
      var query = "insert into todo (description) values (:description)";
      _conn.WriteData(query, new { description = todo.Description });

      // TODO: get id back
      return todo;
    }
  }
}