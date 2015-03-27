using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuotationApp1
{
    public class Quotation
    {
        public int QuotationID { get; set; }

        [Required]
        public string Quote { get; set; }

        [Required]
        public string Author { get; set; }
        private DateTime date = DateTime.Now;
        public DateTime Date { get{return date;} set{date = value;} }
        public virtual Category Category { get; set; }
        public int CategoryID { get; set; }
    }
}