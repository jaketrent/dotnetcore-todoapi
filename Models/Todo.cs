using System.ComponentModel.DataAnnotations;

namespace HelloDotnetCoreApi.Models
{
  public class Todo
  {
    public int Id { get; set; }

    [Required]
    public string Description { get; set; }
  }
}