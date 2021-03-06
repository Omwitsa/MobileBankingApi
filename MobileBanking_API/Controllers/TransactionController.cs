﻿using MobileBanking_API.Models;
using MobileBanking_API.ViewModel;
using System;
using System.Linq;
using System.Web.Http;

namespace MobileBanking_API.Controllers
{
	[RoutePrefix("webservice/transacions")]
	public class TransactionController : ApiController
    {
		kpillerEntities db;
		public TransactionController()
		{
			db = new kpillerEntities();
		}

		[Route("deposit")]
		public ReturnData Deposit([FromBody] Transaction transaction)
		{
			try
			{
				var member = db.CUBs.FirstOrDefault(m => m.AccNo.ToUpper().Equals(transaction.SNo.ToUpper()));
				if (member == null)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, account number not found"
					};
				member.AvailableBalance += transaction.Amount;
				var transactionDescription = "Cash Deposit";

				var vNo = GetVoucherNo(transaction.Amount);
				db.CustomerBalances.Add(new CustomerBalance
				{
					IDNo = member.IDNo,
					PayrollNo = member.Payno,
					CustomerNo = member.Payno,
					vno = vNo,
					AccName = member.AccountName,
					Amount = transaction.Amount,
					MachineID = transaction.MachineID,
					TransactionNo = vNo,
					Auditid = transaction.AuditId,
					AvailableBalance = member.AvailableBalance,
					TransDescription = transactionDescription,
					TransDate = DateTime.UtcNow.Date,
					ReconDate = DateTime.UtcNow.Date,
					AccNO = member.AccNo,
					valuedate = DateTime.UtcNow.Date,
					transType = "CR",
					Status = true,
					Cash = true,
				});

				db.GLTRANSACTIONS.Add(new GLTRANSACTION {
					TransDate = DateTime.UtcNow.Date,
					Amount = transaction.Amount,
					DocumentNo = vNo,
					TransactionNo = vNo,
					AuditTime = DateTime.UtcNow.AddHours(3),
					AuditID = transaction.AuditId,
					DrAccNo = "827",
					CrAccNo = "942",
					TransDescript = transactionDescription,
					Source = member.MemberNo
				});

				db.SaveChanges();
				return new ReturnData
				{
					Success = true,
					Message = "Deposited sucessfully"
				};
			}
			catch (Exception ex)
			{
				return new ReturnData
				{
					Success = false,
					Message = "Sorry, An error occurred"
				};
			}
		}

		private string GetVoucherNo(decimal amount)
		{
			var query = "select TOP 1 customerbalanceid from CustomerBalance order by customerbalanceid desc";
			var bal = db.Database.SqlQuery<Int64>(query).FirstOrDefault();
			var vno = $"{bal}{decimal.ToInt32(amount)}";
			return vno;
		}

		[Route("withdraw")]
		public ReturnData Withdraw([FromBody] Transaction transaction)
		{
			try
			{
				var member = db.CUBs.FirstOrDefault(m => m.AccNo.ToUpper().Equals(transaction.SNo.ToUpper()));
				if (member == null)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, account number not found"
					};
				member.AvailableBalance -= transaction.Amount;
				if (member.AvailableBalance < 500)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, your account must remain with a minimum of KES. 500"
					};

				var transactionDescription = "Cash Withdraw";
				var vNo = GetVoucherNo(transaction.Amount);
				
				db.CustomerBalances.Add(new CustomerBalance
				{
					IDNo = member.IDNo,
					PayrollNo = member.Payno,
					CustomerNo = member.Payno,
					vno = vNo,
					AccName = member.AccountName,
					Auditid = transaction.AuditId,
					Amount = transaction.Amount,
					MachineID = transaction.MachineID,
					TransactionNo = vNo,
					AvailableBalance = member.AvailableBalance,
					TransDescription = transactionDescription,
					TransDate = DateTime.UtcNow.Date,
					ReconDate = DateTime.UtcNow.Date,
					AccNO = member.AccNo,
					valuedate = DateTime.UtcNow.Date,
					transType = "DR",
					Status = true,
					Cash = true,
				});

				var Withdrawal_Charges = 50;
				var saccoCommission = 0.7 * Withdrawal_Charges;
				var agentCommision = 0.3 * Withdrawal_Charges;

				db.GLTRANSACTIONS.Add(new GLTRANSACTION
				{
					TransDate = DateTime.UtcNow.Date,
					Amount = transaction.Amount,
					DocumentNo = vNo,
					TransactionNo = vNo,
					DrAccNo = "942",
					CrAccNo = "827",
					TransDescript = transactionDescription,
					AuditTime = DateTime.UtcNow.AddHours(3),
					AuditID = transaction.AuditId,
					AgentCommision = (decimal)agentCommision,
					SaccoCommision = (decimal)saccoCommission,
					Source = member.MemberNo
				});
				
				member.AvailableBalance -= Withdrawal_Charges;
				transactionDescription = "Whithdrawal Charges";
				db.CustomerBalances.Add(new CustomerBalance
				{
					IDNo = member.IDNo,
					PayrollNo = member.Payno,
					CustomerNo = member.Payno,
					vno = vNo,
					AccName = member.AccountName,
					Auditid = transaction.AuditId,
					Amount = Withdrawal_Charges,
					MachineID = transaction.MachineID,
					TransactionNo = vNo,
					AvailableBalance = member.AvailableBalance,
					TransDescription = transactionDescription,
					TransDate = DateTime.UtcNow.Date,
					ReconDate = DateTime.UtcNow.Date,
					AccNO = member.AccNo,
					valuedate = DateTime.UtcNow.Date,
					transType = "DR",
					Status = true,
					Cash = true
				});

				db.GLTRANSACTIONS.Add(new GLTRANSACTION
				{
					TransDate = DateTime.UtcNow.Date,
					Amount = Withdrawal_Charges,
					DocumentNo = vNo,
					TransactionNo = vNo,
					AuditTime = DateTime.UtcNow.AddHours(3),
					AuditID = transaction.AuditId,
					DrAccNo = "942",
					CrAccNo = "052",
					TransDescript = transactionDescription,
					Source = member.MemberNo
				});

				var Excise_duty = 10;
				member.AvailableBalance -= Excise_duty;
				transactionDescription = "Excise duty";
				db.CustomerBalances.Add(new CustomerBalance
				{
					IDNo = member.IDNo,
					PayrollNo = member.Payno,
					CustomerNo = member.Payno,
					vno = vNo,
					AccName = member.AccountName,
					Amount = Excise_duty,
					MachineID = transaction.MachineID,
					Auditid = transaction.AuditId,
					TransactionNo = vNo,
					AvailableBalance = member.AvailableBalance,
					TransDescription = transactionDescription,
					TransDate = DateTime.UtcNow.Date,
					ReconDate = DateTime.UtcNow.Date,
					AccNO = member.AccNo,
					valuedate = DateTime.UtcNow.Date,
					transType = "DR",
					Status = true,
					Cash = true
				});

				db.GLTRANSACTIONS.Add(new GLTRANSACTION
				{
					TransDate = DateTime.UtcNow.Date,
					Amount = Excise_duty,
					DocumentNo = vNo,
					TransactionNo = vNo,
					AuditTime = DateTime.UtcNow.AddHours(3),
					AuditID = transaction.AuditId,
					DrAccNo = "942",
					CrAccNo = "956",
					TransDescript = transactionDescription,
					Source = member.MemberNo
				});

				db.SaveChanges();
				return new ReturnData
				{
					Success = true,
					Message = "Withdrawn successfully"
				};
			}
			catch (Exception ex)
			{
				return new ReturnData
				{
					Success = false,
					Message = "Sorry, An error occurred"
				};
			}
		}

		[Route("balance")]
		public ReturnData Balance([FromBody] Transaction transaction)
		{
			try
			{
				var isMember = db.CustomerBalances.Any(b => b.AccNO.ToUpper().Equals(transaction.SNo.ToUpper()));
				if (!isMember)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, You details could not be found"
					};

				var debtQuery = $"SELECT ISNULL(Sum(Amount),0) FROM CUSTOMERBALANCE WHERE AccNO = '{transaction.SNo}' AND cash = 1 AND transtype='DR' AND TransDescription != 'Cheque Dep(uncleared)' AND TransDescription != 'Bounced Cheque'";
				var debts = db.Database.SqlQuery<decimal>(debtQuery).FirstOrDefault();
				var creditQuery = $"SELECT ISNULL(Sum(Amount),0) FROM CUSTOMERBALANCE WHERE AccNO = '{transaction.SNo}' AND cash = 1 AND transtype='CR' AND TransDescription != 'Cheque Dep(uncleared)'";
				var credits = db.Database.SqlQuery<decimal>(creditQuery).FirstOrDefault();

				var balance = credits - debts;
				return new ReturnData
				{
					Success = true,
					Message = $"{balance}",
					Data = balance,
				};
			}
			catch (Exception ex)
			{
				return new ReturnData
				{
					Success = false,
					Message = "Sorry, An error occurred"
				};
			}
		}

		[Route("applyAdvance")]
		public ReturnData ApplyAdvance([FromBody] Transaction transaction)
		{
			try
			{
				var incomeQuery = $"SELECT Amount FROM INCOME WHERE AccNo = '{transaction.SNo}' AND Period > (SELECT DATEADD(month, -3, GETDATE()))";
				var threeMonthsIncome = db.Database.SqlQuery<decimal>(incomeQuery).Sum();
				var averageIncome = 0m;
				if (threeMonthsIncome > 0)
					averageIncome = threeMonthsIncome / 3;

				var advanceBalQuery = $"SELECT Amount FROM DEDUCTION WHERE Amount > 0 AND ProductID IN (SELECT ProductID FROM INCOME WHERE AccNo = '{transaction.SNo}' AND Period > (SELECT DATEADD(month, -3, GETDATE())))";
				var threeMonthsAdvanceBal = db.Database.SqlQuery<decimal>(advanceBalQuery).Sum();
				var averageAdvanceBal = 0m;
				if (threeMonthsAdvanceBal > 0)
					averageAdvanceBal = threeMonthsAdvanceBal / 3;

				var advanceArreaersQuery = $"SELECT Arrears FROM DEDUCTION WHERE Amount > 0 AND ProductID IN (SELECT ProductID FROM INCOME WHERE AccNo = '{transaction.SNo}' AND Period > (SELECT DATEADD(month, -3, GETDATE())))";
				var threeMonthsAdvanceArrears = db.Database.SqlQuery<decimal>(advanceArreaersQuery).Sum();
				var averageAdvanceArrears = 0m;
				if (threeMonthsAdvanceArrears > 0)
					averageAdvanceArrears = threeMonthsAdvanceArrears / 3;

				var repayRateQuery = $"SELECT RepayRate FROM LOANBAL WHERE ACCNO = '{transaction.SNo}'";
				var repayRate = db.Database.SqlQuery<decimal>(repayRateQuery).Sum();

				var loanArrearsQuery = $"SELECT arrears FROM LOANARREARS WHERE AccNo = '{transaction.SNo}'";
				var loanArrears = db.Database.SqlQuery<decimal>(loanArrearsQuery).Sum();

				var advance = averageIncome - averageAdvanceBal - averageAdvanceArrears - repayRate - loanArrears;
				if (transaction.Amount < 200)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, minimum advance amount is KES 200"
					};

				if (transaction.Amount > advance)
					return new ReturnData
					{
						Success = false,
						Message = $"Sorry, your maximum advance amount is KES {advance}"
					};

				var member = db.CUBs.FirstOrDefault(m => m.AccNo.ToUpper().Equals(transaction.SNo.ToUpper()));
				if (member == null)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, account number not found"
					};

				db.Advances.Add(new Advance {
					accno = transaction.SNo,
					appamnt = transaction.Amount,
					payrollno = member.Payno,
					advdate = DateTime.UtcNow.Date,
					auditid = transaction.AuditId,
					serialno = transaction.MachineID,
					audittime = DateTime.UtcNow.AddHours(3)
				});

				db.SaveChanges();

				return new ReturnData
				{
					Success = true,
					Message = $"You have successfully applied for an advance of KES {transaction.Amount}"
				};
			}
			catch (Exception ex)
			{
				return new ReturnData
				{
					Success = false,
					Message = "Sorry, An error occurred"
				};
			}
		}
	}
}
