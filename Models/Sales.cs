using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopManagementSystem.Models
{
    public class Sales
    {
        [Key]
        public int SalesId { get; set; }

        public int Unit { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Sales Date")]
        public DateTime SalesDate { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Grand Total")]
        public decimal GrandTotal { get; set; }

        [Display(Name ="User Name")]
        public string UnserName { get; set; } = string.Empty;

        public virtual ICollection<SalesDetail> SalesDetails { get; set; } = new List<SalesDetail>();
    }
}
