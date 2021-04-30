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
		KONOIN_BOSAEntities db;
		public UsersController()
		{
			db = new KONOIN_BOSAEntities();
		}

		[Route("registerAgentMember")]
		public ReturnData RegisterAgentMember([FromBody] AgentNewMembers agent)
		{
			try
			{
				if (string.IsNullOrEmpty(agent.idno) || string.IsNullOrEmpty(agent.MachineId) || string.IsNullOrEmpty(agent.Surname) || string.IsNullOrEmpty(agent.other_Names) || string.IsNullOrEmpty(agent.mobile_number) || string.IsNullOrEmpty(agent.Gender))
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, kindly provide all member data"
					};
				}

				var agentMember65 = $"select IDNo from Members where IDNo='{agent.idno}'";
				var posadmin2idno55 = db.Database.SqlQuery<string>(agentMember65).FirstOrDefault();
				if (posadmin2idno55 != null)
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, Member already exist"
					};
				}
				else
				{
					var agentPosUser = $"select SurName from PosUsers where IDNo='{agent.Agentid}' AND  PosSerialNo='{agent.MachineId}' AND Active=1";
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
						var agentPosUser30 = $"select SurName+' '+OtherNames from PosUsers where IDNo='{agent.Agentid}'  and Active=1";
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
				if (string.IsNullOrEmpty(agencymember.idno) || string.IsNullOrEmpty(agencymember.MachineID) || string.IsNullOrEmpty(agencymember.lastname) || string.IsNullOrEmpty(agencymember.names) || string.IsNullOrEmpty(agencymember.phone))
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
				var posadmin4 = $"Select SurName+' '+OtherNames From PosUsers Where IDNo='{agencymember.agentid}'AND  PosSerialNo='{agencymember.MachineID}' and Active=1";
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
						Message = "You have already registered"
					};


				}

				else
				{
					var inserPosUser23 = $"set dateformat dmy insert into PosUsers(IDNo,SurName,OtherNames,AgencyCode,PhoneNo,Active,FingerPrint1,FingerPrint2,PosSerialNo,Admin,CreatedBy,Teller)values('{ agencymember.idno }','{agencymember.names}','{agencymember.lastname}','{posadmin2idno4}','{agencymember.phone}','{true}','{""}','{""}','{ agencymember.MachineID}','{Isadmin}','{ posadmin2idno5}','{true}')";
					db.Database.ExecuteSqlCommand(inserPosUser23);
				}
                var posadminMbr = $"Select IDNo From Members Where IDNo='{agencymember.idno}'";
                var poscheckidMbr = db.Database.SqlQuery<string>(posadminMbr).FirstOrDefault();
				if (string.IsNullOrEmpty(poscheckidMbr).Equals(false))
				{
					var posadminad = $"Select IDNo From PosMembers Where IDNo='{agencymember.idno}' AND  PosSerialNo='{agencymember.MachineID}' and Active=1";
					var poscheckids = db.Database.SqlQuery<string>(posadminad).FirstOrDefault();
					if (string.IsNullOrEmpty(poscheckids).Equals(true))
					{
						var inserPosUser = $"set dateformat dmy insert into PosMembers(IDNo,AgencyCode,Active,FingerPrint1,FingerPrint2,PosSerialNo,AuditID)values('{ agencymember.idno }','{posadmin2idno4}','{true}','{""}','{""}','{ agencymember.MachineID}','{ posadmin2idno5}')";
						db.Database.ExecuteSqlCommand(inserPosUser);
					}
					

				}
				




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
					Message = "Your registration was Unsuccessful"
				};
			}
		}

		[Route("passwordLogin")]
		public ReturnData PasswordLogin([FromBody] LoadData loadData)
		{
			try
			{
				var posAgent = $"Select AgencyCode from PosAgents  where PosSerialNo='{loadData.MachineId}' and Active=1";
				var posAgents = db.Database.SqlQuery<string>(posAgent).FirstOrDefault();
				if (string.IsNullOrEmpty(posAgents))
				{
					return new ReturnData
					{
						Success = false,
						Message = "Device details not Found in the System"
					};
				}
				else
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
        [Route("TellerStatus")]
        public ReturnData TellerDate([FromBody] TellerDate tellerDate)
        {
            try
            {


                //select the Admin status  from database

                var idposuser1 = $"Select Teller from PosUsers  where IDNo='{tellerDate.idno}'and PosSerialNo='{tellerDate.MachineId}'and Active=1";
                bool posuser1 = db.Database.SqlQuery<bool>(idposuser1).FirstOrDefault();

                var idposuser11 = $"Select AgencyCode from PosAgents  where PosSerialNo='{tellerDate.MachineId}' and Active=1";
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
                        Message = $"{posuser1}",
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

				var admidLogin = $"select IDNO from UserAccounts where UserLoginID='{admin.username}' and POSAdmin=1 and Status=1";
				var adminidUser = db.Database.SqlQuery<string>(admidLogin).FirstOrDefault();

				var admLogin1 = $"select mPassword from UserAccounts where UserLoginID='{admin.username}'and POSAdmin=1 and Status=1";
				var adminUser11 = db.Database.SqlQuery<string>(admLogin1).FirstOrDefault();
				string dbPassword = Convert.ToString(adminUser11);
				string Password = Decryptor.Decript_String(admin.password);
				if (adminidUser != null && adminidUser != "")
				{

					var idposuser1 = $"Select IDNo from PosUsers  where IDNo='{adminidUser}' and PosSerialNo='{admin.machineId} ' and Active=1";
					var possuser2 = db.Database.SqlQuery<string>(idposuser1).FirstOrDefault();
					if (String.IsNullOrEmpty(possuser2))
					{
						var idposuse = $"Select Surname+' ' +OtherNames from MEMBERS  where IDNo='{adminidUser}' ";
						string regists = db.Database.SqlQuery<string>(idposuse).FirstOrDefault();
						//var regist = db.UserAccounts.FirstOrDefault(a => a.IDNO == printModel.IdNo);
						var idposusery1 = $"Select OtherNames from MEMBERS  where IDNo='{adminidUser}'";
						string regist = db.Database.SqlQuery<string>(idposusery1).FirstOrDefault();
						//var phone = db.MEMBERS.FirstOrDefault(a => a.IDNo == printModel.IdNo);
						var idposusery = $"Select MobileNO from MEMBERS  where IDNo='{adminidUser}' ";
						string phone = db.Database.SqlQuery<string>(idposusery).FirstOrDefault();

						var idposusery78 = $"Select Surname from MEMBERS  where IDNo='{adminidUser}' ";
						string regist20 = db.Database.SqlQuery<string>(idposusery78).FirstOrDefault();

						var idposusery56 = $"Select OtherNames from MEMBERS  where IDNo='{adminidUser}' ";
						string regist30 = db.Database.SqlQuery<string>(idposusery56).FirstOrDefault();
						//var serial = db.PosAgents.FirstOrDefault(a => a.PosSerialNo == printModel.MachineId);
						var idposuser11 = $"Select AgencyCode from PosAgents  where PosSerialNo='{admin.machineId}'and Active=1 ";
						string serial = db.Database.SqlQuery<string>(idposuser11).FirstOrDefault();
						var inserPosUser1 = $" set dateformat dmy insert into PosUsers(IDNo,SurName,OtherNames,FingerPrint1,PosSerialNo,AgencyCode,PhoneNo,Admin,Active,CreatedBy,FingerPrint2,CreatedOn,Teller)values('{ adminidUser}','{regist20}','{regist30}','{""}','{admin.machineId}','{serial}','{phone}','{true}','{true}','{regists}','{""}','{DateTime.Now}','{false}')";
						db.Database.ExecuteSqlCommand(inserPosUser1);

						var posadminMbr = $"Select IDNo From Members Where IDNo='{adminidUser}'";
						var poscheckidMbr = db.Database.SqlQuery<string>(posadminMbr).FirstOrDefault();
						if (string.IsNullOrEmpty(poscheckidMbr).Equals(false))
						{
							var posadminad = $"Select IDNo From PosMembers Where IDNo='{adminidUser}' AND  PosSerialNo='{admin.machineId}' and Active=1";
							var poscheckids = db.Database.SqlQuery<string>(posadminad).FirstOrDefault();
							if (string.IsNullOrEmpty(poscheckids).Equals(true))
							{
								var inserPosUser = $"set dateformat dmy insert into PosMembers(IDNo,AgencyCode,Active,FingerPrint1,FingerPrint2,PosSerialNo,AuditID)values('{ adminidUser }','{serial}','{true}','{""}','{""}','{ admin.machineId}','{regists}')";
								db.Database.ExecuteSqlCommand(inserPosUser);
							}


						}

					}


				}

				if (adminUser != null && adminUser != null)
				{

					if (Password.Equals(dbPassword))

					{
						var LoginSucess = "Login Successfull";
						bool isRole = true;
						return new ReturnData
						{
							Success = true,
							Message = $"{LoginSucess},{adminidUser}",
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

				var idposuse = $"Select Surname+' ' +OtherNames from PosUsers  where IDNo='{printModel.AgentId}' ";
				string regists = db.Database.SqlQuery<string>(idposuse).FirstOrDefault();

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
					
					if (printModel.FingerName.Contains("Right"))
					{
						var insertPosMember1 = $"Insert into PosMembersUpdates (IDNo,OldFingerPrint,NewFingerPrint,OldFingerPrintName,NewFingerPrintName,AuditID,PosSerialNo,AgencyCode)" +
							$"Select IDNo,FingerPrint2,'{decimalFingerprint}','{printModel.FingerName}','{regists}',PosSerialNo,AgencyCode" +
							$" From PosMembers  WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}' and Active=1";
						db.Database.ExecuteSqlCommand(insertPosMember1);

						var inserPosMember1 = $"UPDATE PosMembers SET FingerPrint2 = '{decimalFingerprint}',FingerName2='{printModel.FingerName}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}' and Active=1";
						db.Database.ExecuteSqlCommand(inserPosMember1);

						var idposuser1 = $"Select IDNo from PosUsers  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId} ' and Active=1";
						var possuser2 = db.Database.SqlQuery<string>(idposuser1).FirstOrDefault();

						if (possuser2 != null && possuser2 != "")
						{

							var posuserFingerprint1 = $"UPDATE PosUsers SET FingerPrint2 = '{decimalFingerprint}',FingerName2='{printModel.FingerName}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}' and Active=1";
							db.Database.ExecuteSqlCommand(posuserFingerprint1);
						}


								return new ReturnData
						{
							Success = true,
							Message = " Fingerprint2 Updated  sucessfully"
						};

					}
					else if(printModel.FingerName.Contains("Left"))
					{
						var insertPosMember1 = $"Insert into PosMembersUpdates (IDNo,OldFingerPrint,NewFingerPrint,OldFingerPrintName,NewFingerPrintName,AuditID,PosSerialNo,AgencyCode)" +
							$"Select IDNo,FingerPrint1,'{decimalFingerprint}','{printModel.FingerName}','{regists}',PosSerialNo,AgencyCode" +
							$" From PosMembers  WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}' and Active=1";
						db.Database.ExecuteSqlCommand(insertPosMember1);

						var inserPosMember1 = $"UPDATE PosMembers SET FingerPrint1 = '{decimalFingerprint}',FingerName1='{printModel.FingerName}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}' and Active=1";
						db.Database.ExecuteSqlCommand(inserPosMember1);

						var idposuser1 = $"Select IDNo from PosUsers  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId} ' and Active=1";
						var possuser2 = db.Database.SqlQuery<string>(idposuser1).FirstOrDefault();

						if (possuser2 != null && possuser2 != "")
						{

							var posuserFingerprint1 = $"UPDATE PosUsers SET FingerPrint1 = '{decimalFingerprint}',FingerName1 ='{printModel.FingerName}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}' and Active=1";
							db.Database.ExecuteSqlCommand(posuserFingerprint1);
						}


						return new ReturnData
						{
							Success = true,
							Message = " Fingerprint1 Updated  sucessfully"
						};
					}

				}
				else
				{
					var idposuser11 = $"Select AgencyCode from PosAgents  where PosSerialNo='{printModel.MachineId} ' and Active=1";
					string serial = db.Database.SqlQuery<string>(idposuser11).FirstOrDefault();
					var searchAuditID = $"Select SurName from PosUsers  where PosSerialNo='{regists} ' and Active=1";
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
					
					if (printModel.FingerName.Contains("Right"))
					{
						var posuserFingerprint = $"UPDATE AgentMembers SET FingerPrint2 = '{decimalFingerprint}',FingerName2='{printModel.FingerName}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}' and Registered=0";
						db.Database.ExecuteSqlCommand(posuserFingerprint);
						return new ReturnData
						{
							Success = true,
							Message = " Fingerprint2 Updated  sucessfully"
						};


					}
					else if (printModel.FingerName.Contains("Left"))
					{
						var posuserFingerprint1 = $"UPDATE AgentMembers SET FingerPrint1 = '{decimalFingerprint}',FingerName1='{printModel.FingerName}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId} ' and Registered=0";
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

				
				var idposuse = $"Select Surname+' ' +OtherNames from PosUsers  where IDNo='{printModel.AgentId}' ";
				string regists = db.Database.SqlQuery<string>(idposuse).FirstOrDefault();

				var idposuser1 = $"Select IDNo from PosUsers  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId} ' and Active=1";
				var possuser2 = db.Database.SqlQuery<string>(idposuser1).FirstOrDefault();

				if (possuser2 != null && possuser2 != "")
				{
					
					if (printModel.FingerName.Contains("Right"))
					{
						var insertPosMember1 = $"Insert into PosMembersUpdates (IDNo,OldFingerPrint,NewFingerPrint,OldFingerPrintName,NewFingerPrintName,AuditID,PosSerialNo,AgencyCode)" +
							$"Select IDNo,FingerPrint2,'{decimalFingerprint}',FingerName2,'{printModel.FingerName}','{regists}',PosSerialNo,AgencyCode" +
							$" From PosUsers  WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}' and Active=1 and FingerPrint2 <>''";
						db.Database.ExecuteSqlCommand(insertPosMember1);

						var posuserFingerprint1 = $"UPDATE PosUsers SET FingerPrint2 = '{decimalFingerprint}',FingerName2='{printModel.FingerName}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}' and Active=1";
						db.Database.ExecuteSqlCommand(posuserFingerprint1);
						
						var idposuser3 = $"Select IDNo from PosMembers  where IDNo='{printModel.IdNo}' and  PosSerialNo='{printModel.MachineId} ' and Active=1 ";
						var possuser3 = db.Database.SqlQuery<string>(idposuser3).FirstOrDefault();
						if (string.IsNullOrEmpty(possuser3).Equals(false))
						{
					
							var MemberFingerprint11 = $"UPDATE PosMembers SET FingerPrint2 = '{decimalFingerprint}',FingerName2='{printModel.FingerName}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId} ' and Active=1";
							db.Database.ExecuteSqlCommand(MemberFingerprint11);
						}
						return new ReturnData
						{
							Success = true,
							Message = " Fingerprint2 Updated  sucessfully"
						};
					}
					else if (printModel.FingerName.Contains("Left"))
					{
						var insertPosMember1 = $"Insert into PosMembersUpdates (IDNo,OldFingerPrint,NewFingerPrint,OldFingerPrintName,NewFingerPrintName,AuditID,PosSerialNo,AgencyCode)" +
							$"Select IDNo,FingerPrint1,'{decimalFingerprint}',FingerName1,'{printModel.FingerName}','{regists}',PosSerialNo,AgencyCode" +
							$" From PosUsers  WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId}' and Active=1 and FingerPrint1 <>''";
						db.Database.ExecuteSqlCommand(insertPosMember1);

						var posuserFingerprint11 = $"UPDATE PosUsers SET FingerPrint1 = '{decimalFingerprint}',FingerName1='{printModel.FingerName}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId} ' and Active=1";
						db.Database.ExecuteSqlCommand(posuserFingerprint11);


						var idposuser3 = $"Select IDNo from PosMembers  where IDNo='{printModel.IdNo}' and PosSerialNo='{printModel.MachineId} ' and Active=1 ";
						var possuser3 = db.Database.SqlQuery<string>(idposuser3).FirstOrDefault();
						if (string.IsNullOrEmpty(possuser3).Equals(false))
						{

							var MemberFingerprint11 = $"UPDATE PosMembers SET FingerPrint1 = '{decimalFingerprint}',FingerName1='{printModel.FingerName}' WHERE IDNo = '{printModel.IdNo}'and PosSerialNo='{printModel.MachineId} ' and Active=1";
							db.Database.ExecuteSqlCommand(MemberFingerprint11);
						}
						

					}
					return new ReturnData
					{
						Success = true,
						Message = "Fingerprint1 Updated  sucessfully"
					};


				}
				else
				{
					return new ReturnData
					{
						Success = true,
						Message = "You are not Registered as an Operator"
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
		[Route("existingFirstName")]
		public ReturnData existingFirstName([FromBody] FirstNameModel firstNameModel)
		{
			try
			{
				var posUser = $"Select Surname from PosUsers where IDNo ='{firstNameModel.idno}'";
				var posUsers = db.Database.SqlQuery<string>(posUser).FirstOrDefault();

				var posUser1 = $"Select OtherNames from PosUsers where IDNo ='{firstNameModel.idno}'";
				var posUsers1 = db.Database.SqlQuery<string>(posUser1).FirstOrDefault();

				var posFingerprint1 = $"Select Fingerprint1 from PosUsers where IDNo ='{firstNameModel.idno}' and PosSerialNo='{firstNameModel.MachineID}'";
				var posFinger1 = db.Database.SqlQuery<string>(posFingerprint1).FirstOrDefault();
				var posFingerprint2 = $"Select Fingerprint2 from PosUsers where IDNo ='{firstNameModel.idno}' and PosSerialNo='{firstNameModel.MachineID}'";
				var posFinger2 = db.Database.SqlQuery<string>(posFingerprint2).FirstOrDefault();

				var posFringer1 = $"Select FingerName1+' '+FingerName2 from PosUsers where IDNo ='{firstNameModel.idno}'";
				var posFinger171 = db.Database.SqlQuery<string>(posFringer1).FirstOrDefault();
				var posFinger170 = "";

				if (string.IsNullOrEmpty(posFinger171))
				{
					posFinger170 = "Yet to Capture FingerPrints";

				}
				else
				{
					posFinger170 = posFinger171;
				}

				int counts = 0;

				if (string.IsNullOrEmpty(posFinger1).Equals(false) && (string.IsNullOrEmpty(posFinger2).Equals(true)))
				{
					counts = 1;
				}

				if (string.IsNullOrEmpty(posFinger2).Equals(false) && (string.IsNullOrEmpty(posFinger1).Equals(true)))
				{
					counts = 1;
				}

				if (string.IsNullOrEmpty(posFinger1).Equals(false) && (string.IsNullOrEmpty(posFinger2).Equals(false)))
				{
					counts = 2;
				}

				if (posUsers != null && posUsers != "")
				{
					return new ReturnData
					{
						Success = true,
						Message = $"{posUsers},{posUsers1},{counts},{posFinger170}",
					};
				}
				else
				{
					return new ReturnData
					{
						Success = true,
						Message = "You are not registered"
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

		[Route("agentMembersSecondName")]
		public ReturnData agentMembersSecondName([FromBody] AgentNameModel agentNameModel)
		{
			try
			{
				var posUser = $"Select Surname from AgentMembers where IDNo ='{agentNameModel.idno}'";
				var posUsers = db.Database.SqlQuery<string>(posUser).FirstOrDefault();

				var posUser1 = $"Select OtherNames from AgentMembers where IDNo ='{agentNameModel.idno}'";
				var posUsers1 = db.Database.SqlQuery<string>(posUser1).FirstOrDefault();

				var posFingerprint1 = $"Select Fingerprint1 from AgentMembers where IDNo ='{agentNameModel.idno}' and PosSerialNo='{agentNameModel.MachineID}'";
				var posFinger1 = db.Database.SqlQuery<string>(posFingerprint1).FirstOrDefault();
				var posFingerprint2 = $"Select Fingerprint2 from AgentMembers where IDNo ='{agentNameModel.idno}' and PosSerialNo='{agentNameModel.MachineID}'";
				var posFinger2 = db.Database.SqlQuery<string>(posFingerprint2).FirstOrDefault();

				var posFringer1 = $"Select FingerName1+' '+FingerName2 from AgentMembers where IDNo ='{agentNameModel.idno}'";
				var posFinger171 = db.Database.SqlQuery<string>(posFringer1).FirstOrDefault();

				var posFinger170 = "";

				if (string.IsNullOrEmpty(posFinger171))
				{
					posFinger170 = "Yet to Capture FingerPrints";

				}
				else
				{
					posFinger170 = posFinger171;
				}

				int counts = 0;

				if (string.IsNullOrEmpty(posFinger1).Equals(false) && (string.IsNullOrEmpty(posFinger2).Equals(true)))
				{
					counts = 1;
				}

				if (string.IsNullOrEmpty(posFinger2).Equals(false) && (string.IsNullOrEmpty(posFinger1).Equals(true)))
				{
					counts = 1;
				}

				if (string.IsNullOrEmpty(posFinger1).Equals(false) && (string.IsNullOrEmpty(posFinger2).Equals(false)))
				{
					counts = 2;
				}

				if (posUsers != null && posUsers != "")
				{
					return new ReturnData
					{
						Success = true,
						Message = $"{posUsers},{posUsers1},{counts},{posFinger170}",
					};
				}
				else
				{
					return new ReturnData
					{
						Success = true,
						Message = "You are not registered"
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
		[Route("lastTransaction")]
		public ReturnData lastTransaction([FromBody] TransActionEnquiryModel transActionEnquiryModel)
		{
			try
			{
				var dateTimeNow = DateTime.Now; 
				var dateOnlyString = dateTimeNow.ToShortDateString();
				

				var posUser = $"Select Surname from Members where AccNo ='{transActionEnquiryModel.idno}'";
				var posUsers = db.Database.SqlQuery<string>(posUser).FirstOrDefault();

				var posUser1 = $"Select OtherNames from Members where AccNo ='{transActionEnquiryModel.idno}'";
				var posUsers1 = db.Database.SqlQuery<string>(posUser1).FirstOrDefault();

				var posUser12 = $" set dateformat dmy Select Top 1 Amount from PosReconcilliation where AccNo='{transActionEnquiryModel.idno}' and PosSerialNo='{transActionEnquiryModel.MachineID}' and (convert(varchar(10),InitiationDate,103)='{dateOnlyString}') order by InitiationDate desc";
				decimal posUsers12 = db.Database.SqlQuery<decimal>(posUser12).FirstOrDefault();

				var posUser13 = $"set dateformat dmy Select Top 1 VoucherNo from PosReconcilliation where AccNo='{transActionEnquiryModel.idno}' and PosSerialNo='{transActionEnquiryModel.MachineID}' and (convert(varchar(10),InitiationDate,103)='{dateOnlyString}') order by InitiationDate desc";
				var posUsers13 = db.Database.SqlQuery<string>(posUser13).FirstOrDefault();

				var posUser136 = $"set dateformat dmy Select Top 1 Activity from PosReconcilliation where AccNo='{transActionEnquiryModel.idno}' and PosSerialNo='{transActionEnquiryModel.MachineID}' and (convert(varchar(10),InitiationDate,103)='{dateOnlyString}') order by InitiationDate desc";
				var posUsers136 = db.Database.SqlQuery<string>(posUser136).FirstOrDefault();

				var posUser14 = $"set dateformat dmy Select AgencyName from PosAgents where PosSerialNo='{transActionEnquiryModel.MachineID}'";
				var posUsers14 = db.Database.SqlQuery<string>(posUser14).FirstOrDefault();

				var posUser15 = $"set dateformat dmy Select Top 1 InitiationDate from PosReconcilliation where AccNo='{transActionEnquiryModel.idno}' and PosSerialNo='{transActionEnquiryModel.MachineID}' and  (convert(varchar(10),InitiationDate,103)='{dateOnlyString}') order by InitiationDate desc";
				DateTime posUsers15 = db.Database.SqlQuery<DateTime>(posUser15).FirstOrDefault();

				var posUser16 = $"set dateformat dmy Select Top 1 Name from PosReconcilliation where AccNo='{transActionEnquiryModel.idno}' and PosSerialNo='{transActionEnquiryModel.MachineID}'and (convert(varchar(10),InitiationDate,103)='{dateOnlyString}')  order by InitiationDate desc";
				var posUsers16 = db.Database.SqlQuery<string>(posUser16).FirstOrDefault();

				var posUser17 = $"set dateformat dmy Select Top 1 RefNo from PosReconcilliation where AccNo='{transActionEnquiryModel.idno}' and PosSerialNo='{transActionEnquiryModel.MachineID}' and (convert(varchar(10),InitiationDate,103)='{dateOnlyString}') order by InitiationDate desc";
				var posUsers17 = db.Database.SqlQuery<string>(posUser17).FirstOrDefault();

				if (posUsers13 != null && posUsers13 != "")
				{
					return new ReturnData
					{
						Success = true,
						Message = $"{posUsers} {posUsers1},{transActionEnquiryModel.idno},{posUsers12},{posUsers13},{posUsers14},{posUsers15},{posUsers16},{posUsers17},{posUsers136}",
					};
				}
				else
				{
					return new ReturnData
					{
						Success = true,
						Message = "No data found"
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
		[Route("existingSecondName")]
		public ReturnData existingSecondName([FromBody] SecondNameModel secondNameModel)
		{
			try
			{
				
				var posUser = $"Select OtherNames from Members where IDNo ='{secondNameModel.idno}'";
				var posUsers = db.Database.SqlQuery<string>(posUser).FirstOrDefault();

				var posUser1 = $"Select Surname from Members where IDNo ='{secondNameModel.idno}'";
				var posUsers1 = db.Database.SqlQuery<string>(posUser1).FirstOrDefault();

				var posFringer1 = $"Select FingerName1+' '+FingerName2 from PosMembers where IDNo ='{secondNameModel.idno}'";
				var posFinger171 = db.Database.SqlQuery<string>(posFringer1).FirstOrDefault();
				var posFinger170 = "";

				if (string.IsNullOrEmpty(posFinger171))
				{
					posFinger170 = "Yet to Capture FingerPrints";

				}
				else
				{
					posFinger170 = posFinger171;
				}


				var posFingerprint1 = $"Select Fingerprint1 from PosMembers where IDNo ='{secondNameModel.idno}' and PosSerialNo='{secondNameModel.machineID}'";
				var posFinger1 = db.Database.SqlQuery<string>(posFingerprint1).FirstOrDefault();
				var posFingerprint2 = $"Select Fingerprint2 from PosMembers where IDNo ='{secondNameModel.idno}' and PosSerialNo='{secondNameModel.machineID}'";
				var posFinger2 = db.Database.SqlQuery<string>(posFingerprint2).FirstOrDefault();
				int counts = 0;

				if (string.IsNullOrEmpty(posFinger1).Equals(false) && (string.IsNullOrEmpty(posFinger2).Equals(true)))
				{
					counts = 1;
				}

				if (string.IsNullOrEmpty(posFinger2).Equals(false) && (string.IsNullOrEmpty(posFinger1).Equals(true)))
				{
					counts = 1;
				}

				if (string.IsNullOrEmpty(posFinger1).Equals(false) && (string.IsNullOrEmpty(posFinger2).Equals(false)))
				{
					counts = 2;
				}


				if (posUsers != null && posUsers != "")
				{
					return new ReturnData
					{
						Success = true,
						Message = $"{posUsers1},{posUsers},{counts},{posFinger170}",
					};
				}
				else
				{
					return new ReturnData
					{
						Success = true,
						Message = "You are not registered"
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
		[Route("superAdminDetails")]
		public ReturnData superAdminDetails([FromBody] SuperAdminModel secondNameModel)
		{
			try
			{
				var posUser = $"Select OtherNames from Members where IDNo ='{secondNameModel.idno}'";
				var posUsers = db.Database.SqlQuery<string>(posUser).FirstOrDefault();

				var posUser1 = $"Select Surname from Members where IDNo ='{secondNameModel.idno}'";
				var posUsers1 = db.Database.SqlQuery<string>(posUser1).FirstOrDefault();

			

				if (posUsers != null && posUsers != "")
				{
					return new ReturnData
					{
						Success = true,
						Message = $"{posUsers1},{posUsers}",
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


