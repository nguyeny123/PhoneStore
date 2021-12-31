using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Data.Entities
{
public class AppRole : IdentityRole<int>
{
    public AppRole() { }
    public AppRole(string name)
    {
        Name = name;
    }
}
}