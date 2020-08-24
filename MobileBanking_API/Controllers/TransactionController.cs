using MobileBanking_API.Models;
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
					Status = true
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
					Status = true
				});

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
					Source = member.MemberNo
				});

				var Withdrawal_Charges = 50;
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
					Status = true
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
					Status = true
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
	}
}
