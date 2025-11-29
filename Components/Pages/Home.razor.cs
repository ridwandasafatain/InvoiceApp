using InvoiceApp.Model;
using InvoiceApp.Service;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace InvoiceApp.Components.Pages
{
    public class HomeBase : ComponentBase
    {
        [Inject]
        protected InvoiceService Service { get; set; } = new();

        protected List<InvoiceResult> results = new();
        protected RadzenDataGrid<InvoiceResult>? gridResults;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        protected async Task LoadData()
        {
            results = await Service.GetValidationResultsAsync();
        }

        protected void OnRowRender(RowRenderEventArgs<InvoiceResult> args)
        {
            if (!args.Data.IsValid)
            {
                if (args.Attributes.ContainsKey("class"))
                    args.Attributes["class"] += " invalid-row";
                else
                    args.Attributes["class"] = "invalid-row";
            }
        }
    }
}
