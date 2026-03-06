using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopManagementSystem.Models
{
    public class ProductLiftingHistory
    {
        [Key]
        public int Id { get; set; }

        // 🔹 FK to ProductLifting
        [Required]
        [ForeignKey(nameof(ProductLifting))]
        public int ProductLiftingId { get; set; }

        // 🔹 FK to Product
        [Required]
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public int PreviousUnit { get; set; }
        public int SoldQuantity { get; set; }
        public int RemainingUnit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MRP { get; set; }

        [Column(TypeName = "date")]
        public DateTime LiftingDate { get; set; }

        public DateTime SoldDate { get; set; } = DateTime.Now;

        // 🔹 Navigation Properties
        public virtual ProductLifting? ProductLifting { get; set; } = default!;
        public virtual Product? Product { get; set; } = default!;
    }
}
