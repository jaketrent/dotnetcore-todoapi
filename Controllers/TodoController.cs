using System.Collections.Generic;
using HelloDotnetCoreApi.Models;
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

    public TodoController(IConfiguration config)
    {
      _config = config;
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
  }
}