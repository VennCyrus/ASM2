namespace FPTBOOK_STORE.Models;
using System.ComponentModel.DataAnnotations;
public class Book
{
    public int Id { get; set; }
    [Required(ErrorMessage = "This box is not empty")]
    public string? Name { get; set; }

    [Range(1000, 30000000, ErrorMessage = "Price must be over 1000")]
    [Required(ErrorMessage = "This box is not empty")]
    public double Price { get; set; }
    [Required(ErrorMessage = "This box is not empty")]
    public string Description { get; set; }

    [DataType(DataType.ImageUrl)]
    public string? UploadImage { get; set; }
 
    public int AuthorID { get; set; }
    public Author? Author { get; set; }
 
    public int CategoryID { get; set; }
    public Category? Category { get; set; }
  
    public int PublisherID { get; set; }
    public Publisher? Publisher { get; set; }
    public ICollection<OrderDetail>? OrderDetails { get; set; }
}