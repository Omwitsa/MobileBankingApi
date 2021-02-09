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
		TESTEntities db;
		public UsersController()
		{
			db = new TESTEntities();
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

                var posAgent = db.PosAgents.FirstOrDefault(a => a.AgencyName == agencymember.agency);
                if (posAgent == null)
                    return new ReturnData
                    {
                        Success = false,
                        Message = "Sorry, Member already exist"
                    };

				db.PosUsers.Add(new PosUser
				{
					IDNo = agencymember.idno,
					Name = agencymember.names,
					AgencyCode = posAgent.AgencyCode,
					PhoneNo = agencymember.phone,
					Active = true,
					FingerPrint1 = agencymember.Fingerprint,
					PosSerialNo = agencymember.MachineID,
					Admin = true,
					CreatedBy = agencymember.agentid,
					CreatedOn = DateTime.UtcNow.Date

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
                    Message = "Sorry, Your registration was Unsuccessfully,check your network connection"
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
		[Route("Login")]
		public ReturnData RegisterFingers([FromBody] RegisterFingers fingerprint)
		{
			try
			{
				
				
				//select the fingerprints from database
				var idposuser = $"Select fingerprint1 from PosUsers  where IDNo='{fingerprint.IdNo}'and PosSerialNo='{fingerprint.MachineId}'";
				var posuser = db.Database.SqlQuery<string>(idposuser).FirstOrDefault();
				if (posuser != null && posuser !="")
				{
					return new ReturnData
					{
						Success = true,
						Message = "FingerPrint Verification was successfully"
					};
				}
				else
				{
					var idposuser1 = $"Select fingerprint2 from PosUsers  where IDNo='{fingerprint.IdNo}'and PosSerialNo='{fingerprint.MachineId}'";
					var posuser1 = db.Database.SqlQuery<string>(idposuser1).FirstOrDefault();
					if (posuser1 != null && posuser1 != "")
					{
						return new ReturnData
						{
							Success = true,
							Message = "FingerPrint Verification was successfully"

						};
					}
					
					//verification for posMembers
					var posmember = $"Select fingerprint1 from PosMembers  where IDNo='{fingerprint.IdNo}'and PosSerialNo='{fingerprint.MachineId}'";
					var posmember1 = db.Database.SqlQuery<string>(posmember).FirstOrDefault();
					if (posmember1 != null && posmember1 != "")
					{
						return new ReturnData
						{
							Success = true,
							Message = "FingerPrint Verification was successfully"
						};
					}
					else
					{
						var idposuser2 = $"Select fingerprint2 from PosMembers  where IDNo='{fingerprint.IdNo}'and PosSerialNo='{fingerprint.MachineId}'";
						var posuser2 = db.Database.SqlQuery<string>(idposuser2).FirstOrDefault();
						if (posuser2 != null && posuser2 != "")
						{
							return new ReturnData
							{
								Success = true,
								Message = "FingerPrint Verification was successfully"

							};
						}
					}
					return new ReturnData
					{
						Success = true,
						Message = "Verification was successfully"

					};
				}
			}
			catch (Exception ex)
			{
				return new ReturnData
				{
					Success = false,
					Message = "Sorry, your identity could not be verified,please try again"
				};
			}
		}

//[Route("login")]
//public ReturnData Login([FromBody] PosUser admin)
//{
//	try
//	{
//		var adminUser = db.UserAccounts.FirstOrDefault(u => u.UserLoginID.ToUpper().Equals(admin.username.ToUpper()));
//		if (adminUser == null)
//			return new ReturnData
//			{
//				Success = false,
//				Message = "Sorry, Invalid username or password"
//			};

//		if (!Decryptor.Decript_String (admin.password).Equals(adminUser.mPassword))

//			return new ReturnData
//			{
//				Success = false,
//				Message = "Sorry, Invalid username or password"
//			};

//		if (adminUser.PosAdmin.Equals(false))
//			return new ReturnData
//			{
//				Success = false,
//				Message = "You are not Authorised to use this device"
//			};

//		return new ReturnData
//		{
//			Success = true,
//			Message = "Logged in successfully"
//		};
//	}
//	catch (Exception ex)
//	{
//		return new ReturnData
//		{
//			Success = false,
//			Message = "Sorry, An error occurred"
//		};
//	}
//}
///
//registerFingers
[Route("registerFingerPrints")]
        public ReturnData RegisterFingerPrints([FromBody] FingerPrintModel printModel)
		{
			try
			{
				//if (!string.IsNullOrEmpty(printModel.FingerPrint))
				var figuerPrintInfo = printModel.FingerPrint.Split('@');
				printModel.FingerPrint = figuerPrintInfo.Count() < 2 ? figuerPrintInfo[0] : figuerPrintInfo[1];
				int decimalFingerprint = int.Parse(printModel.FingerPrint, System.Globalization.NumberStyles.HexNumber);
				//verify idno in the database
				var posUser = db.PosUsers.FirstOrDefault(a => a.IDNo == printModel.IdNo);
				if (posUser != null)
				{

					
					//check existence of id in the PosUser
					var idposuser = $"Select FingerPrint1 from PosUsers  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}'";
					var possuser = db.Database.SqlQuery<string>(idposuser).FirstOrDefault();

					if (possuser != null && possuser != "")
					{
						var posuserFingerprint = $"UPDATE PosUsers SET FingerPrint2 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}'";
						db.Database.ExecuteSqlCommand(posuserFingerprint);
						return new ReturnData
						{
							Success = true,
							Message = "Fingerprint2 updated successfully"
						};
						
					}
					else
					{
						var posuserFingerprint1 = $"UPDATE PosUsers SET FingerPrint1 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}'";
						db.Database.ExecuteSqlCommand(posuserFingerprint1);

						return new ReturnData
						{
							Success = true,
							Message = "Fingerprint1 updated successfully"
						};
					}

					
				}


				var idsposmember = db.PosMembers.FirstOrDefault(a => a.IDNo == printModel.IdNo);
				if (idsposmember != null)

				{
					//checked existence  if id in PosMembers
					var idposmember = $"Select FingerPrint1 from posmembers  where IDNo='{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}'";
					var posmember = db.Database.SqlQuery<string>(idposmember).FirstOrDefault();
					if (posmember != null && posmember != "")
					{

						var posmemberFingerprint = $"UPDATE posmembers SET FingerPrint2 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}'";
						db.Database.ExecuteSqlCommand(posmemberFingerprint);
						var memberFingerprint1 = $"UPDATE members SET fingerprint2 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'";
						db.Database.ExecuteSqlCommand(memberFingerprint1);
						return new ReturnData
						{
							Success = true,
							Message = "Fingerprint2 updated successfully"
						};

					}
					else
					{
						var posuserFingerprint1 = $"UPDATE posmembers SET FingerPrint1 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}'";
						db.Database.ExecuteSqlCommand(posuserFingerprint1);
						var memberFingerprint2 = $"UPDATE members SET fingerprint1 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'";
						db.Database.ExecuteSqlCommand(memberFingerprint2);
						return new ReturnData
						{
							Success = true,
							Message = "Fingerprint1 updated successfully"
						};
					}

				}
				return new ReturnData
				{
					Success = true,
					Message = "Fingerprints updated successfully"
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
