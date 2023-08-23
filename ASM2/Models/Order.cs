namespace FPTBOOK_STORE.Models;
using System.ComponentModel.DataAnnotations;
using FPTBOOK_STORE.Areas.Identity.Data;

public class Order{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public int Status { get; set; }

    public string? FPTBOOKUserId { get; set; }
    public FPTBOOKUser? FPTBOOKUser { get; set; }

    public ICollection<OrderDetail>? OrderDetails { get; set; }
}