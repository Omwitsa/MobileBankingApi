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
		TESTEntities3 db;
		public UsersController()
		{
			db = new TESTEntities3();
		}


		[Route("registerAgentMember")]
		public ReturnData RegisterAgentMember([FromBody] AgentNewMembers agent)
		{
			try
			{
				if (string.IsNullOrEmpty(agent.idno) || string.IsNullOrEmpty(agent.MachineId))
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, kindly provide member data"
					};
				}


				var agentPosUser = $"select Name from PosUsers where IDNo='{agent.Agentid}' AND  PosSerialNo='{agent.MachineId}'";
				var agentPosUser1 = db.Database.SqlQuery<string>(agentPosUser).FirstOrDefault();
				//db.Agentmembers.Add(agent);

				//var agentMember = db.AgentMembers.FirstOrDefault(a => a.IDNo == agent.idno);
				var agentMember = $"select IDNo from AgentMembers where IDNo='{agent.idno}' AND  PosSerialNo='{agent.MachineId}'";
				var posadmin2idno5 = db.Database.SqlQuery<string>(agentMember).FirstOrDefault();
				if (posadmin2idno5 != null)
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, Member already exist"
					};
				}

				//var agentMember1 = db.PosUsers.FirstOrDefault(a => a.IDNo == agent.Agentid);





				var insertAgentMember = $"INSERT INTO AgentMembers(SurName,OtherNames,IDNo,MobileNo,Sex,DOB,PosSerialNo,FingerPrint1,FingerPrint2,AuditID,Registered)values('{ agent.Surname}','{agent.other_Names}','{ agent.idno}','{agent.mobile_number}','{agent.Gender}','{agent.DOB}','{ agent.MachineId}','{agent.FingerPrint1}','{ agent.FingerPrint2}','{agentPosUser1}','{false}')";
				db.Database.ExecuteSqlCommand(insertAgentMember);
                //return new ReturnData
                //{
                //	Success = true,
                //	Message = "Member Registered successfully"
                //};
                //db.AgentMembers.Add(new AgentMember
                //{
                //	SurName = agent.Surname,
                //	OtherNames = agent.other_Names,
                //	IDNo = agent.idno,
                //	MobileNo = agent.mobile_number,
                //	Sex = agent.Gender,
                //	DOB = agent.DOB,
                //	PosSerialNo=agent.MachineId,
                //	FingerPrint1 = agent.FingerPrint1,
                //	FingerPrint2 = agent.FingerPrint2,
                //	AuditID = agentMember1.Name,
                //	RegistrationDate= DateTime.UtcNow.Date,
                //	Registred=false

                //});
                //db.SaveChanges();


                //Agent deposit commission
                var pullFunction2 = $"Select Expense_Amount From dbo.Get_POS_Expenses (0,'Registration')";
                decimal AgentRegistrationCommission = db.Database.SqlQuery<decimal>(pullFunction2).FirstOrDefault();
                //get voucher number
                var Vno = $"Select TOP 1 ID From Masters order by ID desc";
                Int64 getvno = db.Database.SqlQuery<Int64>(Vno).FirstOrDefault();
				Int64 nextvno = getvno + 1;

                //var floatAcc = db.PosAgents.FirstOrDefault(m => m.PosSerialNo.ToUpper().Equals(agent.MachineId.ToUpper()));


                var agentPosUser3 = $"select CommissionAccNo from PosAgents where PosSerialNo='{agent.MachineId}'";
                var floatAcc = db.Database.SqlQuery<string>(agentPosUser3).FirstOrDefault();



                //var OperatorName = db.PosUsers.FirstOrDefault(m => m.IDNo.ToUpper().Equals(agent.Agentid.ToUpper()));
                var agentPosUser30 = $"select Name from PosUsers where IDNo='{agent.Agentid}'";
                var OperatorName = db.Database.SqlQuery<string>(agentPosUser30).FirstOrDefault();

				string accno = "205";
				string message = "EasyAgent Deposit Commission";


				var insertGltrans = $"INSERT INTO GLTRANSACTIONS(TransDate,Amount,DocumentNo,TransactionNo,DrAccNo,CrAccNo,TransDescript,AuditTime,AuditID,Source)values('{DateTime.UtcNow.Date}','{AgentRegistrationCommission}','{nextvno}','{nextvno}','{accno}','{floatAcc}','{message}','{DateTime.UtcNow.AddHours(3)}','{OperatorName}','{ agent.idno}')";
				db.Database.ExecuteSqlCommand(insertGltrans);

				//db.GLTRANSACTIONS.Add(new GLTRANSACTION
    //            {
    //                //TransDate = DateTime.UtcNow.Date,
    //                Amount = AgentRegistrationCommission,
    //                DocumentNo = nextvno,
    //                TransactionNo = nextvno,
    //                DrAccNo = accno,
    //                CrAccNo = floatAcc,
    //                TransDescript = message,
    //                AuditTime = DateTime.UtcNow.AddHours(3),
    //                AuditID = OperatorName,
    //                Source = agent.idno
    //            });
            

    //            db.SaveChanges();

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

				//var posAgent = db.PosAgents.FirstOrDefault(a => a.AgencyName == agencymember.agency);

				var posadmin3 = $"Select AgencyCode From PosAgents Where AgencyName='{agencymember.agency}'";
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
				var posadmin4 = $"Select Name From PosUsers Where IDNo='{agencymember.agentid}'AND  PosSerialNo='{agencymember.MachineID}'";
				var posadmin2idno5 = db.Database.SqlQuery<string>(posadmin4).FirstOrDefault();







				bool Isadmin = false;
				if (agencymember.admins == "Administrator")
				{
					Isadmin = true;

                }
				var posadminadd = $"Select IDNo From PosUsers Where IDNo='{agencymember.idno}' AND  PosSerialNo='{agencymember.MachineID}'";
				var poscheckid = db.Database.SqlQuery<string>(posadminadd).FirstOrDefault();
				if (poscheckid != null && poscheckid !="")
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, You have already registered"
					};


				}





                var inserPosUser = $"insert into PosUsers(IDNo,Name,AgencyCode,PhoneNo,Active,FingerPrint1,PosSerialNo,Admin,CreatedBy)values('{ agencymember.idno }','{agencymember.names}','{posadmin2idno4}','{agencymember.phone}','{true}','{ agencymember.Fingerprint}','{ agencymember.MachineID}','{Isadmin}','{ posadmin2idno5}')";
                db.Database.ExecuteSqlCommand(inserPosUser);
                ///var poscheckid1 = db.Database.SqlQuery<string>(inserPosUser).FirstOrDefault();
                //var admin = Isadmin ;


                //db.PosUsers.Add(new PosUser
                //{
                //	IDNo = agencymember.idno,
                //	Name = agencymember.names,
                //	AgencyCode = posAgent.AgencyCode,
                //	PhoneNo = agencymember.phone,
                //	Active = true,
                //	FingerPrint1 = agencymember.Fingerprint,
                //	PosSerialNo = agencymember.MachineID,
                //	Admin = Isadmin,
                //	CreatedBy = agencymember.agentid,
                //	CreatedOn = DateTime.UtcNow.Date,
                //	FingerPrint2 = ""

                //});

                //db.SaveChanges();
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
				var posUser = $"Select IDNo from PosUsers  where PosSerialNo='{loadData.MachineId}'";
				var posUsers = db.Database.SqlQuery<string>(posUser).FirstOrDefault();
				if (posUsers != null && posUsers != "")
				{
					bool isRole = true;
					
					return new ReturnData
					{
						Success = true,
						Message = "Complete",
						Data= isRole
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

        [Route("login")]
        public ReturnData Login([FromBody] MemberModel admin)
        {
            try
            {
				var adminUser = db.UserAccounts.FirstOrDefault(a => a.UserLoginID == admin.username);
				if (adminUser != null)
				{
					string Password = Decryptor.Decript_String(admin.password);
					string dbPassword = adminUser.mPassword;
					if (Password == dbPassword)

					{
						bool isRole = true;
						return new ReturnData
						{
							Success = true,
							Message = "Login Successfull",
							Data=isRole
						};
					}
					else
					{
						return new ReturnData
						{
							Success = false,
							Message = "Login failed,wrong password provided"
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


						var inserPosUser1 = $"insert into PosUsers(IDNo,Name,FingerPrint1,PosSerialNo,AgencyCode,PhoneNo,Admin,Active,CreatedBy,FingerPrint2,CreatedOn)values('{ printModel.IdNo }','{regist.UserName}','{""}','{printModel.MachineId}','{serial.AgencyCode}','{phone.MobileNo}','{true}','{true}','{regist.UserLoginID}','{""}','{DateTime.UtcNow.Date}')";
						db.Database.ExecuteSqlCommand(inserPosUser1);



						//db.PosUsers.Add(new PosUser
						//{
						//	IDNo = printModel.IdNo,
						//	Name = regist.UserName,
						//	FingerPrint1 = "",
						//	PosSerialNo = printModel.MachineId,
						//	AgencyCode = serial.AgencyCode,
						//	PhoneNo = phone.MobileNo,
						//	Admin = true,
						//	Active = true,
						//	CreatedBy = regist.UserLoginID,
						//	FingerPrint2 = "",
						//	CreatedOn = DateTime.UtcNow.Date

						//});

						//db.SaveChanges();
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





			var inserPosMember = $"insert into PosMembers(IDNo,AuditID,RegistrationDate,FingerPrint1,PosSerialNo,AgencyCode,Active,FingerPrint2)values('{ printModel.IdNo }','{""}','{DateTime.UtcNow.Date}','{""}','{printModel.MachineId}','{serial.AgencyCode}','{true}','{""}')";
			db.Database.ExecuteSqlCommand(inserPosMember);

					//db.PosMembers.Add(new PosMember
					//{
					//	IDNo = printModel.IdNo,
					//	AuditID = "",
					//	RegistrationDate= DateTime.UtcNow.Date,
					//	FingerPrint1 = "",
					//	PosSerialNo = printModel.MachineId,
					//	AgencyCode = serial.AgencyCode,
					//	Active = true,
					//	FingerPrint2 = "",
						

					//});

					//db.SaveChanges();
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
