﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class kpillerEntities : DbContext
    {
        public kpillerEntities()
            : base("name=kpillerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Advance> Advances { get; set; }
        public virtual DbSet<Advert> Adverts { get; set; }
        public virtual DbSet<APPRAISAL> APPRAISALs { get; set; }
        public virtual DbSet<ATM_TR> ATM_TR { get; set; }
        public virtual DbSet<Branch_Date> Branch_Dates { get; set; }
        public virtual DbSet<CASHINCOME> CASHINCOMEs { get; set; }
        public virtual DbSet<CASHPAYMENT> CASHPAYMENTS { get; set; }
        public virtual DbSet<CASHPROCEEDSMEMBER> CASHPROCEEDSMEMBERS { get; set; }
        public virtual DbSet<cheqcomm> cheqcomms { get; set; }
        public virtual DbSet<ChequeDeposit> ChequeDeposits { get; set; }
        public virtual DbSet<CHEQUE> CHEQUES { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Clients_Accounts> Clients_Accounts { get; set; }
        public virtual DbSet<COMMISSIONSETUP> COMMISSIONSETUPs { get; set; }
        public virtual DbSet<CONTRIB> CONTRIBs { get; set; }
        public virtual DbSet<CRB_Cleared_Loans> CRB_Cleared_Loans { get; set; }
        public virtual DbSet<CUB> CUBs { get; set; }
        public virtual DbSet<CustomerBalance> CustomerBalances { get; set; }
        public virtual DbSet<Declined_Subscription> Declined_Subscriptions { get; set; }
        public virtual DbSet<DEDUCTION> DEDUCTIONs { get; set; }
        public virtual DbSet<DEPARTMENT> DEPARTMENTS { get; set; }
        public virtual DbSet<DisbursementDeduction> DisbursementDeductions { get; set; }
        public virtual DbSet<dtproperty> dtproperties { get; set; }
        public virtual DbSet<Edited_Loan> Edited_Loans { get; set; }
        public virtual DbSet<EditedLoanStandingOrder> EditedLoanStandingOrders { get; set; }
        public virtual DbSet<FDR> FDRs { get; set; }
        public virtual DbSet<FirstTime_Withdrawal> FirstTime_Withdrawals { get; set; }
        public virtual DbSet<FROZENACC> FROZENACCs { get; set; }
        public virtual DbSet<GLTRANSACTION> GLTRANSACTIONS { get; set; }
        public virtual DbSet<INCOME> INCOMEs { get; set; }
        public virtual DbSet<LCharge> LCharges { get; set; }
        public virtual DbSet<LOANBAL> LOANBALs { get; set; }
        public virtual DbSet<LOANGUAR> LOANGUARs { get; set; }
        public virtual DbSet<LoanRange> LoanRanges { get; set; }
        public virtual DbSet<LOAN> LOANS { get; set; }
        public virtual DbSet<MDEDUCT> MDEDUCTs { get; set; }
        public virtual DbSet<MEMBERDEDUCTION> MEMBERDEDUCTIONS { get; set; }
        public virtual DbSet<MEMBER> MEMBERS { get; set; }
        public virtual DbSet<MEMBERS_DETAILS> MEMBERS_DETAILS { get; set; }
        public virtual DbSet<Membership_Detail> Membership_Details { get; set; }
        public virtual DbSet<MEMBERTRANSACTION> MEMBERTRANSACTIONS { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Mobile_Number_Change_Requests> Mobile_Number_Change_Requests { get; set; }
        public virtual DbSet<MONTHLYADVDEDUCTION> MONTHLYADVDEDUCTIONS { get; set; }
        public virtual DbSet<MONTHLYDEDUCTION> MONTHLYDEDUCTIONS { get; set; }
        public virtual DbSet<PIN_Lock_Request> PIN_Lock_Requests { get; set; }
        public virtual DbSet<PIN_Reset> PIN_Resets { get; set; }
        public virtual DbSet<PRODUCTINC> PRODUCTINCs { get; set; }
        public virtual DbSet<PRODUCTSETUP> PRODUCTSETUPs { get; set; }
        public virtual DbSet<REPAY> REPAYs { get; set; }
        public virtual DbSet<SessionUSSD> SessionUSSDs { get; set; }
        public virtual DbSet<SessionUSSD_BKP> SessionUSSD_BKP { get; set; }
        public virtual DbSet<SHARE> SHARES { get; set; }
        public virtual DbSet<sharetype> sharetypes { get; set; }
        public virtual DbSet<Subscriber> Subscribers { get; set; }
        public virtual DbSet<Transactionno> Transactionnoes { get; set; }
        public virtual DbSet<A> A { get; set; }
        public virtual DbSet<AccountCode> AccountCodes { get; set; }
        public virtual DbSet<AdvanceGuarantor> AdvanceGuarantors { get; set; }
        public virtual DbSet<AdvanceStatement> AdvanceStatements { get; set; }
        public virtual DbSet<AdvanceStmt> AdvanceStmts { get; set; }
        public virtual DbSet<agingsummary> agingsummaries { get; set; }
        public virtual DbSet<AP> APs { get; set; }
        public virtual DbSet<Asset_Depreciation> Asset_Depreciations { get; set; }
        public virtual DbSet<assetAllocation> assetAllocations { get; set; }
        public virtual DbSet<assetcode> assetcodes { get; set; }
        public virtual DbSet<assetDescription> assetDescriptions { get; set; }
        public virtual DbSet<AssetMovement> AssetMovements { get; set; }
        public virtual DbSet<asset> assets { get; set; }
        public virtual DbSet<assetStatu> assetStatus { get; set; }
        public virtual DbSet<assetstran> assetstrans { get; set; }
        public virtual DbSet<atmLog> atmLogs { get; set; }
        public virtual DbSet<AuditTable> AuditTables { get; set; }
        public virtual DbSet<AUDITTRAN> AUDITTRANS { get; set; }
        public virtual DbSet<BankRecon> BankRecons { get; set; }
        public virtual DbSet<BANK> BANKS { get; set; }
        public virtual DbSet<BFDEDUCT> BFDEDUCTs { get; set; }
        public virtual DbSet<branch> branches { get; set; }
        public virtual DbSet<BRIDGINGLOAN> BRIDGINGLOANs { get; set; }
        public virtual DbSet<budget> budgets { get; set; }
        public virtual DbSet<Capital_Adequacy_Return> Capital_Adequacy_Return { get; set; }
        public virtual DbSet<CAPITALAR> CAPITALARs { get; set; }
        public virtual DbSet<Cash_Shares_Fosa> Cash_Shares_Fosas { get; set; }
        public virtual DbSet<CashFlowAccount> CashFlowAccounts { get; set; }
        public virtual DbSet<cashflowActivity> cashflowActivities { get; set; }
        public virtual DbSet<cashflowheader> cashflowheaders { get; set; }
        public virtual DbSet<CashFlowStatement> CashFlowStatements { get; set; }
        public virtual DbSet<cashsplit> cashsplits { get; set; }
        public virtual DbSet<CASHSUMMARY> CASHSUMMARies { get; set; }
        public virtual DbSet<CashTransaction> CashTransactions { get; set; }
        public virtual DbSet<CASHZONE> CASHZONES { get; set; }
        public virtual DbSet<cheqdepositsetup> cheqdepositsetups { get; set; }
        public virtual DbSet<cincome> cincomes { get; set; }
        public virtual DbSet<COLLOANGUAR> COLLOANGUARs { get; set; }
        public virtual DbSet<COMMUNICATION> COMMUNICATIONs { get; set; }
        public virtual DbSet<Comprehensive_Income> Comprehensive_Income { get; set; }
        public virtual DbSet<contrib1> contrib1 { get; set; }
        public virtual DbSet<Cprofile> Cprofiles { get; set; }
        public virtual DbSet<CRB_Exemption> CRB_Exemptions { get; set; }
        public virtual DbSet<DAILYTRAN> DAILYTRANS { get; set; }
        public virtual DbSet<DEDUCTIONLIST> DEDUCTIONLISTs { get; set; }
        public virtual DbSet<DEDUCTION1> DEDUCTIONS1 { get; set; }
        public virtual DbSet<DEFAULTEDLOAN> DEFAULTEDLOANS { get; set; }
        public virtual DbSet<DefaultersMessage> DefaultersMessages { get; set; }
        public virtual DbSet<depreciationMethod> depreciationMethods { get; set; }
        public virtual DbSet<DIFFEREDLOANFORM> DIFFEREDLOANFORMS { get; set; }
        public virtual DbSet<EasyMobi_Mobile_Numbers_Update> EasyMobi_Mobile_Numbers_Updates { get; set; }
        public virtual DbSet<EmployeeDetail> EmployeeDetails { get; set; }
        public virtual DbSet<Employer> Employers { get; set; }
        public virtual DbSet<ENDMAIN> ENDMAINs { get; set; }
        public virtual DbSet<Exedutyfee> Exedutyfees { get; set; }
        public virtual DbSet<ExemptedLoanInterest> ExemptedLoanInterests { get; set; }
        public virtual DbSet<FDRrate> FDRrates { get; set; }
        public virtual DbSet<FILLEDLOANFORM> FILLEDLOANFORMS { get; set; }
        public virtual DbSet<fin_Postion_accounts> fin_Postion_accounts { get; set; }
        public virtual DbSet<Financial_Position> Financial_Position { get; set; }
        public virtual DbSet<Financialyear> Financialyears { get; set; }
        public virtual DbSet<finpos> finpos { get; set; }
        public virtual DbSet<FosaCharge> FosaCharges { get; set; }
        public virtual DbSet<FROZEN> FROZENs { get; set; }
        public virtual DbSet<FROZENBALANCE> FROZENBALANCEs { get; set; }
        public virtual DbSet<GLSETUP> GLSETUPs { get; set; }
        public virtual DbSet<GroupLoanBalance> GroupLoanBalances { get; set; }
        public virtual DbSet<Groupright> Grouprights { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupSharesBalance> GroupSharesBalances { get; set; }
        public virtual DbSet<GuarantorAmt> GuarantorAmts { get; set; }
        public virtual DbSet<IMPORT> IMPORTS { get; set; }
        public virtual DbSet<INCOMESOURCE> INCOMESOURCEs { get; set; }
        public virtual DbSet<incomeStatement_setup_accounts> incomeStatement_setup_accounts { get; set; }
        public virtual DbSet<Int> Ints { get; set; }
        public virtual DbSet<InterestTable> InterestTables { get; set; }
        public virtual DbSet<Investment_Return> Investment_Return { get; set; }
        public virtual DbSet<IR> IRs { get; set; }
        public virtual DbSet<jointMember> jointMembers { get; set; }
        public virtual DbSet<JV> JVs { get; set; }
        public virtual DbSet<KTDA_List> KTDA_Lists { get; set; }
        public virtual DbSet<LiquidityStatement> LiquidityStatements { get; set; }
        public virtual DbSet<LN> LNS { get; set; }
        public virtual DbSet<Loan_Sector> Loan_Sectors { get; set; }
        public virtual DbSet<Loan_Standingorder> Loan_Standingorder { get; set; }
        public virtual DbSet<loanaging> loanagings { get; set; }
        public virtual DbSet<loanagingdetailed> loanagingdetaileds { get; set; }
        public virtual DbSet<LoanAnalysi> LoanAnalysis { get; set; }
        public virtual DbSet<LOANARREAR> LOANARREARS { get; set; }
        public virtual DbSet<LoanGuarantor> LoanGuarantors { get; set; }
        public virtual DbSet<LoanOpeningBalance> LoanOpeningBalances { get; set; }
        public virtual DbSet<Loans_Classification> Loans_Classifications { get; set; }
        public virtual DbSet<LOANTYPE> LOANTYPEs { get; set; }
        public virtual DbSet<LOANTYPE_INTEREST> LOANTYPE_INTEREST { get; set; }
        public virtual DbSet<loantypeaging> loantypeagings { get; set; }
        public virtual DbSet<LoginAttempt> LoginAttempts { get; set; }
        public virtual DbSet<LOGIN> LOGINS { get; set; }
        public virtual DbSet<LSTMT> LSTMTs { get; set; }
        public virtual DbSet<Master> Masters { get; set; }
        public virtual DbSet<MDEDUCTNOTPOSTED> MDEDUCTNOTPOSTEDs { get; set; }
        public virtual DbSet<MDEDUCTTEMP> MDEDUCTTEMPs { get; set; }
        public virtual DbSet<MEMBERLOAN> MEMBERLOANS { get; set; }
        public virtual DbSet<Members_Extra_Obligations> Members_Extra_Obligations { get; set; }
        public virtual DbSet<MEMBERSTO> MEMBERSTOes { get; set; }
        public virtual DbSet<MEMBERTRAN> MEMBERTRANS { get; set; }
        public virtual DbSet<menu> menus { get; set; }
        public virtual DbSet<MicroArrear> MicroArrears { get; set; }
        public virtual DbSet<MicroDistributionSummary> MicroDistributionSummaries { get; set; }
        public virtual DbSet<MiniStat> MiniStats { get; set; }
        public virtual DbSet<Mpesa> Mpesas { get; set; }
        public virtual DbSet<NARRATION> NARRATIONS { get; set; }
        public virtual DbSet<NHIF> NHIFS { get; set; }
        public virtual DbSet<OtherLoanCharge> OtherLoanCharges { get; set; }
        public virtual DbSet<param> @params { get; set; }
        public virtual DbSet<PayBill_Reconcilliation> PayBill_Reconcilliations { get; set; }
        public virtual DbSet<PaymentBooking> PaymentBookings { get; set; }
        public virtual DbSet<PERTRAN> PERTRANs { get; set; }
        public virtual DbSet<port> ports { get; set; }
        public virtual DbSet<REASON> REASONS { get; set; }
        public virtual DbSet<ReceiptBooking> ReceiptBookings { get; set; }
        public virtual DbSet<Refund> Refunds { get; set; }
        public virtual DbSet<repay1> repay1 { get; set; }
        public virtual DbSet<reportpath> reportpaths { get; set; }
        public virtual DbSet<Risk_Classification> Risk_Classification { get; set; }
        public virtual DbSet<Sacco> Saccos { get; set; }
        public virtual DbSet<Salary> Salaries { get; set; }
        public virtual DbSet<SalaryTran> SalaryTrans { get; set; }
        public virtual DbSet<SASRA_Daily_Summaries> SASRA_Daily_Summaries { get; set; }
        public virtual DbSet<SavingsAccountsParameter> SavingsAccountsParameters { get; set; }
        public virtual DbSet<sharesback> sharesbacks { get; set; }
        public virtual DbSet<SharesOpeningBalance> SharesOpeningBalances { get; set; }
        public virtual DbSet<ShareWithdrawal> ShareWithdrawals { get; set; }
        public virtual DbSet<SHRVAR> SHRVARs { get; set; }
        public virtual DbSet<smsLoanGuarantor> smsLoanGuarantors { get; set; }
        public virtual DbSet<SMSSetting> SMSSettings { get; set; }
        public virtual DbSet<SMTDR> SMTDRs { get; set; }
        public virtual DbSet<STATEMENT> STATEMENTs { get; set; }
        public virtual DbSet<Stmt> Stmts { get; set; }
        public virtual DbSet<SubDepartment> SubDepartments { get; set; }
        public virtual DbSet<summarycollection> summarycollections { get; set; }
        public virtual DbSet<Swift_Messages> Swift_Messages { get; set; }
        public virtual DbSet<SwM> SwMs { get; set; }
        public virtual DbSet<sysincome> sysincomes { get; set; }
        public virtual DbSet<SYSPARAM> SYSPARAMs { get; set; }
        public virtual DbSet<t_x> t_x { get; set; }
        public virtual DbSet<TB> TBs { get; set; }
        public virtual DbSet<tbbalance> tbbalances { get; set; }
        public virtual DbSet<tbbalance1> tbbalance1 { get; set; }
        public virtual DbSet<tbl_ISOResults> tbl_ISOResults { get; set; }
        public virtual DbSet<tbl_menus> tbl_menus { get; set; }
        public virtual DbSet<tbl_userGroupmenus> tbl_userGroupmenus { get; set; }
        public virtual DbSet<tbl_usermenus> tbl_usermenus { get; set; }
        public virtual DbSet<Teller_Transaction> Teller_Transactions { get; set; }
        public virtual DbSet<TELLERSUMMARY> TELLERSUMMARies { get; set; }
        public virtual DbSet<TempGuar> TempGuars { get; set; }
        public virtual DbSet<TempStatement> TempStatements { get; set; }
        public virtual DbSet<TEST> TESTs { get; set; }
        public virtual DbSet<TMPDEDUCTION> TMPDEDUCTIONS { get; set; }
        public virtual DbSet<TransactionCode> TransactionCodes { get; set; }
        public virtual DbSet<TRANSACTION> TRANSACTIONS { get; set; }
        public virtual DbSet<TransCode> TransCodes { get; set; }
        public virtual DbSet<UNIT> UNITS { get; set; }
        public virtual DbSet<UserAccount> UserAccounts { get; set; }
        public virtual DbSet<UserGroupRight> UserGroupRights { get; set; }
        public virtual DbSet<UserGroupRights_Logs> UserGroupRights_Logs { get; set; }
        public virtual DbSet<VOUCHERNO> VOUCHERNOes { get; set; }
        public virtual DbSet<WCHARGE> WCHARGES { get; set; }
        public virtual DbSet<ZONE> ZONES { get; set; }
        public virtual DbSet<Agentmember> Agentmembers { get; set; }
        public virtual DbSet<PosDevice> PosDevices { get; set; }
        public virtual DbSet<PosUser> PosUsers { get; set; }
    }
}
