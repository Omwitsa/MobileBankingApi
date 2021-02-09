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
		[Route("registerFingers")]
		public ReturnData RegisterFingers([FromBody] RegisterFingers fingerprint)
		{
			try
			{
				//if (!string.IsNullOrEmpty(printModel.FingerPrint))
				var figuerPrintInfo = fingerprint.FingerPrint.Split('@');
				fingerprint.FingerPrint = figuerPrintInfo.Count() < 2 ? figuerPrintInfo[0] : figuerPrintInfo[1];
				int decimalFingerprint = int.Parse(fingerprint.FingerPrint, System.Globalization.NumberStyles.HexNumber);
				//select the fingerprints from database
				var idposuser = $"Select fingerprint1 from PosUsers  where IDNo='{fingerprint.IdNo}'";
				int posuser = db.Database.SqlQuery<int>(idposuser).FirstOrDefault();
				if (posuser >= decimalFingerprint)
				{
					return new ReturnData
					{
						Success = true,
						Message = "Verification was successfully"
					};
				}
				else
				{
					var idposuser1 = $"Select fingerprint2 from PosUsers  where IDNo='{fingerprint.IdNo}'";
					int posuser1 = db.Database.SqlQuery<int>(idposuser1).FirstOrDefault();
					if (posuser1 >= decimalFingerprint)
					{
						return new ReturnData
						{
							Success = true,
							Message = "Verification was successfully"

						};
					}
					return new ReturnData
					{
						Success = true,
						Message = "Verification was successfully"
					};
					//verification for posMembers
					var posmember = $"Select fingerprint1 from PosMembers  where IDNo='{fingerprint.IdNo}'";
					int posmember1 = db.Database.SqlQuery<int>(posmember).FirstOrDefault();
					if (posmember1 >= decimalFingerprint)
					{
						return new ReturnData
						{
							Success = true,
							Message = "Verification was successfully"
						};
					}
					else
					{
						var idposuser2 = $"Select fingerprint2 from PosUsers  where IDNo='{fingerprint.IdNo}'";
						int posuser2 = db.Database.SqlQuery<int>(idposuser2).FirstOrDefault();
						if (posuser2 >= decimalFingerprint)
						{
							return new ReturnData
							{
								Success = true,
								Message = "Verification was successfully"

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
				//verify idno in the database
				var posUser = db.PosUsers.FirstOrDefault(a => a.IDNo == printModel.IdNo);
				if (posUser == null)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, id number doesnot exist"
					};
				//if (!string.IsNullOrEmpty(printModel.FingerPrint))
				var figuerPrintInfo = printModel.FingerPrint.Split('@');
				printModel.FingerPrint = figuerPrintInfo.Count() < 2 ? figuerPrintInfo[0] : figuerPrintInfo[1];
				int decimalFingerprint = int.Parse(printModel.FingerPrint, System.Globalization.NumberStyles.HexNumber);
				//check existence of id in the PosUser
				var idposuser= $"Select FingerPrint1 from PosUsers  where IDNo='{printModel.IdNo}'";
				var possuser = db.Database.SqlQuery<string>(idposuser).FirstOrDefault();
				if (possuser == null)
				{
					var posuserFingerprint = $"UPDATE PosUsers SET FingerPrint2 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'";
					db.Database.ExecuteSqlCommand(posuserFingerprint);
					return new ReturnData
					{
						Success = true,
						Message = "Fingerprint1 updated successfully"
					};
					//var posuserfingerprint = $"Select FingerPrint from PosUsers  where IDNo='{printModel.IdNo}'";
					//var posuserdata = db.Database.SqlQuery<string>(posuserfingerprint).FirstOrDefault();
				}
				else
				{
					var posuserFingerprint1 = $"UPDATE PosUsers SET FingerPrint1 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'";
					db.Database.ExecuteSqlCommand(posuserFingerprint1);

					return new ReturnData
					{
						Success = true,
						Message = "Fingerprint1 updated successfully"
					};
				}


				
                
				//checked existence  if id in PosMembers
				var idposmember = $"Select * from posmembers  where IDNo='{printModel.IdNo}'";
				var posmember = db.Database.SqlQuery<string>(idposmember).FirstOrDefault();
				if (posmember != null)
				{
					var posmemberfingerprint = $"Select FingerPrint1 from posmembers  where IDNo='{printModel.IdNo}'";
					var posmemberdata = db.Database.SqlQuery<string>(posmemberfingerprint).FirstOrDefault();

					if (posmemberdata != null)
					{
						var posmemberFingerprint = $"UPDATE posmembers SET FingerPrint2 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'";
						db.Database.ExecuteSqlCommand(posmemberFingerprint);
						var memberFingerprint1 = $"UPDATE members SET fingerprint2 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'";
						db.Database.ExecuteSqlCommand(memberFingerprint1);

					}
					else
					{
						var posuserFingerprint1 = $"UPDATE posmembers SET FingerPrint1 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'";
						db.Database.ExecuteSqlCommand(posuserFingerprint1);
						var memberFingerprint2 = $"UPDATE members SET fingerprint1 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'";
						db.Database.ExecuteSqlCommand(memberFingerprint2);
					}


				}
				else
				{
                    //var posAgent = db.PosAgents.FirstOrDefault(a => a.AgencyName == printModel.AgencyCode);
                    //if (posAgent == null)
                    //    return new ReturnData
                    //    {
                    //        Success = false,
                    //        Message = ""
                    //    };
                    //db.PosMembers.Add(new PosMember
                    //{
                    //    IDNo = printModel.IdNo,
                    //    FingerPrint1 = printModel.FingerPrint,
                    //    FingerPrint2 = "",
                    //    AuditID = printModel.AuditId,
                    //    RegistrationDate = DateTime.UtcNow.Date,
                    //    PosSerialNo = printModel.serialNo,
                    //    AgencyCode = posAgent.AgencyCode

                    //});

					var memberFingerprint = $"UPDATE members SET fingerprint1 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'";
					db.Database.ExecuteSqlCommand(memberFingerprint);
                }

				//Update members table with fingerprint
				var accmeber = $"Select Fingerprint1 from MEMBERS  where IDNo='{printModel.IdNo}'";
                var membeno = db.Database.SqlQuery<string>(accmeber).FirstOrDefault();
                
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
