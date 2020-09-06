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
		public ReturnData SeedAdminUser([FromBody] PosUser admin)
		{
			try
			{
				var isMembers = db.PosUsers.Any(a => a.username.ToUpper().Equals(admin.username.ToUpper()));
				if (!isMembers)
				{
					admin.password = SecurePasswordHasher.Hash(admin.password);
					db.PosUsers.Add(admin);
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

		[Route("registerAgentMember")]
		public ReturnData RegisterAgentMember([FromBody] Agentmember agent)
		{
			try
			{
				var agentMember = db.Agentmembers.FirstOrDefault(a => a.idno == agent.idno);
				if (agentMember != null)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, Member already exist"
					};

                db.Agentmembers.Add(agent);
                  db.SaveChanges();
				return new ReturnData
				{
					Success = true,
					Message = "Member Registered successfully"
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
		public ReturnData Login([FromBody] PosUser admin)
		{
			try
			{
				var adminUser = db.PosUsers.FirstOrDefault(u => u.username.ToUpper().Equals(admin.username.ToUpper()));
				if (adminUser == null)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, Invalid username or password"
					};

				if (!SecurePasswordHasher.Verify(admin.password, adminUser.password))
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
