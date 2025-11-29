using InvoiceApp.Model;

namespace InvoiceApp.Service
{
    public class InvoiceService
    {
        public Task<List<InvoiceResult>> GetValidationResultsAsync()
        {
            var invoices = GetInvoices();
            var details = GetInvoiceDetails();

            var results = ValidateInvoices(invoices, details);
            return Task.FromResult(results);
        }

        private List<InvoiceResult> ValidateInvoices(List<Invoice> invoices, List<InvoiceDetail> details)
        {
            var results = new List<InvoiceResult>();

            var groupedDetails = details
                .GroupBy(d => d.InvoiceID)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Qty * x.Price));

            foreach (var inv in invoices)
            {
                groupedDetails.TryGetValue(inv.InvoiceID, out var calc);

                results.Add(new InvoiceResult
                {
                    InvoiceID = inv.InvoiceID,
                    SystemTotal = inv.TotalAmount,
                    CalculatedTotal = calc
                });
            }

            return results;
        }

        private List<Invoice> GetInvoices()
        {
            return new List<Invoice>
            {
                new Invoice { InvoiceID = "INV001", TotalAmount = 250000 },
                new Invoice { InvoiceID = "INV002", TotalAmount = 90000 },
                new Invoice { InvoiceID = "INV003", TotalAmount = 180000 },
                new Invoice { InvoiceID = "INV004", TotalAmount = 200000 },
            };
        }

        private List <InvoiceDetail> GetInvoiceDetails()
        {
            return new List<InvoiceDetail>
            {
                new InvoiceDetail { InvoiceID = "INV001", Qty = 2, Price = 125000 },
                new InvoiceDetail { InvoiceID = "INV002", Qty = 1, Price = 90000 },
                new InvoiceDetail { InvoiceID = "INV003", Qty = 2, Price = 90000 },
                new InvoiceDetail { InvoiceID = "INV004", Qty = 2, Price = 110000 },
            };
        }
    }
}
