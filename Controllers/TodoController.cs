using System.Collections.Generic;
using HelloDotnetCoreApi.Models;
using HelloDotnetCoreApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;

public class Ok<T>
{
  public T Data { get; set; }
}

namespace HelloDotnetCoreApi.Controllers
{
  [Route("api/[controller]")]
  public class TodoController
  {
    private IConfiguration _config;
    private ITodoRepository _repo;

    public TodoController(IConfiguration config, ITodoRepository repo)
    {
      _config = config;
      _repo = repo;
    }

    [HttpGet]
    public Ok<Todo[]> list()
    {

      using (var conn = new NpgsqlConnection(_config.GetConnectionString("todo")))
      {
        conn.Open();

        var todos = new List<Todo>();
        using (var cmd = new NpgsqlCommand("select id, description from todo", conn))
        using (var reader = cmd.ExecuteReader())
          while (reader.Read())
            todos.Add(new Todo { Id = reader.GetInt32(0), Description = reader.GetString(1) });

        return new Ok<Todo[]> { Data = todos.ToArray() };
      }

    }

    [HttpPost]
    public Ok<Todo[]> create([FromBody] TodoCreateModel viewModel)
    {
      var todo = new Todo { Description = viewModel.Description };
      var newTodo = _repo.create(todo);

      return new Ok<Todo[]> { Data = new[] { newTodo } };

    }
  }
}