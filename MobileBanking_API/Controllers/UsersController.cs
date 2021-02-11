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


                //check if another admin exist based on option selected
                if (agencymember.Role == "Administrator")
                {
                    var posadmin2 = $"Select IDNo From PosUsers Where IDNo='{agencymember.idno}' Not in (Select IDNo='{agencymember.idno}' From UserAccounts Where POSAdmin ='1') AND Admin = '1' and Active = '1' and PosSerialNo = '{agencymember.MachineID}'";
                    var posadmin2idno = db.Database.SqlQuery<string>(posadmin2).FirstOrDefault();
                    if (posadmin2idno != null && posadmin2idno != "")
                    {
						return new ReturnData
						{
							Success = false,
							Message = "Sorry, You cannot be registered as Administrator"
						};


					}

                }
				bool Isadmin = false;
				if (agencymember.Role == "Administrator")
				{
					Isadmin = true;

                }

				var posAgent = db.PosAgents.FirstOrDefault(a => a.AgencyName == agencymember.agency);
				db.PosUsers.Add(new PosUser
				{
					IDNo = agencymember.idno,
					Name = agencymember.names,
					AgencyCode = posAgent.AgencyCode,
					PhoneNo = agencymember.phone,
					Active = true,
					FingerPrint1 = agencymember.Fingerprint,
					PosSerialNo = agencymember.MachineID,
					Admin = Isadmin,
					CreatedBy = agencymember.Operatorid,
					CreatedOn = DateTime.UtcNow.Date

				});

				db.SaveChanges();
                return new ReturnData
                {
                    Success = true,
                    Message = " Operator Registered successfully"
                };
            }
            catch (Exception ex)
            {
                return new ReturnData
                {
                    Success = false,
                    Message = "Sorry, Your registration was Unsuccessful"
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
				var role = db.PosUsers.Where(m => m.IDNo == fingerprint.IdNo).Select(m => m.Admin).ToList();
				if (posuser != null && posuser !="")
				{
					return new ReturnData
					{
						Success = true,
						Message = "FingerPrint Verification was successfully",
						Data=role
						
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
				//var posUser = db.PosUsers.FirstOrDefault(a => a.IDNo == printModel.IdNo);
				//var posUser = $"Select IDNo PosUsers  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}'";
				var posUser = $"Select IDNO from useraccounts  where IDNo='{printModel.IdNo}' and PosAdmin='1'";
				var posUsers = db.Database.SqlQuery<string>(posUser).FirstOrDefault();
				if (posUsers != null && posUsers != "")
				{

					var regist = db.UserAccounts.FirstOrDefault(a => a.IDNO == printModel.IdNo);
					var phone = db.MEMBERS.FirstOrDefault(a => a.IDNo == printModel.IdNo);
					var serial = db.PosAgents.FirstOrDefault(a => a.PosSerialNo == printModel.MachineId);
					db.PosUsers.Add(new PosUser
					{
						IDNo = printModel.IdNo,
						Name = regist.UserName,
						FingerPrint1 = printModel.FingerPrint,
						PosSerialNo = printModel.MachineId,
						AgencyCode = serial.AgencyCode,
						PhoneNo = phone.MobileNo,
						Admin = true,
						Active = true,
						CreatedBy = regist.UserLoginID,
						FingerPrint2 = ""

					});

					db.SaveChanges();
					return new ReturnData
					{
						Success = true,
						Message = "Registration completed sucessfully"
					};
				}
				//check existence of fingerprint in the PosUsers
				var posAmin = $"Select Fingerprint1 from PosUsers  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}'";
				var posAdminFingerprint = db.Database.SqlQuery<string>(posAmin).FirstOrDefault();
				if (posAdminFingerprint != null && posAdminFingerprint != "")
				{
					var posAminupdate = $"Update PosUsers set Fingerprint2='{decimalFingerprint}'  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}'";
					var posAdminFingerprintupdate = db.Database.SqlQuery<string>(posAminupdate).FirstOrDefault();
				}
				else
				{
					var posAminupdate = $"Update PosUsers set Fingerprint1='{decimalFingerprint}'  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}'";
					var posAdminFingerprintupdate = db.Database.SqlQuery<string>(posAminupdate).FirstOrDefault();
				}

				//check existence of fingerprint in the PosMembers
				var idposuser = $"Select FingerPrint1 from PosMembers  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}'";
				var possuser = db.Database.SqlQuery<string>(idposuser).FirstOrDefault();

				if (possuser != null && possuser != "")
				{
					var posuserFingerprint = $"UPDATE PosMembers SET FingerPrint2 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}'";
					db.Database.ExecuteSqlCommand(posuserFingerprint);


				}
				else
				{
					var posuserFingerprint1 = $"UPDATE PosMembers SET FingerPrint1 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}'";
					db.Database.ExecuteSqlCommand(posuserFingerprint1);


				}

				return new ReturnData
				{
					Success = false,
					Message = "Fingerprint Updated Successfully"
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
