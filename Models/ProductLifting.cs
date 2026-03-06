using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopManagementSystem.Models
{
    public class ProductLifting
    {
        [Key]
        public int ProductLiftingId { get; set; }

        [Required]
        [ForeignKey(nameof(Product))]
        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [Required]
        public int Unit { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Cost Price")]
        public decimal CostPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "MRP")]
        public decimal MRP { get; set; }

        [Required]
        [Display(Name = "Lifting Date")]
        [Column(TypeName = "date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LiftingDate { get; set; }

        public virtual Product Product { get; set; } = default!;

        public virtual ICollection<SalesDetail> SalesDetails { get; set; } = new List<SalesDetail>();

        public virtual ICollection<ProductLiftingHistory> ProductLiftingHistories { get; set; } = new List<ProductLiftingHistory>();

    }
}
