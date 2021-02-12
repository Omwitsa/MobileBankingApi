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
		[Route("adminLogin")]
		public ReturnData LoginModel([FromBody] LoginModel logindata)
		{
			try
			{
				
				
				//select the Admin status  from database
				var idposuser = $"Select IDNo from PosUsers  where IDNo='{logindata.IdNo}'and PosSerialNo='{logindata.MachineId}'and Active='1'";
				var posuser = db.Database.SqlQuery<string>(idposuser).FirstOrDefault();
				if (posuser != null && posuser !="")
				{
					var idposuser1 = $"Select Admin from PosUsers  where IDNo='{logindata.IdNo}'and PosSerialNo='{logindata.MachineId}'and Active='1'";
					bool posuser1 = db.Database.SqlQuery<bool>(idposuser1).FirstOrDefault();

					return new ReturnData
					{
						Success = true,
						Message = $"{posuser1}",
						
						
					};
				}
				else
				{
					
						return new ReturnData
						{
							Success = true,
							Message = "You are not Authorised as Administrator "

						};
				}
					
			}
			catch (Exception ex)
			{
				return new ReturnData
				{
					Success = false,
					Message = "Sorry, your identity could not be verified, contact System Administrator"
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
				var decimalFingerprint = int.Parse(printModel.FingerPrint, System.Globalization.NumberStyles.HexNumber);
				//verify idno in the database
				//var posUser = db.PosUsers.FirstOrDefault(a => a.IDNo == printModel.IdNo);
				//var posUser = $"Select IDNo PosUsers  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}'";
				var posUser = $"Select IDNO from useraccounts  where IDNo='{printModel.IdNo}' and PosAdmin='1'";
				var posUsers = db.Database.SqlQuery<string>(posUser).FirstOrDefault();
				if (posUsers != null && posUsers != "")
				{
					//int decimalFingerprint = int.Parse(printModel.FingerPrint, System.Globalization.NumberStyles.HexNumber);

					
					var news = db.PosUsers.FirstOrDefault(a => a.IDNo == printModel.IdNo);
					if (news != null)
					{
						var posSmin = $"Select Fingerprint1 from PosUsers  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}'";
						var posSdminFingerprint = db.Database.SqlQuery<string>(posSmin).FirstOrDefault();
						if (posSdminFingerprint != null && posSdminFingerprint != "")
						{
							var posSminupdate = $"Update PosUsers set Fingerprint2='{decimalFingerprint}'  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}'";
							db.Database.ExecuteSqlCommand(posSminupdate);
							return new ReturnData
							{
								Success = true,
								Message = "Fingerprint2 updated Successfully"
							};
						}
						else
						{
							var posSSminupdate = $"Update PosUsers set Fingerprint1='{decimalFingerprint}'  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}'";
							db.Database.ExecuteSqlCommand(posSSminupdate);
							return new ReturnData
							{
								Success = true,
								Message = "Fingerprint1 updated Successfully"
							};
						}
						
					}
                    else 
					{
						var regist = db.UserAccounts.FirstOrDefault(a => a.IDNO == printModel.IdNo);
						var phone = db.MEMBERS.FirstOrDefault(a => a.IDNo == printModel.IdNo);
						var serial = db.PosAgents.FirstOrDefault(a => a.PosSerialNo == printModel.MachineId);

						db.PosUsers.Add(new PosUser
						{
							IDNo = printModel.IdNo,
							Name = regist.UserName,
							FingerPrint1 = "",
							PosSerialNo = printModel.MachineId,
							AgencyCode = serial.AgencyCode,
							PhoneNo = phone.MobileNo,
							Admin = true,
							Active = true,
							CreatedBy = regist.UserLoginID,
							FingerPrint2 = "",
							CreatedOn = DateTime.UtcNow.Date

						});

						db.SaveChanges();
						return new ReturnData
						{
							Success = true,
							Message = "Registration completed sucessfully"
						};
					}
				}
				//check existence of fingerprint in the PosUsers
				var posAmin = $"Select Fingerprint1 from PosUsers  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}'";
				var posAdminFingerprint = db.Database.SqlQuery<string>(posAmin).FirstOrDefault();
				if (posAdminFingerprint != null && posAdminFingerprint != "")
				{
					var posAminupdate = $"Update PosUsers set Fingerprint2='{decimalFingerprint}'  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}'";
					db.Database.ExecuteSqlCommand(posAminupdate);
					return new ReturnData
					{
						Success = true,
						Message = "Fingerprint2 Updated  sucessfully"
					};
				}
				else
				{
					var posAminupdates = $"Update PosUsers set Fingerprint1='{decimalFingerprint}'  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}'";
					db.Database.ExecuteSqlCommand(posAminupdates);

					

				}

				//check existence of fingerprint in the PosMembers
				var idposuser = $"Select IDNo from PosMembers  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}'";
				var possuser = db.Database.SqlQuery<string>(idposuser).FirstOrDefault();

				if (possuser != null && possuser != "")
				{
					var idposuserid = $"Select FingerPrint1 from PosMembers  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}'";
					var possuserid = db.Database.SqlQuery<string>(idposuserid).FirstOrDefault();
					if (possuserid != null && possuserid != "")
					{
						var posuserFingerprint = $"UPDATE PosMembers SET FingerPrint2 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}'";
						db.Database.ExecuteSqlCommand(posuserFingerprint);
						return new ReturnData
						{
							Success = true,
							Message = "Fingerprint2 Updated  sucessfully"
						};


					}
					else
					{
						var posuserFingerprint1 = $"UPDATE PosMembers SET FingerPrint1 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}'";
						db.Database.ExecuteSqlCommand(posuserFingerprint1);
						return new ReturnData
						{
							Success = true,
							Message = "Fingerprint2 Updated  sucessfully"
						};
					}

				}
				else
				{
					var newmember = db.MEMBERS.FirstOrDefault(a => a.IDNo == printModel.IdNo);
					var serial = db.PosAgents.FirstOrDefault(a => a.PosSerialNo == printModel.MachineId);

					db.PosMembers.Add(new PosMember
					{
						IDNo = printModel.IdNo,
						AuditID = "",
						RegistrationDate= DateTime.UtcNow.Date,
						FingerPrint1 = "",
						PosSerialNo = printModel.MachineId,
						AgencyCode = serial.AgencyCode,
						Active = true,
						FingerPrint2 = "",
						

					});

					db.SaveChanges();
					return new ReturnData
					{
						Success = true,
						Message = "Registration completed sucessfully"
					};

				}

				
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
