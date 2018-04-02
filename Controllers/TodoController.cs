using System.Collections.Generic;
using HelloDotnetCoreApi.Models;
using Microsoft.AspNetCore.Mvc;

public class Ok<T>{
  public T Data { get; set; }
}

[Route("api/[controller]")]
public class TodoController {
  [HttpGet]
  public Ok<Todo[]> list() => new Ok<Todo[]>{ Data = new Todo[] { new Todo { Desc = "Wow" }}};
}