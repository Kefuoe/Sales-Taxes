using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTaxesApp.Entities
{
    public class Receipt
    {
        public int Id { get; set; }
        public Basket Basket { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalTax { get; set; }
    }
}