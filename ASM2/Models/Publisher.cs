namespace FPTBOOK_STORE.Models;
using System.ComponentModel.DataAnnotations;

public class Publisher{
    public int Id { get; set; }
    [Required(ErrorMessage = "Please type name of publisher !")]
    public string? Name { get; set; }
    public ICollection<Book>? Books { get; set; }
}