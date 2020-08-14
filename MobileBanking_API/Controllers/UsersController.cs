using MobileBanking_API.Models;
using MobileBanking_API.ViewModel;
using System;
using System.Linq;
using System.Web.Http;

namespace MobileBanking_API.Controllers
{
	[RoutePrefix("webservice/users")]
	public class UsersController : ApiController
    {
		kpillerEntities db;
		public UsersController()
		{
			db = new kpillerEntities();
		}

		[Route("seedAdminUser")]
		public ReturnData SeedAdminUser([FromBody] MobileBankingAdmin admin)
		{
			try
			{
				var members = db.MobileBankingAdmins.Add(admin);
				db.SaveChanges();
				return new ReturnData
				{
					Success = true,
					Message = "Account created successfully"
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

		[Route("login")]
		public ReturnData Login([FromBody] MobileBankingAdmin admin)
		{

			try
			{
				var isValidUser = db.MobileBankingAdmins.Any(u => u.Username.ToUpper().Equals(admin.Username.ToUpper()) && u.Password.Equals(admin.Password));
				if (!isValidUser)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, Invalid username or password"
					};
				return new ReturnData
				{
					Success = true,
					Message = "Logged in successfully"
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

		[Route("registerFingerPrints")]
		public ReturnData RegisterFingerPrints([FromBody] FingerPrintModel printModel)
		{
			try
			{
				var member = db.CUBs.FirstOrDefault(m => m.MemberNo.ToUpper().Equals(printModel.MemberNo.ToUpper()));
				member.FingerPrint = printModel.FingerPrint;
				db.SaveChanges();
				return new ReturnData
				{
					Success = true,
					Message = "finger print saved successfully"
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

		[Route("values")]
		public ReturnData Values()
		{
			try
			{
				var member = db.CUBs.FirstOrDefault();
				return new ReturnData
				{
					Success = true,
					Data = member
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
