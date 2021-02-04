﻿namespace MobileBanking_API.ViewModel
{
	public class Transaction
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