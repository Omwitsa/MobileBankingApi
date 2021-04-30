using MobileBanking_API.Models;
using MobileBanking_API.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;

namespace MobileBanking_API.Controllers
{
	[RoutePrefix("webservice/transacions")]
	public class TransactionController : ApiController
    {
		KONOIN_BOSAEntities db;
		public TransactionController()
		{
			db = new KONOIN_BOSAEntities();
		}

		[Route("deposit")]

		public ReturnData DepositService(Transaction transaction)
		{
			try
			{
				var idd2 = ""; var pay1 = ""; var Acco = ""; decimal avail2 = 0; var mbno1 = ""; var accccc = ""; var Name = "";


				System.Data.SqlClient.SqlCommand cmd;
				string str = "Data Source=KON-DL-SERVER;Initial Catalog=KONOIN_BOSA;Integrated Security=false;user id=K-PILLAR;password=kpillar;Connection Timeout=60000; max pool size=500";
				System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(str);
				con.Open();
				try
				{
					System.Data.SqlClient.SqlCommand comPrice;
					string sqlPrice = null;
					System.Data.SqlClient.SqlDataReader dataReaderPrice;
					sqlPrice = "select Name,AvailableBalance,IDNo,Payno,AccountName,MemberNo,AccNo from CUB where  AccNo = '" + transaction.SNo + "'";


					comPrice = new System.Data.SqlClient.SqlCommand(sqlPrice, con);
					dataReaderPrice = comPrice.ExecuteReader();
					if (dataReaderPrice.HasRows)
					{
						dataReaderPrice.Read();

						Name = Convert.ToString(dataReaderPrice.GetValue(0));
						avail2 = Convert.ToDecimal(dataReaderPrice.GetValue(1));//.ToString());
						idd2 = Convert.ToString(dataReaderPrice.GetValue(2));
						pay1 = Convert.ToString(dataReaderPrice.GetValue(3));
						Acco = Convert.ToString(dataReaderPrice.GetValue(4));
						mbno1 = Convert.ToString(dataReaderPrice.GetValue(5));
						accccc = Convert.ToString(dataReaderPrice.GetValue(6));


					}

					dataReaderPrice.Close(); dataReaderPrice.Dispose(); dataReaderPrice = null;
					con.Close();
				}
				catch (Exception ex)
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, network error occurred,check your internet and try again"
					};
				}



				var memberr = $"select AccNo from CUB where AccNo= '{transaction.SNo}'";
				var member1 = db.Database.SqlQuery<string>(memberr).FirstOrDefault();
				if (member1 == null)
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, account number not found",
						Data = "0"
					};
				}
				avail2 += transaction.Amount;
				var transactionDescription = "EasyAgent Deposit";
				//get voucher number
				var Vno = $"Select ISNULL(max(ID),1) From Masters";
				Int64 getvno = db.Database.SqlQuery<Int64>(Vno).FirstOrDefault();
				Int64 nextvno = getvno + 1;
				//Operator name
				//var OperatorName = db.PosUsers.FirstOrDefault(m => m.IDNo.ToUpper().Equals(transaction.AuditId.ToUpper()));
				//Operator name
				var opr = $"select SurName from PosUsers where IDNo= '{transaction.AuditId}' and Active=1 and PosSerialNo='{transaction.MachineID}'";
				string OperatorName = db.Database.SqlQuery<string>(opr).FirstOrDefault();

				//var vNo = GetVoucherNo(transaction.Amount);
				//var floatAcc = db.PosAgents.FirstOrDefault(m => m.PosSerialNo.ToUpper().Equals(transaction.MachineID.ToUpper()));
				//float account
				var flo = $"select FloatAccNo from PosAgents A Inner Join PosUsers U On U.AgencyCode=A.AgencyCode where U.IDNo='{transaction.AuditId}' and U.Active=1 and U.PosSerialNo ='{transaction.MachineID}'";
				string floatAcc = db.Database.SqlQuery<string>(flo).FirstOrDefault();
				if (floatAcc != "" && floatAcc != null)
				{


					//string floatAcc = "840";

					var flo1 = $"select CommissionAccNo from PosAgents A Inner Join PosUsers U On U.AgencyCode=A.AgencyCode where U.IDNo='{transaction.AuditId}' and U.Active=1 and U.PosSerialNo ='{transaction.MachineID}'";
					string floatAcc2 = db.Database.SqlQuery<string>(flo1).FirstOrDefault();
					//string floatAcc2 = "001-1";

					var flo222 = $"select AgencyCode from PosAgents where PosSerialNo= '{transaction.MachineID}' and Active=1";
					string floatAcc33 = db.Database.SqlQuery<string>(flo222).FirstOrDefault();
					if (String.IsNullOrEmpty(floatAcc33))
					{
						return new ReturnData
						{
							Success = true,
							Message = "You are not allowed to use this device",
							Data = "0"
						};

					}
					var flo2 = $"select AgencyName from PosAgents where PosSerialNo= '{transaction.MachineID}' and Active=1";
					string floatAcc3 = db.Database.SqlQuery<string>(flo2).FirstOrDefault();



					//debit balance for the float account;
					var drBalance = $"Select ISNull((Select SUM(Amount) From GLTransactions Where DrAccNo='{floatAcc}'),0)";
					decimal drbl = db.Database.SqlQuery<decimal>(drBalance).FirstOrDefault();
					//credit balance for the float account;
					var crBalance = $"Select ISNull((Select SUM(Amount) From GLTransactions Where CrAccNo='{floatAcc}'),0)";
					decimal crbl = db.Database.SqlQuery<decimal>(crBalance).FirstOrDefault();
					//decimal transamount = transaction.Amount;




					decimal TellerBalance = drbl - crbl;
					if (TellerBalance < transaction.Amount)
					{
						return new ReturnData
						{
							Success = false,
							Message = "Sorry, Your float balance is insufficient",
							Data = "0"
						};
					}
					else
					{

						if (string.IsNullOrEmpty(transaction.Name) && string.IsNullOrEmpty(transaction.Did))
						{
							var oprrs = $"select SurName from Members where AccNo= '{transaction.SNo}'";
							string OperatorNamemember = db.Database.SqlQuery<string>(oprrs).FirstOrDefault();
							var oprrs1 = $"select OtherNames from Members where AccNo= '{transaction.SNo}'";
							string OperatorNamemember1 = db.Database.SqlQuery<string>(oprrs1).FirstOrDefault();
							var opr1 = $"select IDNo from  Members where AccNo= '{transaction.SNo}'";
							string OperatorNamememberid = db.Database.SqlQuery<string>(opr1).FirstOrDefault();
							var inserttransactions = $"set dateformat dmy INSERT INTO PosReconcilliation(Name,RefNo,InitiationDate,AccNo,VoucherNo,Amount,CompletionDate,PosSerialNo,AgencyCode,InitiatedBy,Activity,Posted,productID)values('{OperatorNamemember}  {OperatorNamemember1}','{OperatorNamememberid}','{DateTime.Now}','{transaction.SNo}','{nextvno}','{transaction.Amount}','{DateTime.Now}','{transaction.MachineID}','{floatAcc33}','{OperatorName}','{transactionDescription}','{false}','{"000"}')";
							db.Database.ExecuteSqlCommand(inserttransactions);

							var DepositSucess = "Deposit Successfull";
							return new ReturnData
							{
								Success = true,
								Message = $"{DepositSucess},{nextvno},{OperatorNamemember}  {OperatorNamemember1},{floatAcc3},{DateTime.Now},{OperatorNamemember}  {OperatorNamemember1},{OperatorNamememberid}",
								Data = "1"
							};
						}
						else
						{
							var oprrs66 = $"select SurName from Members where AccNo= '{transaction.SNo}'";
							string OperatorNamemember77 = db.Database.SqlQuery<string>(oprrs66).FirstOrDefault();
							var oprrs188 = $"select OtherNames from Members where AccNo= '{transaction.SNo}'";
							string OperatorNamemember199 = db.Database.SqlQuery<string>(oprrs188).FirstOrDefault();
							//Store transaction 
							var inserttransactions = $"set dateformat dmy INSERT INTO PosReconcilliation(Name,RefNo,InitiationDate,AccNo,VoucherNo,Amount,CompletionDate,PosSerialNo,AgencyCode,InitiatedBy,Activity,Posted,productID)values('{transaction.Name}','{transaction.Did}','{DateTime.Now}','{transaction.SNo}','{nextvno}','{transaction.Amount}','{DateTime.Now}','{transaction.MachineID}','{floatAcc33}','{OperatorName}','{transactionDescription}','{false}','{"000"}')";
							db.Database.ExecuteSqlCommand(inserttransactions);

							var DepositSucess = "Deposit Successfull";
							return new ReturnData
							{
								Success = true,
								Message = $"{DepositSucess},{nextvno},{OperatorNamemember77}  {OperatorNamemember199},{floatAcc3},{DateTime.Now},{transaction.Name},{transaction.Did}",
								Data = "1"
							};
						}

					



					}


				}
				else
				{
					return new ReturnData
					{
						Success = false,
						Message = "You cannot transact at the moment",
						Data = "0"
					};
				}
				
			}
			catch (Exception ex)
			{
				return new ReturnData
				{
					Success = false,
					Message = "Sorry, network error occurred,check your internet and try again",
					Data = "0"
				};
			}
		}

		private string GetVoucherNo(decimal amount)
		{
			var query = "select TOP 1 customerbalanceid from CustomerBalance order by customerbalanceid desc";
			var bal = db.Database.SqlQuery<Int64>(query).FirstOrDefault();
			var vno = $"{bal}{decimal.ToInt32(amount)}";
			return vno;
		}

		[Route("withdraw")]

		public ReturnData WithdrawalService(Transaction transaction)
		{
			try
			{
				var idd2 = ""; var pay1 = ""; var Acco = ""; decimal avail2 = 0; var mbno1 = ""; var accccc = ""; var Name = "";


				System.Data.SqlClient.SqlCommand cmd;
				string str = "Data Source=KON-DL-SERVER;Initial Catalog=KONOIN_BOSA;Integrated Security=false;user id=K-PILLAR;password=kpillar;Connection Timeout=60000; max pool size=500";
				System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(str);
				con.Open();
				try
				{
					System.Data.SqlClient.SqlCommand comPrice;
					string sqlPrice = null;
					System.Data.SqlClient.SqlDataReader dataReaderPrice;
					sqlPrice = "select Name,AvailableBalance,IDNo,Payno,AccountName,MemberNo,AccNo from CUB where AccNo = '" + transaction.SNo + "'";


					comPrice = new System.Data.SqlClient.SqlCommand(sqlPrice, con);
					dataReaderPrice = comPrice.ExecuteReader();
					if (dataReaderPrice.HasRows)
					{
						dataReaderPrice.Read();

						Name = Convert.ToString(dataReaderPrice.GetValue(0));
						avail2 = Convert.ToDecimal(dataReaderPrice.GetValue(1));//.ToString());
						idd2 = Convert.ToString(dataReaderPrice.GetValue(2));
						pay1 = Convert.ToString(dataReaderPrice.GetValue(3));
						Acco = Convert.ToString(dataReaderPrice.GetValue(4));
						mbno1 = Convert.ToString(dataReaderPrice.GetValue(5));
						accccc = Convert.ToString(dataReaderPrice.GetValue(6));


					}

					dataReaderPrice.Close(); dataReaderPrice.Dispose(); dataReaderPrice = null;
					con.Close();
				}
				catch (Exception ex)
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, network error occurred,check your internet and try again",
						Data = "0"
					};
				}


				var memberr = $"select AccNo from CUB where AccNo= '{transaction.SNo}'";
				var member = db.Database.SqlQuery<string>(memberr).FirstOrDefault();
				if (member == null)
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, account number not found",
						Data = "0"
					};

				}

				//var OperatorName = db.PosUsers.FirstOrDefault(m => m.IDNo.ToUpper().Equals(transaction.AuditId.ToUpper()));
				var debtQuery = $"SELECT ISNULL(Sum(Amount),0) FROM CUSTOMERBALANCE WHERE AccNO = '{transaction.SNo}' AND cash = 1 AND transtype='DR' AND TransDescription != 'Cheque Dep(uncleared)' AND TransDescription != 'Bounced Cheque'";
				var debts = db.Database.SqlQuery<decimal>(debtQuery).FirstOrDefault();
				var creditQuery = $"SELECT ISNULL(Sum(Amount),0) FROM CUSTOMERBALANCE WHERE AccNO = '{transaction.SNo}' AND cash = 1 AND transtype='CR' AND TransDescription != 'Cheque Dep(uncleared)'";
				var credits = db.Database.SqlQuery<decimal>(creditQuery).FirstOrDefault();

				var balance = credits - debts;
				float MMbalance = 0;
				if (Acco == "SAVINGS ACCOUNT")
				{
					MMbalance = 500;


					if (avail2 < 500)
					{
						return new ReturnData
						{
							Success = false,
							Message = "Sorry, your account must remain with a minimum of KES. 500",
							Data = "0"
						};
					}
				}
				if (transaction.Amount < 50)
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, withdrawal amount should be Ksh 50 or more",
						Data = "0"
					};
				}
				if (transaction.Amount > (balance - 500))
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, your account balance is insufficient",
						Data = "0"
					};
				}
				if (transaction.Amount > 70000)
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, your cannot transact more than Ksh 70,000 at once",
						Data = "0"
					};
				}

				//get voucher number
				var Vno = $"Select ISNULL(max(ID),1) From Masters";
				Int64 getvno = db.Database.SqlQuery<Int64>(Vno).FirstOrDefault();
				Int64 nextvno = getvno + 1;
				//var floatAcc = db.PosAgents.FirstOrDefault(m => m.PosSerialNo.ToUpper().Equals(transaction.MachineID.ToUpper()));

				var flo222 = $"select AgencyCode from PosAgents where PosSerialNo= '{transaction.MachineID}' and Active=1";
				string floatAcc33 = db.Database.SqlQuery<string>(flo222).FirstOrDefault();


				if (String.IsNullOrEmpty(floatAcc33))
				{
					return new ReturnData
					{
						Success = true,
						Message = "You are not allowed to use this device",
						Data = "0"
					};

				}


				var flo1 = $"select CommissionAccNo from PosAgents A Inner Join PosUsers U On U.AgencyCode=A.AgencyCode where U.IDNo = '{transaction.AuditId}' and U.Active=1 and U.PosSerialNo ='{transaction.MachineID}'";
				string floatAcc2 = db.Database.SqlQuery<string>(flo1).FirstOrDefault();

				var flo2 = $"select AgencyName from PosAgents where PosSerialNo= '{transaction.MachineID}' and Active=1";
				string floatAcc3 = db.Database.SqlQuery<string>(flo2).FirstOrDefault();

				var opr = $"select SurName from PosUsers where IDNo= '{transaction.AuditId}'";
				string OperatorName = db.Database.SqlQuery<string>(opr).FirstOrDefault();
				var flo = $"select FloatAccNo from PosAgents where PosSerialNo= '{transaction.MachineID}' and Active=1";
				string floatAcc = db.Database.SqlQuery<string>(flo).FirstOrDefault();



				if (floatAcc != "" && floatAcc != null)
				{

					avail2 -= transaction.Amount;
					var transactionDescription = "EasyAgent Withdrawal";
					//var vNo = GetVoucherNo(transaction.Amount);
					//Store transaction 

					if (string.IsNullOrEmpty(transaction.Name) && string.IsNullOrEmpty(transaction.Did))
					{
						var oprrs = $"select SurName from Members where AccNo= '{transaction.SNo}'";
						string OperatorNamemember = db.Database.SqlQuery<string>(oprrs).FirstOrDefault();
						var opr1 = $"select IDNo from  Members where AccNo= '{transaction.SNo}'";
						string OperatorNamememberid = db.Database.SqlQuery<string>(opr1).FirstOrDefault();
						var opr12 = $"select OtherNames from  Members where AccNo= '{transaction.SNo}'";
						string OperatorNamemember1 = db.Database.SqlQuery<string>(opr12).FirstOrDefault();
						var inserttransactions = $"set dateformat dmy INSERT INTO PosReconcilliation(Name,RefNo,InitiationDate,AccNo,VoucherNo,Amount,CompletionDate,PosSerialNo,AgencyCode,InitiatedBy,Activity,Posted,productID)values('{OperatorNamemember}  {OperatorNamemember1}','{OperatorNamememberid}','{DateTime.Now}','{transaction.SNo}','{nextvno}','{transaction.Amount}','{DateTime.UtcNow.Date}','{transaction.MachineID}','{floatAcc33}','{OperatorName}','{ transactionDescription}','{false}','{"000"}')";
						db.Database.ExecuteSqlCommand(inserttransactions);

						var DepositSucess = "Withdrawal successful";
						return new ReturnData
						{
							Success = true,
							Message = $"{DepositSucess},{nextvno},{OperatorNamemember}  {OperatorNamemember1},{floatAcc3},{DateTime.Now},{OperatorNamemember}  {OperatorNamemember1},{OperatorNamememberid}",
							Data = "1"
						};
					}
					else 
					{
						var oprrs = $"select SurName from Members where AccNo= '{transaction.SNo}'";
						string OperatorNamemember = db.Database.SqlQuery<string>(oprrs).FirstOrDefault();
						var opr12 = $"select OtherNames from  Members where AccNo= '{transaction.SNo}'";
						string OperatorNamemember1 = db.Database.SqlQuery<string>(opr12).FirstOrDefault();

						var inserttransactions = $"set dateformat dmy INSERT INTO PosReconcilliation(Name,RefNo,InitiationDate,AccNo,VoucherNo,Amount,CompletionDate,PosSerialNo,AgencyCode,InitiatedBy,Activity,Posted,productID)values('{transaction.Name}','{transaction.Did}','{DateTime.Now}','{transaction.SNo}','{nextvno}','{transaction.Amount}','{DateTime.UtcNow.Date}','{transaction.MachineID}','{floatAcc33}','{OperatorName}','{ transactionDescription}','{false}','{"000"}')";
						db.Database.ExecuteSqlCommand(inserttransactions);

						var DepositSucess = "Withdrawal successful";
						return new ReturnData
						{
							Success = true,
							Message = $"{DepositSucess},{nextvno},{OperatorNamemember}  {OperatorNamemember1},{floatAcc3},{DateTime.Now},{transaction.Name},{transaction.Did}",
							Data = "1"
						};
					}
					



				}
				else
				{
					return new ReturnData
					{
						Success = false,
						Message = "You cannot transact at the moment",
						Data = "0"
					};
				}
			
			}

			catch (Exception ex)
			{
				return new ReturnData
				{
					Success = false,
					Message = "Sorry, Network error occurred",
					Data = "0"
				};
			}
		}

		[Route("balance")]
		public ReturnData Balance([FromBody] Transaction transaction)
		{
			try
			{


				var opr = $"select AccNo from CUB where AccNo= '{transaction.SNo}'";
				string member = db.Database.SqlQuery<string>(opr).FirstOrDefault();

				//var member = db.CUBs.FirstOrDefault(m => m.AccNo.ToUpper().Equals(transaction.SNo.ToUpper()));
				if (member == null)
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, account number not found",
						Data = "0"

					};
				}

				var debtQuery = $"SELECT ISNULL(Sum(Amount),0) FROM CUSTOMERBALANCE WHERE AccNO = '{transaction.SNo}' AND cash = 1 AND transtype='DR' AND TransDescription <> 'Cheque Dep(uncleared)' AND TransDescription <> 'Bounced Cheque'";
				decimal debts = db.Database.SqlQuery<decimal>(debtQuery).FirstOrDefault();
				var creditQuery = $"SELECT ISNULL(Sum(Amount),0) FROM CUSTOMERBALANCE WHERE AccNO = '{transaction.SNo}' AND cash = 1 AND transtype='CR' AND TransDescription <> 'Cheque Dep(uncleared)'";
				decimal credits = db.Database.SqlQuery<decimal>(creditQuery).FirstOrDefault();

				decimal balance = credits - debts;
				decimal MMbalance = 0;
				//decimal expectedCharges = 0;
				//function getSacco charges

				var pullFunction = $"Select Sacco_Charges+Agent_Charges+Excise_Duty From dbo.Get_POS_Charges ({balance})";
				decimal expectedCharges = db.Database.SqlQuery<decimal>(pullFunction).FirstOrDefault();
				var oprty = $"select AccountName from CUB where AccNo= '{transaction.SNo}'";
				string memberrf = db.Database.SqlQuery<string>(oprty).FirstOrDefault();
				if (memberrf == "SAVINGS ACCOUNT")
				{
					MMbalance = 500;
				}
				decimal withdrawableAmt = balance - MMbalance - expectedCharges;
				//withdrawableAmt = Math.Round(withdrawableAmt, 2);
				var pullFunction1 = $"Select (convert(int,{withdrawableAmt}/5)*5)-5";
				int RoundedWithdrawableAmt = db.Database.SqlQuery<int>(pullFunction1).FirstOrDefault();

				if (withdrawableAmt < 50)
				{
					withdrawableAmt = 0;
				}


				var opr1 = $"select SurName from PosUsers where IDNo= '{transaction.AuditId}'";
				string OperatorName = db.Database.SqlQuery<string>(opr1).FirstOrDefault();
				var flo = $"select FloatAccNo from PosAgents where PosSerialNo= '{transaction.MachineID}'";
				string floatAcc = db.Database.SqlQuery<string>(flo).FirstOrDefault();

				//var members = db.MEMBERS.FirstOrDefault(m => m.AccNo.ToUpper().Equals(transaction.AccountNo.ToUpper()));

				var flo22 = $"select MobileNo from MEMBERS where AccNo= '{transaction.SNo}'";
				string phone = db.Database.SqlQuery<string>(flo22).FirstOrDefault();



				var flo2 = $"select AgencyName from PosAgents where PosSerialNo= '{transaction.MachineID}' and Active=1";
				string floatAcc3 = db.Database.SqlQuery<string>(flo2).FirstOrDefault();

				var flo223 = $"select AgencyCode from PosAgents where PosSerialNo= '{transaction.MachineID}' and Active=1";
				string floatAcc334 = db.Database.SqlQuery<string>(flo223).FirstOrDefault();

				var transactionDescription = "EasyAgent Balance";

				//var floatAcc = db.PosAgents.FirstOrDefault(m => m.PosSerialNo.ToUpper().Equals(transaction.MachineID.ToUpper()));
				//var OperatorName = db.PosUsers.FirstOrDefault(m => m.IDNo.ToUpper().Equals(transaction.AuditId.ToUpper()));
				var oprrs = $"select SurName from Members where AccNo= '{transaction.SNo}'";
				string OperatorNamemember = db.Database.SqlQuery<string>(oprrs).FirstOrDefault();
				var opr10 = $"select IDNo from  Members where AccNo= '{transaction.SNo}'";
				string OperatorNamememberid = db.Database.SqlQuery<string>(opr10).FirstOrDefault();
				var opr12 = $"select OtherNames from  Members where AccNo= '{transaction.SNo}'";
				string OperatorNamemember1 = db.Database.SqlQuery<string>(opr12).FirstOrDefault();
				var inserttransactions = $"set dateformat dmy INSERT INTO PosReconcilliation(Name,RefNo,InitiationDate,AccNo,VoucherNo,Amount,CompletionDate,PosSerialNo,AgencyCode,InitiatedBy,Activity,Posted,productID)values('{OperatorNamemember}  {OperatorNamemember1}','{OperatorNamememberid}','{DateTime.Now}','{transaction.SNo}','{OperatorNamememberid}','{RoundedWithdrawableAmt}','{DateTime.UtcNow.Date}','{transaction.MachineID}','{floatAcc334}','{OperatorName}','{ transactionDescription}','{true}','{"000"}')";
				db.Database.ExecuteSqlCommand(inserttransactions);
				if (phone.Length > 9)
				{
					var inMess = $"set dateformat dmy insert into Messages(AccNo,Source,Telephone,Processed,AlertType,Charged,MsgType,DateReceived,Content)values('{transaction.SNo}','{OperatorName}','{phone }','{false }','{"EasyAgent Balance Inquiry"}','{false}','{"Outbox"}','{DateTime.UtcNow.Date }','{$"Account balance for account {transaction.SNo} is Ksh {balance}. Your withdrawable amount is Ksh {RoundedWithdrawableAmt} checked at {floatAcc3}"}')";
					db.Database.ExecuteSqlCommand(inMess);
				}

				//db.SaveChanges();
				return new ReturnData
				{
					Success = true,
					Message = $"{RoundedWithdrawableAmt},{floatAcc3},{OperatorNamemember}  {OperatorNamemember1}",
					Data = "1"
				};
			}
			catch (Exception ex)
			{
				return new ReturnData
				{
					Success = false,
					Message = "Sorry, An error occurred",
					Data = "0"
				};
			}
		}

		[Route("advanceLimit")]
		public ReturnData advanceLimit([FromBody] AdvanceLimitModel advanceLimitModel)
		{
			try
			{
				var fetchProductID23 = $"Select Recoverfrom from DEDUCTIONLIST where Description ='{advanceLimitModel.ProdID}'";
				string fetchProductID = db.Database.SqlQuery<string>(fetchProductID23).FirstOrDefault();

				var productDetailsQuery = $"Select dbo.Recommended_Advance ('{advanceLimitModel.Accno}','{fetchProductID}')";
				decimal RecommAdvance = db.Database.SqlQuery<decimal>(productDetailsQuery).FirstOrDefault();
				{
					return new ReturnData
					{
						Success = true,
						Message = $"{RecommAdvance}",
						Data = "1"
					};
				}
				
			}
			catch (Exception)
			{
				return new ReturnData
				{
					Success = false,
					Message = "Sorry, An error occurred",
					Data = "0"
				};
			}
		}





		[Route("calculateAdvance")]
		public ReturnData AdvanceService([FromBody] Transaction transaction)

		{
			try
			{
				var idd2 = ""; var pay1 = ""; var Acco = ""; var avail2 = ""; var mbno1 = ""; var accccc = ""; var Name = "";


				System.Data.SqlClient.SqlCommand cmd;
				string str = "Data Source=KON-DL-SERVER;Initial Catalog=KONOIN_BOSA;Integrated Security=false;user id=K-PILLAR;password=kpillar;Connection Timeout=60000; max pool size=500";

				System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(str);
				con.Open();
				try
				{
					System.Data.SqlClient.SqlCommand comPrice;
					string sqlPrice = null;
					System.Data.SqlClient.SqlDataReader dataReaderPrice;
					sqlPrice = "select Name,AvailableBalance,IDNo,Payno,AccountName,MemberNo,AccNo from CUB where AccNo = '" + transaction.SNo + "'";


					comPrice = new System.Data.SqlClient.SqlCommand(sqlPrice, con);
					dataReaderPrice = comPrice.ExecuteReader();
					if (dataReaderPrice.HasRows)
					{
						dataReaderPrice.Read();

						Name = Convert.ToString(dataReaderPrice.GetValue(0));
						avail2 = Convert.ToString(dataReaderPrice.GetValue(1));//.ToString());
						idd2 = Convert.ToString(dataReaderPrice.GetValue(2));
						pay1 = Convert.ToString(dataReaderPrice.GetValue(3));
						Acco = Convert.ToString(dataReaderPrice.GetValue(4));
						mbno1 = Convert.ToString(dataReaderPrice.GetValue(5));
						accccc = Convert.ToString(dataReaderPrice.GetValue(6));


					}

					dataReaderPrice.Close(); dataReaderPrice.Dispose(); dataReaderPrice = null;
					con.Close();
				}
				catch (Exception ex)
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, network error occurred,check your internet and try again",
						Data = "0"
					};
				}

				var memberr = $"select AccNo from CUB where AccNo= '{transaction.AccountNo}'";
				var member = db.Database.SqlQuery<string>(memberr).FirstOrDefault();

				//var member = db.CUBs.FirstOrDefault(m => m.AccNo.ToUpper().Equals(transaction.AccountNo.ToUpper()));
				if (member == null)
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, Account number not found",
						Data = "0"
					};
				}

				//var fetchProductID = db.DEDUCTIONLISTs.FirstOrDefault(m => m.Description.ToUpper().Equals(transaction.ProductDescription.ToUpper()));

				var fetchProductID23 = $"Select Recoverfrom from DEDUCTIONLIST where Description ='{transaction.ProductDescription}'";
				string fetchProductID = db.Database.SqlQuery<string>(fetchProductID23).FirstOrDefault();

				var productDetailsQuery = $"Select dbo.Recommended_Advance ('{transaction.AccountNo}','{fetchProductID}')";
				decimal RecommAdvance = db.Database.SqlQuery<decimal>(productDetailsQuery).FirstOrDefault();


				if (transaction.Amount < 200)
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, Minimum advance amount to apply should be Ksh 200 or more",
						Data = "0"
					};
				}

				if (transaction.Amount > RecommAdvance)
				{
					return new ReturnData
					{
						Success = false,
						Message = $"Your maximum advance amount for {transaction.ProductDescription} is KES {RecommAdvance}",
						Data = "0"
					};
				}
				var query = $"select TOP 1 serialno from advance where  (ISNUMERIC(serialno) = 1) AND (serialno NOT LIKE '%.%') ORDER BY LEN(serialno) DESC, serialno DESC";
				var advanceSerialno = db.Database.SqlQuery<string>(query).FirstOrDefault();
				Int64 serii = Int64.Parse(advanceSerialno);
				Int64 seri = serii + 1;
				//periods
				//var OperatorName = db.PosUsers.FirstOrDefault(m => m.IDNo.ToUpper().Equals(transaction.AuditId.ToUpper()));
				//var Advancedetails = db.DEDUCTIONLISTs.FirstOrDefault(m => m.Recoverfrom.ToUpper().Equals(fetchProductID.ToUpper()));

				var quer12 = $"select InterestRate from DEDUCTIONLIST where Recoverfrom ='{fetchProductID}'";
				double Advancedetails = db.Database.SqlQuery<double>(quer12).FirstOrDefault();

				var quer123 = $"select DedCode from DEDUCTIONLIST where Recoverfrom ='{fetchProductID}'";
				string Advancedetails1 = db.Database.SqlQuery<string>(quer123).FirstOrDefault();

				var quer1234 = $"select Description from DEDUCTIONLIST where Recoverfrom ='{fetchProductID}'";
				string Advancedetails2 = db.Database.SqlQuery<string>(quer1234).FirstOrDefault();

				var opr = $"select SurName from PosUsers where IDNo= '{transaction.AuditId}' and Active=1";
				string OperatorName = db.Database.SqlQuery<string>(opr).FirstOrDefault();

				var flo222 = $"select AgencyCode from PosAgents where PosSerialNo= '{transaction.MachineID}' and Active=1";
				string floatAcc33 = db.Database.SqlQuery<string>(flo222).FirstOrDefault();
				if (String.IsNullOrEmpty(floatAcc33))
				{
					return new ReturnData
					{
						Success = true,
						Message = "You are not allowed to use this device",
						Data = "0"
					};

				}

				//Store transaction 
				if (string.IsNullOrEmpty(transaction.Name) && string.IsNullOrEmpty(transaction.Did))
				{
					var oprrs = $"select SurName from Members where AccNo= '{transaction.SNo}'";
					string OperatorNamemember = db.Database.SqlQuery<string>(oprrs).FirstOrDefault();
					var oprrs1 = $"select SurName from Members where AccNo= '{transaction.SNo}'";
					string OperatorNamemember2 = db.Database.SqlQuery<string>(oprrs1).FirstOrDefault();
					var opr1 = $"select IDNo from  Members where AccNo= '{transaction.SNo}'";
					string OperatorNamememberid = db.Database.SqlQuery<string>(opr1).FirstOrDefault();
					var inserttransactions = $" set dateformat dmy INSERT INTO PosReconcilliation(Name,RefNo,InitiationDate,AccNo,VoucherNo,Amount,CompletionDate,PosSerialNo,AgencyCode,InitiatedBy,Activity,Posted,productID)values('{OperatorNamemember}  {OperatorNamemember2}','{OperatorNamememberid}','{DateTime.Now}','{transaction.SNo}','{seri}','{transaction.Amount}','{DateTime.UtcNow.Date}','{transaction.MachineID}','{floatAcc33}','{OperatorName}','{"Advance Request"}','{false}','{fetchProductID}')";
					db.Database.ExecuteSqlCommand(inserttransactions);
				}
				else 
				{
					var inserttransactions = $" set dateformat dmy INSERT INTO PosReconcilliation(Name,RefNo,InitiationDate,AccNo,VoucherNo,Amount,CompletionDate,PosSerialNo,AgencyCode,InitiatedBy,Activity,Posted,productID)values('{transaction.Name}','{transaction.Did}','{DateTime.Now}','{transaction.SNo}','{seri}','{transaction.Amount}','{DateTime.UtcNow.Date}','{transaction.MachineID}','{floatAcc33}','{OperatorName}','{"Advance Request"}','{false}','{fetchProductID}')";
					db.Database.ExecuteSqlCommand(inserttransactions);
				}
			
				return new ReturnData
				{

					Success = true,
					Message = $"You have successfully applied for an advance of KES {transaction.Amount}",
					Data = "1"
				};
			}

			catch (Exception ex)
			{

				return new ReturnData
				{
					Success = false,
					Message = "Sorry,an error occured",
					Data = "0"
				};
			}
		}

		//[Route("applyAdvance")]
		//public ReturnData ApplyAdvance([FromBody] Transaction transaction)
		//{
		//	var response = AdvanceService(transaction);
		//	return response;
		//}

		[Route("fetchAdvanceProducts")]
		public List<AdvanceProduct> FetchAdvanceProducts([FromBody] Transaction transaction)
		{
			try
			{
				transaction.AccountNo = transaction.AccountNo ?? "";
				var productDescriptionQuery = $"Select Distinct  I.ProductID, P.Description From INCOME I Inner Join DEDUCTIONLIST P On P.Recoverfrom=I.ProductID INNER Join PRODUCTSETUP S on S.ProductID=I.ProductID Where AccNo = '{transaction.AccountNo}' and p.DedCode <>'020' and  p.Recoverfrom not in (select RecoverFrom from DEDUCTION where AccNo='{transaction.AccountNo}' and Arrears+AmountCF+AmountIntCF>1) AND DATEDiff(dd,I.Transdate,GETDATE())<=S.intervals AND P.Mobile=1 ";

				var advanceProducts = db.Database.SqlQuery<AdvanceProduct>(productDescriptionQuery).ToList();
				return advanceProducts;

			}
			catch (Exception ex)
			{
				return new List<AdvanceProduct>();

			}
		}
		[Route("fetchAgencyAccounts")]
		public List<string> fetchAgencyAccounts([FromBody] Transaction transaction)
		{

			try
			{
				var accountsQuery = $"select AgencyName from PosAgents where";
				var accounts = db.Database.SqlQuery<string>(accountsQuery).ToList();
				return accounts;
			}
			catch (Exception ex)
			{
				return new List<string>();
			}
		}

		[Route("fetchMemberAccounts")]
		public List<string> FetchMemberAccounts([FromBody] Transaction transaction)
		{
			var accounts = new List<string>();
			try
			{
				var VerifyRegistration = $"Select IDNo from PosMembers where IDNo='{transaction.SNo}'and PosSerialNo='{transaction.MachineID}' and  Active=1";
				var checkedids = db.Database.SqlQuery<string>(VerifyRegistration).FirstOrDefault();
				if (string.IsNullOrEmpty(checkedids).Equals(false))
				{

					var getAcc = $"Select AccNo from CUB where IDNo='{transaction.SNo}'and Frozen=0";
					accounts = db.Database.SqlQuery<string>(getAcc).ToList();
				}


				return accounts;
			}
			catch (Exception ex)
			{
				return accounts;
			}
		}






	}

}

