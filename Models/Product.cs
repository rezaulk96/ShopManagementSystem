using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopManagementSystem.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(30)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = default!;

        public virtual ICollection<ProductLifting> ProductLiftings { get; set; } = new List<ProductLifting>();

        public virtual ICollection<ProductLiftingHistory> ProductLiftingHistories { get; set; } = new List<ProductLiftingHistory>();
    }
}
