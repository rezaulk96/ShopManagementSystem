using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopManagementSystem.Models
{
    public class SalesDetail
    {
        [Key]
        public int SalesDetailId { get; set; }

        [Required]
        [ForeignKey(nameof(Sales))]
        public int SalesId { get; set; }

        [Required]
        [ForeignKey(nameof(ProductLifting))]
        public int ProductLiftingId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; } // MRP snapshot

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        public virtual Sales Sales { get; set; } = default!;
        public virtual ProductLifting ProductLifting { get; set; } = default!;
    }
}
