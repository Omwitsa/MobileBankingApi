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
				var member = db.CUBs.FirstOrDefault(m => m.MemberNo.ToUpper().Equals(transaction.SNo.ToUpper()));
				if (member == null)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, member not found"
					};
				member.ActualBalance = + transaction.Amount;
				
				db.CustomerBalances.Add(new CustomerBalance
				{
					IDNo = "",
					PayrollNo = "",
					AccName = "",
					AvailableBalance = 0,
					TransDescription = "",
					TransDate = DateTime.UtcNow.Date, 
					AccNO = "",
					valuedate = null,
					transType = "",
					Status = false
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

		[Route("withdraw")]
		public ReturnData Withdraw([FromBody] Transaction transaction)
		{
			try
			{
				var member = db.CUBs.FirstOrDefault(m => m.MemberNo.ToUpper().Equals(transaction.SNo.ToUpper()));
				if (member == null)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, member not found"
					};
				member.ActualBalance = - transaction.Amount;

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
