using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SalesTaxesApp.Services;

namespace SalesTaxesApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly ReceiptGenerator _receiptGenerator;

        public IndexModel(ILogger<IndexModel> logger, ReceiptGenerator receiptGenerator)
        {
            _logger = logger;
            _receiptGenerator = receiptGenerator;
        }

        [BindProperty]
        public string InputBasket { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var receipt = await _receiptGenerator.GenerateReceiptAsync(InputBasket);
                if (receipt == null)
                {
                    return Page();
                }
                return RedirectToPage("Receipt", new { id = receipt.Id });
            }
            catch (Exception ex)
            {
                // Log the exception using the logger
                _logger.LogError(ex, "An error occurred while processing the input basket");

                // Redirect to an error page
                return RedirectToPage("Error"); // Redirect to an error page
            }
        }
    }
}