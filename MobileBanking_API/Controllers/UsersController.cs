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
                    admin.password = Decryptor.Decript_String(admin.password);
                    //admin.password = SecurePasswordHasher.Hash(admin.password);
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

        [Route("registerAgencyMember")]
		public ReturnData RegisterAgencyMember([FromBody] Agencymember agencymember)
		{
			try
			{
				if (string.IsNullOrEmpty(agencymember.idno) || string.IsNullOrEmpty(agencymember.MachineID))
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, kindly provide member data"
					};

				var posAgent = db.PosAgents.FirstOrDefault(a => a.IDNo == agencymember.idno);
				if (posAgent != null)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, Member already exist"
					};

                
				db.PosAgents.Add(new PosAgent
				{
					
					
				});
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
		[Route("registerAgentMember")]
		public ReturnData RegisterAgentMember([FromBody] Agentmember agent)
		{
			try
			{
				if (string.IsNullOrEmpty(agent.idno) || string.IsNullOrEmpty(agent.Surname))
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, kindly provide member data"
					};

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
				var adminUser = db.UserAccounts.FirstOrDefault(u => u.UserLoginID.ToUpper().Equals(admin.username.ToUpper()));
				if (adminUser == null)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, Invalid username or password"
					};

				if (!Decryptor.Decript_String (admin.password).Equals(adminUser.mPassword))

					return new ReturnData
					{
						Success = false,
						Message = "Sorry, Invalid username or password"
					};

				if (adminUser.PosAdmin.Equals(false))
					return new ReturnData
					{
						Success = false,
						Message = "You are not Authorised to use this device"
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
                
                
                //if (!string.IsNullOrEmpty(printModel.FingerPrint))
                var accmeber = $"Select FingerPrint1 from MEMBERS  where IDNo='{printModel.IdNo}'";
                var membeno = db.Database.SqlQuery<string>(accmeber).FirstOrDefault();
                var figuerPrintInfo = printModel.FingerPrint.Split('@');
                printModel.FingerPrint = figuerPrintInfo.Count() < 2 ? figuerPrintInfo[0] : figuerPrintInfo[1];
                int decimalFingerprint = int.Parse(printModel.FingerPrint, System.Globalization.NumberStyles.HexNumber);
                if (membeno != null)
                {
                    var fingerUpdateQuery = $"UPDATE MEMBERS SET FingerPrint = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'";
                    db.Database.ExecuteSqlCommand(fingerUpdateQuery);
                }
                else
                {
                 
                    var fingerUpdateQuery1 = $"UPDATE MEMBERS SET FingerPrint1 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'";
                    db.Database.ExecuteSqlCommand(fingerUpdateQuery1);
                }
			
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

		[Route("fetchUserAccounts")]
		public ReturnData FetchUserAccounts([FromBody] FingerPrintModel printModel)
		{
			try
			{
				var accounts = db.MEMBERS.Where(m => m.IDNo == printModel.IdNo).Select(m => m.AccNo).ToList();
				return new ReturnData
				{
					Success = true,
					Data = accounts
				};
			}
			catch (Exception)
			{
				return new ReturnData
				{
					Success = false,
					Message = "Sorry, An error occurred"
				};
			}
		}
		[Route("PosAdmin")]
		public ReturnData PosAdmin([FromBody] FingerPrintModel addModel)
		{
			try
			{
				var accounts = db.MEMBERS.Where(m => m.IDNo == addModel.IdNo).Select(m => m.AccNo).ToList();
				return new ReturnData
				{
					Success = true,
					Data = accounts
				};
			}
			catch (Exception)
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
