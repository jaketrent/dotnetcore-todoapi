using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
      var query = "insert into todo (description) values (:description) returning id";
      IEnumerable<Todo> todos = _conn.WriteData(query, new TodoInsertRowMapper(todo), new { description = todo.Description });
      return todos.First();
    }

    private class TodoInsertRowMapper : IRowMapper<Todo>
    {
      private Todo _todo;

      public TodoInsertRowMapper(Todo todo)
      {
        _todo = todo;
      }
      public Todo mapRow(IDataReader reader)
      {
        _todo.Id = reader.GetInt32(0);
        return _todo;
      }
    }

  }
}