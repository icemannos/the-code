using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuyAGrade.Models
{
    public class Customer
    {
        public String Name { get; set; }
        public String Course { get; set; }
        public String CardType { get; set; }
        public String CardNumber { get; set; }
        public Decimal AmountPaid { get; set; }
    }
}