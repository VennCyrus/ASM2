using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using FPTBOOK_STORE.Models;

namespace FPTBOOK_STORE.Areas.Identity.Data;

// Add profile data for application users by adding properties to the FPTBOOKUser class
public class FPTBOOKUser : IdentityUser
{
    public string Name { get; set; }
    public DateTime DOB { get; set; }
    public string Address {get;set;}
    public ICollection<Order>? Orders { get; set; }
}

