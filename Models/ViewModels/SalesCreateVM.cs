namespace ShopManagementSystem.Models.ViewModels
{
    public class SalesCreateVM
    {
        public int SalesId { get; set; }
        public DateTime SalesDate { get; set; }
        public string UserName { get; set; } = string.Empty;

        public List<SalesDetailVM> Items { get; set; } = new();
        public decimal GrandTotal => Items.Sum(x => x.TotalPrice);
    }

    public class SalesDetailVM
    {
        public int ProductLiftingId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
