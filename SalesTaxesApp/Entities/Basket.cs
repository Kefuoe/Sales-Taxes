using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesTaxesApp.Entities
{
    public class Basket
    {
        public int Id { get; set; }
        public List<Item> Items { get; set; }
    }
}