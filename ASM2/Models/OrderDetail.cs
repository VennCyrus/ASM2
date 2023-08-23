namespace FPTBOOK_STORE.Models;
using System.ComponentModel.DataAnnotations;

public class OrderDetail{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public int OrderID { get; set; }
    public Order? Order { get; set; }
    public int BookID { get; set; }
    public Book? Book { get; set; }


}