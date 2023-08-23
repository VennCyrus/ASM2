namespace FPTBOOK_STORE.Models;
using System.ComponentModel.DataAnnotations;

public class Author {
    public int Id { get; set; }
    [Required(ErrorMessage = "Please type name of author !")]
    public string? Name { get; set; }
    public ICollection<Book>? Books { get; set; }
}