using MobileBanking_API.Models;
using MobileBanking_API.Utilities;
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
				var isMembers = db.MobileBankingAdmins.Any(a => a.Username.ToUpper().Equals(admin.Username.ToUpper()));
				if (!isMembers)
				{
					admin.Password = SecurePasswordHasher.Hash(admin.Password);
					db.MobileBankingAdmins.Add(admin);
				}
					

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
				var adminUser = db.MobileBankingAdmins.FirstOrDefault(u => u.Username.ToUpper().Equals(admin.Username.ToUpper()));
				if (adminUser == null)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, Invalid username or password"
					};

				if(!SecurePasswordHasher.Verify(admin.Password, adminUser.Password))
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, Invalid username or password"
					};

				if (!adminUser.MachineId.Equals(admin.MachineId))
					return new ReturnData
					{
						Success = false,
						Message = "Kindly use the device you were assigned"
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
	}
}
