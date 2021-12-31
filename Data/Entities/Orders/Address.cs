using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ECommerce.Data.Entities.Orders
{
    public class Addresss
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required]
        public string TownCity { get; set; }
        [Required]
        public string County { get; set; }
        [Required]
        public string Postcode { get; set; }
    }
}