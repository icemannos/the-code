using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuotationApp1
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public virtual List<Quotation> QuoteList { get; set; }
    }
}