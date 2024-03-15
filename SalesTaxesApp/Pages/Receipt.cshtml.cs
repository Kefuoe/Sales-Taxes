using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
using SalesTaxesApp.Data;
//using SalesTaxesApp.Services;
using SalesTaxesApp.Entities;

namespace SalesTaxesApp.Pages
{
    public class ReceiptModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly DataContext _context;

        public ReceiptModel (DataContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Receipt Receipt { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                Receipt = await _context.Receipts
                    .Include(p => p.Basket)
                        .ThenInclude(b => b.Items)
                    .FirstOrDefaultAsync(r => r.Id == id);


                if (Receipt == null)
                {
                    return NotFound();
                }
                return Page();
            }
            catch (Exception ex)
            {
                // Log the exception using the logger
                _logger.LogError(ex, "An error occurred while processing the receipt");

                // Return a simple error message 
                return Content("An error occurred while processing the input basket."); 
            }
        }

    }
}