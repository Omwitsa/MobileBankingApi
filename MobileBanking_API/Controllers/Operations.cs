using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileBanking_API.Controllers
{
    public class Operations
    {
		public decimal Amount { get; set; }
		public string MachineID { get; set; }
		public string FingerePrint { get; set; }
		public string Pin { get; set; }
		public string SNo { get; set; }
		public string AuditId { get; set; }
		public string Operation { get; set; }
		public string ProductDescription { get; set; }
		public string AgencyName { get; set; }
		public string AccountNo { get; set; }
	}
}