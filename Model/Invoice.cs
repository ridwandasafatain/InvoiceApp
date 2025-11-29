namespace InvoiceApp.Model
{
    public class Invoice
    {
        public string InvoiceID { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }

    public class InvoiceDetail
    {
        public string InvoiceID { get; set; } = string.Empty;
        public int Qty { get; set; }
        public decimal Price { get; set; }
    }

    public class InvoiceResult
    {
        public string InvoiceID { get; set; } = string.Empty;
        public decimal SystemTotal { get; set; }
        public decimal CalculatedTotal { get; set; }
        public bool IsValid => SystemTotal == CalculatedTotal;
    }
}
