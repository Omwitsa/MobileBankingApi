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


		[Route("registerAgentMember")]
		public ReturnData RegisterAgentMember([FromBody] AgentNewMembers agent)
		{
			try
			{
				if (string.IsNullOrEmpty(agent.idno) || string.IsNullOrEmpty(agent.MachineId) || string.IsNullOrEmpty(agent.Surname) || string.IsNullOrEmpty(agent.other_Names) || string.IsNullOrEmpty(agent.DOB) || string.IsNullOrEmpty(agent.mobile_number) || string.IsNullOrEmpty(agent.Gender))
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, kindly provide all member data"
					};
				}


				var agentPosUser = $"select Name from PosUsers where IDNo='{agent.Agentid}' AND  PosSerialNo='{agent.MachineId}' AND Active=1";
				var agentPosUser1 = db.Database.SqlQuery<string>(agentPosUser).FirstOrDefault();
				//db.Agentmembers.Add(agent);

				//var agentMember = db.AgentMembers.FirstOrDefault(a => a.IDNo == agent.idno);
				var agentMember = $"select IDNo from AgentMembers where IDNo='{agent.idno}'";
				var posadmin2idno5 = db.Database.SqlQuery<string>(agentMember).FirstOrDefault();
				if (posadmin2idno5 != null)
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, Member already exist"
					};
				}


				var insertAgentMember = $"set dateformat dmy insert into AgentMembers(SurName,OtherNames,IDNo,MobileNo,Sex,DOB,PosSerialNo,FingerPrint1,FingerPrint2,AuditID,Registered)values('{ agent.Surname}','{agent.other_Names}','{ agent.idno}','{agent.mobile_number}','{agent.Gender}','{agent.DOB}','{ agent.MachineId}','{agent.FingerPrint1}','{ agent.FingerPrint2}','{agentPosUser1}','{false}')";
				db.Database.ExecuteSqlCommand(insertAgentMember);

				//Agent deposit commission
				var pullFunction2 = $"Select Expense_Amount From dbo.Get_POS_Expenses (0,'Registration')";
				decimal AgentRegistrationCommission = db.Database.SqlQuery<decimal>(pullFunction2).FirstOrDefault();
				//get voucher number
				var Vno = $"Select TOP 1 ID From Masters order by ID desc";
				Int64 getvno = db.Database.SqlQuery<Int64>(Vno).FirstOrDefault();
				Int64 nextvno = getvno + 1;
				var agentPosUser3 = $"select CommissionAccNo from PosAgents where PosSerialNo='{agent.MachineId}' and Active=1";
				var floatAcc = db.Database.SqlQuery<string>(agentPosUser3).FirstOrDefault();

				if (String.IsNullOrEmpty(floatAcc))
				{
					return new ReturnData
					{
						Success = false,
						Message = "You are not allowed to transact currently"
					};

				}
				else
				{
					//var OperatorName = db.PosUsers.FirstOrDefault(m => m.IDNo.ToUpper().Equals(agent.Agentid.ToUpper()));
					var agentPosUser30 = $"select Name from PosUsers where IDNo='{agent.Agentid}'  and Active=1";
					var OperatorName = db.Database.SqlQuery<string>(agentPosUser30).FirstOrDefault();

					string accno = "205";
					string message = "EasyAgent Member Registration Commission";


					var insertGltrans = $"set dateformat dmy insert into GLTRANSACTIONS(TransDate,Amount,DocumentNo,TransactionNo,DrAccNo,CrAccNo,TransDescript,AuditTime,AuditID,Source)values('{DateTime.UtcNow.Date}','{AgentRegistrationCommission}','{nextvno}','{nextvno}','{accno}','{floatAcc}','{message}','{DateTime.Now}','{OperatorName}','{ agent.idno}')";
					db.Database.ExecuteSqlCommand(insertGltrans);
				}


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
					Message = "Sorry, Network occurred"
				};
			}
		}

		[Route("registerAgencyMember")]
		public ReturnData RegisterAgencyMember([FromBody] Agencymember agencymember)
		{
			try
			{
				if (string.IsNullOrEmpty(agencymember.idno) || string.IsNullOrEmpty(agencymember.MachineID) || string.IsNullOrEmpty(agencymember.names) || string.IsNullOrEmpty(agencymember.phone))
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, kindly provide PosUser data"
					};

				//var posAgent = db.PosAgents.FirstOrDefault(a => a.AgencyName == agencymember.agency);

				var posadmin3 = $"Select AgencyCode From PosAgents Where PosSerialNo='{agencymember.MachineID}' and Active=1";
				var posadmin2idno4 = db.Database.SqlQuery<string>(posadmin3).FirstOrDefault();



				//check if another admin exist based on option selected
				if (agencymember.admins == "Administrator")
				{
					var posadmin2 = $"Select IDNo From PosUsers Where IDNo Not in (Select IDNo From UserAccounts Where POSAdmin=1) AND Admin=1 and Active=1 and PosSerialNo='{agencymember.MachineID}'";
					var posadmin2idno = db.Database.SqlQuery<string>(posadmin2).FirstOrDefault();
					if (posadmin2idno != null)
					{
						return new ReturnData
						{
							Success = false,
							Message = "Sorry, You cannot be registered as Administrator"
						};


					}

				}
				//var agentMember1 = db.PosUsers.FirstOrDefault(a => a.IDNo == agencymember.agentid);
				var posadmin4 = $"Select Name From PosUsers Where IDNo='{agencymember.agentid}'AND  PosSerialNo='{agencymember.MachineID}' and Active=1";
				var posadmin2idno5 = db.Database.SqlQuery<string>(posadmin4).FirstOrDefault();


				bool Isadmin = false;
				if (agencymember.admins == "Administrator")
				{
					Isadmin = true;

				}
				var posadminadd = $"Select IDNo From PosUsers Where IDNo='{agencymember.idno}' AND  PosSerialNo='{agencymember.MachineID}' and Active=1";
				var poscheckid = db.Database.SqlQuery<string>(posadminadd).FirstOrDefault();
				if (poscheckid != null && poscheckid != "")
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, You have already registered"
					};


				}


				var inserPosUser = $"set dateformat dmy insert into PosUsers(IDNo,Name,AgencyCode,PhoneNo,Active,FingerPrint1,PosSerialNo,Admin,CreatedBy,Teller)values('{ agencymember.idno }','{agencymember.names}','{posadmin2idno4}','{agencymember.phone}','{true}','{ agencymember.Fingerprint}','{ agencymember.MachineID}','{Isadmin}','{ posadmin2idno5}','{true}')";
				db.Database.ExecuteSqlCommand(inserPosUser);

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

		[Route("passwordLogin")]
		public ReturnData PasswordLogin([FromBody] LoadData loadData)
		{
			try
			{
				var posUser = $"Select IDNo from PosUsers  where PosSerialNo='{loadData.MachineId}' and Active=1";
				var posUsers = db.Database.SqlQuery<string>(posUser).FirstOrDefault();
				if (posUsers != null && posUsers != "")
				{
					bool isRole = true;

					return new ReturnData
					{
						Success = true,
						Message = "Complete",
						Data = isRole
					};
				}
				else
				{
					return new ReturnData
					{
						Success = true,
						Message = "False"
					};
				}
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
		//[Route("TellerStatus")]
		//public ReturnData TellerDate([FromBody] TellerDate tellerDate)
		//{
		//	try
		//	{


		//		//select the Admin status  from database

		//		var idposuser1 = $"Select Teller from PosUsers  where IDNo='{tellerDate.idno}'and PosSerialNo='{tellerDate.MachineId}'and Active=1";
		//		bool posuser1 = db.Database.SqlQuery<bool>(idposuser1).FirstOrDefault();

		//		var idposuser11 = $"Select AgencyCode from PosAgents  where PosSerialNo='{tellerDate.MachineId}' and Active=1";
		//		string posuser12 = db.Database.SqlQuery<string>(idposuser11).FirstOrDefault();
		//		if (String.IsNullOrEmpty(posuser12))
		//		{
		//			return new ReturnData
		//			{
		//				Success = true,
		//				Message = "You are not allowed to use this device",
		//			};

		//		}

		//		if (posuser1.Equals(true))
		//		{
		//			return new ReturnData
		//			{
		//				Success = true,
		//				Message = $"{posuser1}",
		//			};
		//		}
		//		else
		//		{
		//			return new ReturnData
		//			{
		//				Success = true,
		//				Message = $"{posuser1}",
		//			};

		//		}


		//	}
		//	catch (Exception ex)
		//	{
		//		return new ReturnData
		//		{
		//			Success = false,
		//			Message = "Sorry network error,try again"
		//		};
		//	}
		//}
		[Route("adminLogin")]
		public ReturnData LoginModel([FromBody] LoginModel logindata)
		{
			try
			{


				//select the Admin status  from database
				var idposuser = $"Select IDNo from PosUsers  where IDNo='{logindata.IdNo}'and PosSerialNo='{logindata.MachineId}'and Active=1";
				var posuser = db.Database.SqlQuery<string>(idposuser).FirstOrDefault();
				if (posuser != null && posuser != "")
				{
					var idposuser1 = $"Select Admin from PosUsers  where IDNo='{logindata.IdNo}'and PosSerialNo='{logindata.MachineId}'and Active=1";
					bool posuser1 = db.Database.SqlQuery<bool>(idposuser1).FirstOrDefault();

					var idposuser11 = $"Select AgencyCode from PosAgents  where PosSerialNo='{logindata.MachineId}' and Active=1";
					string posuser12 = db.Database.SqlQuery<string>(idposuser11).FirstOrDefault();
					if (String.IsNullOrEmpty(posuser12))
					{
						return new ReturnData
						{
							Success = true,
							Message = "You are not allowed to use this device",
						};

					}

					if (posuser1.Equals(true))
					{
						//insert PosLogins
						var inserPosLogins = $"set dateformat dmy insert into PosLogins(IDNo,AgencyCode,PosSerialNo)values('{ logindata.IdNo }','{posuser12 }','{logindata.MachineId}')";
						db.Database.ExecuteSqlCommand(inserPosLogins);
						return new ReturnData
						{
							Success = true,
							Message = $"{posuser1}",
						};
					}
					else
					{
						//insert PosLogins
						var inserPosLogins = $"set dateformat dmy insert into PosLogins(IDNo,AgencyCode,PosSerialNo)values('{ logindata.IdNo }','{posuser12 }','{logindata.MachineId}')";
						db.Database.ExecuteSqlCommand(inserPosLogins);
						return new ReturnData
						{
							Success = true,
							Message = $"{posuser1}",
						};

					}


				}
				else
				{

					return new ReturnData
					{
						Success = true,
						Message = "You are not an Administrator "

					};
				}


			}
			catch (Exception ex)
			{
				return new ReturnData
				{
					Success = false,
					Message = "Sorry network error,try again"
				};
			}
		}

		[Route("login")]
		public ReturnData Login([FromBody] MemberModel admin)
		{
			try
			{
				//var adminUser = db.UserAccounts.FirstOrDefault(a => a.UserLoginID == admin.username);
				var admLogin = $"select UserLoginID from UserAccounts where UserLoginID='{admin.username}' and POSAdmin=1 and Status=1";
				var adminUser = db.Database.SqlQuery<string>(admLogin).FirstOrDefault();

				var admLogin1 = $"select mPassword from UserAccounts where UserLoginID='{admin.username}'and POSAdmin=1 and Status=1";
				var adminUser11 = db.Database.SqlQuery<string>(admLogin1).FirstOrDefault();
				string dbPassword = Convert.ToString(adminUser11);
				string Password = Decryptor.Decript_String(admin.password);

				if (adminUser != null)
				{

					if (Password.Equals(dbPassword))

					{
						bool isRole = true;
						return new ReturnData
						{
							Success = true,
							Message = "Login Successfull",
							Data = isRole
						};
					}
					else
					{
						return new ReturnData
						{
							Success = false,
							Message = "Login failed,wrong  password"
						};
					}

				}
				else
				{
					return new ReturnData
					{
						Success = false,
						Message = "Invalid username and Password"
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



		[Route("existingMembersFingerPrints")]
		public ReturnData existingMembersFingerPrints([FromBody] FingerPrintModel printModel)
		{
			try
			{

				//if (!string.IsNullOrEmpty(printModel.FingerPrint))
				var figuerPrintInfo = printModel.FingerPrint.Split('@');
				printModel.FingerPrint = figuerPrintInfo.Count() < 2 ? figuerPrintInfo[0] : figuerPrintInfo[1];
				var decimalFingerprint = int.Parse(printModel.FingerPrint, System.Globalization.NumberStyles.HexNumber);


				//check existence of fingerprint in the PosMembers
				var idposuser = $"Select IDNo from Members  where IDNo='{printModel.IdNo}'";
				var possuser = db.Database.SqlQuery<string>(idposuser).FirstOrDefault();
				if (String.IsNullOrEmpty(possuser))
				{
					return new ReturnData
					{
						Success = true,
						Message = "Your details could not be found"
					};

				}
				var idposuserid = $"Select IDNo from PosMembers  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}' and Active=1";
				var possuserid = db.Database.SqlQuery<string>(idposuserid).FirstOrDefault();

				if (possuserid != null && possuserid != "")
				{

					var posuserFingerprint = $"Select FingerPrint1 from PosMembers  WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}' and Active=1";
					var checkidposmember = db.Database.SqlQuery<string>(posuserFingerprint).FirstOrDefault();
					if (String.IsNullOrEmpty(checkidposmember).Equals(false))
					{
						var inserPosMember1 = $"UPDATE PosMembers SET FingerPrint2 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}' and Active=1";
						db.Database.ExecuteSqlCommand(inserPosMember1);
						//var idposuseridz = $"Select IDNo from PosUsers  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}' and Active=1";
						//var possuseridz = db.Database.SqlQuery<string>(idposuseridz).FirstOrDefault();
						//if (idposuseridz != null && idposuseridz != "")
						//{
						//	var inserPos = $"UPDATE PosUsers SET FingerPrint2 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}' and Active=1";
						//	db.Database.ExecuteSqlCommand(inserPos);
						//}


						return new ReturnData
						{
							Success = true,
							Message = " Fingerprint1 Updated  sucessfully"
						};

					}
					else
					{
						var inserPosMember1 = $"UPDATE PosMembers SET FingerPrint1 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}' and Active=1";
						db.Database.ExecuteSqlCommand(inserPosMember1);
						//var idposuseridzz = $"Select IDNo from PosUsers  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}' and Active=1";
						//var possuseridzz = db.Database.SqlQuery<string>(idposuseridzz).FirstOrDefault();
						//if (idposuseridzz != null && idposuseridzz != "")
						//{
						//	var inserPos = $"UPDATE PosUsers SET FingerPrint1 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}' and Active=1";
						//	db.Database.ExecuteSqlCommand(inserPos);
						//}
						return new ReturnData
						{
							Success = true,
							Message = " Fingerprint Updated  sucessfully"
						};
					}

				}
				else
				{
					var idposuser11 = $"Select AgencyCode from PosAgents  where PosSerialNo='{printModel.MachineId} ' and Active=1";
					string serial = db.Database.SqlQuery<string>(idposuser11).FirstOrDefault(); 
					var searchAuditID = $"Select Name from PosUsers  where PosSerialNo='{printModel.AuditId} ' and Active=1";
					string AuditID = db.Database.SqlQuery<string>(searchAuditID).FirstOrDefault();

					 var inserPosMember = $"set dateformat dmy insert into PosMembers(IDNo,AuditID,RegistrationDate,FingerPrint1,PosSerialNo,AgencyCode,Active,FingerPrint2)values('{ printModel.IdNo }','{AuditID}','{DateTime.Now}','{decimalFingerprint}','{printModel.MachineId}','{serial}','{true}','{""}')";
					db.Database.ExecuteSqlCommand(inserPosMember);
				}



				return new ReturnData
				{
					Success = true,
					Message = "Fingerprints captured sucessfully"
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
		[Route("newMembersFingerPrints")]
		public ReturnData newMembersFingerPrints([FromBody] FingerPrintModel printModel)
		{
			try
			{

				//if (!string.IsNullOrEmpty(printModel.FingerPrint))
				var figuerPrintInfo = printModel.FingerPrint.Split('@');
				printModel.FingerPrint = figuerPrintInfo.Count() < 2 ? figuerPrintInfo[0] : figuerPrintInfo[1];
				var decimalFingerprint = int.Parse(printModel.FingerPrint, System.Globalization.NumberStyles.HexNumber);

				//AgentMembers fingerprint update
				var agentprint = $"Select IDNo from AgentMembers  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId} ' and Registered=0";
				var AgentPrints1 = db.Database.SqlQuery<string>(agentprint).FirstOrDefault();

				if (AgentPrints1 != null && AgentPrints1 != "")
				{
					var idposuserid = $"Select FingerPrint1 from AgentMembers  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}' and Registered=0";
					var possuserid = db.Database.SqlQuery<string>(idposuserid).FirstOrDefault();
					if (possuserid != null && possuserid != "")
					{
						var posuserFingerprint = $"UPDATE AgentMembers SET FingerPrint2 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}' and Registered=0";
						db.Database.ExecuteSqlCommand(posuserFingerprint);
						return new ReturnData
						{
							Success = true,
							Message = " Fingerprint2 Updated  sucessfully"
						};


					}
					else
					{
						var posuserFingerprint1 = $"UPDATE AgentMembers SET FingerPrint1 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId} ' and Registered=0";
						db.Database.ExecuteSqlCommand(posuserFingerprint1);
						return new ReturnData
						{
							Success = true,
							Message = "Fingerprint1 Updated  sucessfully"
						};
					}

				}
				return new ReturnData
				{
					Success = true,
					Message = "Fingerprints captured sucessfully"
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
		[Route("OperatorsFingerPrints")]
		public ReturnData OperatorsFingerPrints([FromBody] FingerPrintModel printModel)
		{
			try
			{

				//if (!string.IsNullOrEmpty(printModel.FingerPrint))
				var figuerPrintInfo = printModel.FingerPrint.Split('@');
				printModel.FingerPrint = figuerPrintInfo.Count() < 2 ? figuerPrintInfo[0] : figuerPrintInfo[1];
				var decimalFingerprint = int.Parse(printModel.FingerPrint, System.Globalization.NumberStyles.HexNumber);

				var posUser = $"Select IDNO from useraccounts  where IDNo='{printModel.IdNo}' and PosAdmin='1' and Status=1";
				var posUsers = db.Database.SqlQuery<string>(posUser).FirstOrDefault();
				var idposusery19 = $"Select UserName from UserAccounts  where IDNo='{printModel.IdNo}' and POSAdmin=1 and Status=1";
				string regist23 = db.Database.SqlQuery<string>(idposusery19).FirstOrDefault();

				var idposuser110 = $"Select AgencyCode from PosAgents  where PosSerialNo='{printModel.MachineId}'and Active=1 ";
				string serial43 = db.Database.SqlQuery<string>(idposuser110).FirstOrDefault();
				if (posUsers != null && posUsers != "")
				{

					var news = db.PosUsers.FirstOrDefault(a => a.IDNo == printModel.IdNo);
					if (news != null)
					{
						var posSmin = $"Select Fingerprint1 from PosUsers  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}' and Active=1";
						var posSdminFingerprint = db.Database.SqlQuery<string>(posSmin).FirstOrDefault();
						if (posSdminFingerprint != null && posSdminFingerprint != "")
						{
							var posSminupdate = $"Update PosUsers set Fingerprint2='{decimalFingerprint}'  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}' and Active=1";
							db.Database.ExecuteSqlCommand(posSminupdate);
							var idposuser = $"Select IDNo from Members  where IDNo='{printModel.IdNo}'";
							var possuser = db.Database.SqlQuery<string>(idposuser).FirstOrDefault();
							if (possuser != null && possuser != "")
							{
								var idposuser3 = $"Select IDNo from PosMembers  where IDNo='{printModel.IdNo}' PosSerialNo='{printModel.MachineId} ' and Active=1 ";
								var possuser3 = db.Database.SqlQuery<string>(idposuser3).FirstOrDefault();
								if (string.IsNullOrEmpty(possuser3))
								{
									var inserPosUsero1 = $" set dateformat dmy insert into PosMembers(IDNo,AuditID,RegistrationDate,FingerPrint1,PosSerialNo,AgencyCode,Active,FingerPrint2)values('{ printModel.IdNo }','{regist23}','{DateTime.UtcNow.Date}','{""}','{printModel.MachineId}','{serial43}','{true}','{decimalFingerprint}')";
									db.Database.ExecuteSqlCommand(inserPosUsero1);
								}
								else
								{
									var MemberFingerprint11 = $"UPDATE PosMembers SET FingerPrint2 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId} ' and Active=1";
									db.Database.ExecuteSqlCommand(MemberFingerprint11);
								}
							}

							return new ReturnData
							{
								Success = true,
								Message = "Fingerprint2 updated Successfully"
							};
						}
						else
						{
							var posSSminupdate = $"Update PosUsers set Fingerprint1='{decimalFingerprint}'  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}' and Active=1";
							db.Database.ExecuteSqlCommand(posSSminupdate);
							var idposuser = $"Select IDNo from Members  where IDNo='{printModel.IdNo}'";
							var possuser = db.Database.SqlQuery<string>(idposuser).FirstOrDefault();
							if (possuser != null && possuser != "")
							{
								var idposuser3 = $"Select IDNo from PosMembers  where IDNo='{printModel.IdNo}' PosSerialNo='{printModel.MachineId} ' and Active=1 ";
								var possuser3 = db.Database.SqlQuery<string>(idposuser3).FirstOrDefault();
								if (string.IsNullOrEmpty(possuser3))
								{
									var inserPosUsero1 = $" set dateformat dmy insert into PosMembers(IDNo,AuditID,RegistrationDate,FingerPrint1,PosSerialNo,AgencyCode,Active,FingerPrint2)values('{ printModel.IdNo }','{regist23}','{DateTime.UtcNow.Date}','{decimalFingerprint}','{printModel.MachineId}','{serial43}','{true}','{""}')";
									db.Database.ExecuteSqlCommand(inserPosUsero1);
								}
								else
								{
									var MemberFingerprint11 = $"UPDATE PosMembers SET FingerPrint1 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId} ' and Active=1";
									db.Database.ExecuteSqlCommand(MemberFingerprint11);
								}
							}
							return new ReturnData
							{
								Success = true,
								Message = "Fingerprint1 updated Successfully"
							};
						}

					}
					else
					{
						var idposuse = $"Select UserLoginID from UserAccounts  where IDNo='{printModel.IdNo}'  and POSAdmin=1 and Status=1 ";
						string regists = db.Database.SqlQuery<string>(idposuse).FirstOrDefault();
						//var regist = db.UserAccounts.FirstOrDefault(a => a.IDNO == printModel.IdNo);
						var idposusery1 = $"Select UserName from UserAccounts  where IDNo='{printModel.IdNo}' and POSAdmin=1 and Status=1";
						string regist = db.Database.SqlQuery<string>(idposusery1).FirstOrDefault();
						//var phone = db.MEMBERS.FirstOrDefault(a => a.IDNo == printModel.IdNo);
						var idposusery = $"Select MobileNO from MEMBERS  where IDNo='{printModel.IdNo}' ";
						string phone = db.Database.SqlQuery<string>(idposusery).FirstOrDefault();
						//var serial = db.PosAgents.FirstOrDefault(a => a.PosSerialNo == printModel.MachineId);
						var idposuser11 = $"Select AgencyCode from PosAgents  where PosSerialNo='{printModel.MachineId}'and Active=1 ";
						string serial = db.Database.SqlQuery<string>(idposuser11).FirstOrDefault();


						var inserPosUser1 = $" set dateformat dmy insert into PosUsers(IDNo,Name,FingerPrint1,PosSerialNo,AgencyCode,PhoneNo,Admin,Active,CreatedBy,FingerPrint2,CreatedOn,Teller)values('{ printModel.IdNo }','{regist}','{decimalFingerprint}','{printModel.MachineId}','{serial}','{phone}','{true}','{true}','{regists}','{""}','{DateTime.UtcNow.Date}','{false}')";
						db.Database.ExecuteSqlCommand(inserPosUser1);

						var idposuser = $"Select IDNo from Members  where IDNo='{printModel.IdNo}'";
						var possuser = db.Database.SqlQuery<string>(idposuser).FirstOrDefault();
						if (possuser != null && possuser != "")
						{
							var inserPosUsero1 = $" set dateformat dmy insert into PosMembers(IDNo,AuditID,RegistrationDate,FingerPrint1,PosSerialNo,AgencyCode,Active,FingerPrint2)values('{ printModel.IdNo }','{regist}','{DateTime.UtcNow.Date}','{decimalFingerprint}','{printModel.MachineId}','{serial}','{true}','{""}')";
							db.Database.ExecuteSqlCommand(inserPosUsero1);
						}


						return new ReturnData
						{
							Success = true,
							Message = "Registration completed sucessfully"
						};
					}
				}
				//check existence of fingerprint in the PosUsers
				var idposuser1 = $"Select IDNo from PosUsers  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId} ' and Active=1";
				var possuser2 = db.Database.SqlQuery<string>(idposuser1).FirstOrDefault();

				if (possuser2 != null && possuser2 != "")
				{
					var idposuserid1 = $"Select FingerPrint1 from PosUsers  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId}' and Active=1";
					var possuserid2 = db.Database.SqlQuery<string>(idposuserid1).FirstOrDefault();
					if (possuserid2 != null && possuserid2 != "")
					{
						var posuserFingerprint1 = $"UPDATE PosUsers SET FingerPrint2 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}' and Active=1";
						db.Database.ExecuteSqlCommand(posuserFingerprint1);
						var idposuser = $"Select IDNo from Members  where IDNo='{printModel.IdNo}'";
						var possuser = db.Database.SqlQuery<string>(idposuser).FirstOrDefault();
						if (possuser != null && possuser != "")
						{
							var idposuser3 = $"Select IDNo from PosMembers  where IDNo='{printModel.IdNo}' PosSerialNo='{printModel.MachineId} ' and Active=1 ";
							var possuser3 = db.Database.SqlQuery<string>(idposuser3).FirstOrDefault();
							if (string.IsNullOrEmpty(possuser3))
							{
								var inserPosUsero1 = $" set dateformat dmy insert into PosMembers(IDNo,AuditID,RegistrationDate,FingerPrint1,PosSerialNo,AgencyCode,Active,FingerPrint2)values('{ printModel.IdNo }','{regist23}','{DateTime.UtcNow.Date}','{""}','{printModel.MachineId}','{serial43}','{true}','{decimalFingerprint}')";
								db.Database.ExecuteSqlCommand(inserPosUsero1);
							}
							else
							{
								var MemberFingerprint11 = $"UPDATE PosMembers SET FingerPrint2 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId} ' and Active=1";
								db.Database.ExecuteSqlCommand(MemberFingerprint11);
							}
						}
						return new ReturnData
						{
							Success = true,
							Message = " Fingerprint2 Updated  sucessfully"
						};


					}
					else
					{
						var posuserFingerprint11 = $"UPDATE PosUsers SET FingerPrint1 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId} ' and Active=1";
						db.Database.ExecuteSqlCommand(posuserFingerprint11);

                        var idposuser = $"Select IDNo from Members  where IDNo='{printModel.IdNo}'";
                        var possuser = db.Database.SqlQuery<string>(idposuser).FirstOrDefault();
                        if (possuser != null && possuser != "")
                        {
							var idposuser3 = $"Select IDNo from PosMembers  where IDNo='{printModel.IdNo}' PosSerialNo='{printModel.MachineId} ' and Active=1 ";
							var possuser3 = db.Database.SqlQuery<string>(idposuser3).FirstOrDefault();
							if (string.IsNullOrEmpty(possuser3))
							{
								var inserPosUsero1 = $" set dateformat dmy insert into PosMembers(IDNo,AuditID,RegistrationDate,FingerPrint1,PosSerialNo,AgencyCode,Active,FingerPrint2)values('{ printModel.IdNo }','{regist23}','{DateTime.UtcNow.Date}','{decimalFingerprint}','{printModel.MachineId}','{serial43}','{true}','{""}')";
								db.Database.ExecuteSqlCommand(inserPosUsero1);
							}
							else 
							{
								var MemberFingerprint11 = $"UPDATE PosMembers SET FingerPrint1 = '{decimalFingerprint}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId} ' and Active=1";
								db.Database.ExecuteSqlCommand(MemberFingerprint11);
							}
                        }
                        return new ReturnData
						{
							Success = true,
							Message = "Fingerprint1 Updated  sucessfully"
						};
					}

				}
				else
				{

					return new ReturnData
					{
						Success = false,
						Message = "Kindly  register to continue"
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
				//var accounts = db.MEMBERS.Where(m => m.IDNo == printModel.IdNo).Select(m => m.AccNo).ToList();
				var getAcc = $"Select AccNo from CUB where IDNo='{printModel.IdNo}'and Frozen=0";
				return new ReturnData
				{
					Success = true,
					Data = db.Database.SqlQuery<string>(getAcc).ToList()
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

