//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MobileBanking_API.Models
{
    using System;
    
    public partial class Get_Staff_Loan_Arrears_Result
    {
        public System.DateTime AsAt { get; set; }
        public string MemberNo { get; set; }
        public string AccNo { get; set; }
        public string LoanNo { get; set; }
        public decimal Arrears { get; set; }
        public string Category { get; set; }
        public decimal Expected { get; set; }
        public decimal InterestPaid { get; set; }
        public decimal PrincipalPaid { get; set; }
        public System.DateTime LoanDate { get; set; }
        public decimal intArrears { get; set; }
        public int DaysInArrears { get; set; }
    }
}
