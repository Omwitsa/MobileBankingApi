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
		TESTEntities db;
		public TransactionController()
		{
			db = new TESTEntities();
		}

		[Route("deposit")]

		public ReturnData DepositService(Transaction transaction)
		{
			try
			{
				var idd2 = ""; var pay1 = ""; var Acco = ""; decimal avail2 = 0; var mbno1 = ""; var accccc = ""; var Name = "";


				System.Data.SqlClient.SqlCommand cmd;
				string str = "Data Source=HP-PROBOOK-450;Initial Catalog=KONOIN_BOSA;Integrated Security=false;user id=K-PILLAR;password=kpillar;Connection Timeout=60000; max pool size=500";
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
						Message = "Sorry, account number not found"
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
				var opr = $"select Name from PosUsers where IDNo= '{transaction.AuditId}' and Active=1 and PosSerialNo='{transaction.MachineID}'";
				string OperatorName = db.Database.SqlQuery<string>(opr).FirstOrDefault();

				//var vNo = GetVoucherNo(transaction.Amount);
				//var floatAcc = db.PosAgents.FirstOrDefault(m => m.PosSerialNo.ToUpper().Equals(transaction.MachineID.ToUpper()));
				//float account
				var flo = $"select FloatAccNo from PosAgents A Inner Join PosUsers U On U.AgencyCode=A.AgencyCode where U.IDNo={transaction.AuditId} and U.Active=1 and U.PosSerialNo ='{transaction.MachineID}'";
				string floatAcc = db.Database.SqlQuery<string>(flo).FirstOrDefault();
				if (floatAcc != "" && floatAcc != null)
				{


					//string floatAcc = "840";

					var flo1 = $"select CommissionAccNo from PosAgents A Inner Join PosUsers U On U.AgencyCode=A.AgencyCode where U.IDNo={transaction.AuditId} and U.Active=1 and U.PosSerialNo ='{transaction.MachineID}'";
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
							Message = "Sorry, Your float balance is insufficient"
						};
					}
					else
					{
						//Store transaction 
						var inserttransactions = $"INSERT INTO PosReconcilliation(InitiationDate,AccNo,VoucherNo,Amount,CompletionDate,PosSerialNo,AgencyCode,InitiatedBy,Activity,Posted,productID)values('{DateTime.Now}','{transaction.SNo}','{nextvno}','{transaction.Amount}','{DateTime.Now}','{transaction.MachineID}','{floatAcc33}','{OperatorName}','{transactionDescription}','{false}','{"000"}')";
						db.Database.ExecuteSqlCommand(inserttransactions);

						//insertCustomerbalance 
						var insertCustomerbalance = $"INSERT INTO CustomerBalance(IDNo,PayrollNo,CustomerNo,vno,AccName,Amount,MachineID,TransactionNo,AuditID,AvailableBalance,TransDescription,TransDate,ReconDate,AccNO,valuedate,transType,Status,Cash)values('{ idd2}','{pay1}','{pay1}','{nextvno}','{Acco}','{transaction.Amount}','{transaction.MachineID}','{nextvno}','{OperatorName}','{avail2}','{transactionDescription}','{DateTime.UtcNow.Date}','{DateTime.UtcNow.Date}','{accccc}','{ DateTime.UtcNow.Date}','{ "CR"}','{ true}','{true}')";
						db.Database.ExecuteSqlCommand(insertCustomerbalance);



						//insert the first gl
						var IsertGl = $"INSERT INTO GLTRANSACTIONS (TransDate,Amount,DocumentNo,TransactionNo,AuditTime,AuditID,DrAccNo,CrAccNo,TransDescript,Source )values('{DateTime.UtcNow.Date}','{transaction.Amount}','{nextvno}','{nextvno}','{DateTime.Now}','{OperatorName}','{floatAcc}','{"942"}','{transactionDescription}','{mbno1}')";
						db.Database.ExecuteSqlCommand(IsertGl);

						//Agent deposit commission
						var pullFunction2 = $"Select Expense_Amount From dbo.Get_POS_Expenses ('{transaction.Amount}','Deposit')";
						decimal AgentDepositCommission = db.Database.SqlQuery<decimal>(pullFunction2).FirstOrDefault();


						//insert the second gl	
						var insertgl2 = $"INSERT INTO GLTRANSACTIONS (TransDate,Amount,DocumentNo,TransactionNo,DrAccNo,CrAccNo,TransDescript,AuditTime,AuditID,Source) values('{DateTime.UtcNow.Date}','{AgentDepositCommission}','{nextvno}','{nextvno}','{"205"}','{floatAcc2}','{"EasyAgent Deposit Commission"}','{DateTime.Now}','{OperatorName}','{mbno1}')";
						db.Database.ExecuteSqlCommand(insertgl2);



						var insertMasters = $"INSERT INTO Masters(Transdate,Source,Productcode,ProdType,Amount,Refno,Transcode,Users,TransactionNo,Machine,Transtype)values('{DateTime.UtcNow.Date}','{"EasyAgent transactions"}','{"Deposits"}','{"EasyAgentDeposits"}','{transaction.Amount}','{nextvno}','{nextvno}','{OperatorName}','{"Deposit transactions"}','{transaction.MachineID}','{"CR"}')";
						db.Database.ExecuteSqlCommand(insertMasters);

						//var members = db.MEMBERS.FirstOrDefault(m => m.AccNo.ToUpper().Equals(transaction.SNo.ToUpper()));
						var flo22 = $"select MobileNo from MEMBERS where AccNo= '{transaction.SNo}'";
						string phone = db.Database.SqlQuery<string>(flo22).FirstOrDefault();
						if (phone.Length > 9)
						{

							//Insert Message
							var insertMessge = $"INSERT INTO Messages (AccNo,Source,Telephone,Processed,AlertType,Charged,MsgType,DateReceived,Content)values('{accccc}','{OperatorName}','{phone}','{false}','{"EasyAgent Deposit"}','{false}','{"Outbox"}','{DateTime.UtcNow.Date}','{$"Deposit of Ksh {transaction.Amount} on {DateTime.UtcNow.Date} to account {transaction.SNo} at {floatAcc3} was successful. Reference Number{nextvno}."}')";
							db.Database.ExecuteSqlCommand(insertMessge);


						}
						//Update transaction 
						var updatetransactions = $"UPDATE PosReconcilliation set Posted=1,CompletionDate='{DateTime.Now}' where AccNo='{transaction.SNo}' and VoucherNo='{nextvno}' and Amount='{transaction.Amount}' and PosSerialNo='{transaction.MachineID}' and AgencyCode='{floatAcc33}' and InitiatedBy='{OperatorName}'and Activity='{transactionDescription}' and Posted=0";
						db.Database.ExecuteSqlCommand(updatetransactions);



					}


				}
				else
				{
					return new ReturnData
					{
						Success = false,
						Message = "You cannot transact at the moment"
					};
				}
				return new ReturnData
				{
					Success = true,
					Message = "Deposit successful"
				};
			}
			catch (Exception ex)
			{
				return new ReturnData
				{
					Success = false,
					Message = "Sorry, network error occurred,check your internet and try again"
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
				string str = "Data Source=HP-PROBOOK-450;Initial Catalog=KONOIN_BOSA;Integrated Security=false;user id=K-PILLAR;password=kpillar;Connection Timeout=60000; max pool size=500";
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
						Message = "Sorry, network error occurred,check your internet and try again"
					};
				}


				var memberr = $"select AccNo from CUB where AccNo= '{transaction.SNo}'";
				var member = db.Database.SqlQuery<string>(memberr).FirstOrDefault();
				if (member == null)
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, account number not found"
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
							Message = "Sorry, your account must remain with a minimum of KES. 500"
						};
					}
				}
				if (transaction.Amount < 50)
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, withdrawal amount should be Ksh 50 or more"
					};
				}
				if (transaction.Amount > (balance - 500))
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, your account balance is insufficient"
					};
				}
				if (transaction.Amount > 70000)
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, your cannot transact more than Ksh 70,000 at once"
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
					};

				}


				var flo1 = $"select CommissionAccNo from PosAgents A Inner Join PosUsers U On U.AgencyCode=A.AgencyCode where U.IDNo={transaction.AuditId} and U.Active=1 and U.PosSerialNo ='{transaction.MachineID}'";
				string floatAcc2 = db.Database.SqlQuery<string>(flo1).FirstOrDefault();

				var flo2 = $"select AgencyName from PosAgents where PosSerialNo= '{transaction.MachineID}'";
				string floatAcc3 = db.Database.SqlQuery<string>(flo2).FirstOrDefault();

				var opr = $"select Name from PosUsers where IDNo= '{transaction.AuditId}'";
				string OperatorName = db.Database.SqlQuery<string>(opr).FirstOrDefault();
				var flo = $"select FloatAccNo from PosAgents where PosSerialNo= '{transaction.MachineID}'";
				string floatAcc = db.Database.SqlQuery<string>(flo).FirstOrDefault();



				if (floatAcc != "" && floatAcc != null)
				{

					avail2 -= transaction.Amount;
					var transactionDescription = "EasyAgent Withdrawal";
					//var vNo = GetVoucherNo(transaction.Amount);
					//Store transaction 
					var inserttransactions = $"INSERT INTO PosReconcilliation(InitiationDate,AccNo,VoucherNo,Amount,CompletionDate,PosSerialNo,AgencyCode,InitiatedBy,Activity,Posted,productID)values('{DateTime.Now}','{transaction.SNo}','{nextvno}','{transaction.Amount}','{DateTime.UtcNow.Date}','{transaction.MachineID}','{floatAcc33}','{OperatorName}','{ transactionDescription}','{true}','{"000"}')";
					db.Database.ExecuteSqlCommand(inserttransactions);


					var insercustomer = $"INSERT INTO CustomerBalance(IDNo,PayrollNo,CustomerNo,vno,AccName,Auditid,Amount,MachineID,TransactionNo,AvailableBalance,TransDescription,TransDate,ReconDate,AccNO,valuedate,transType,Status,Cash)values ('{idd2}','{pay1}','{pay1}','{nextvno}','{Acco}','{OperatorName}','{transaction.Amount}','{transaction.MachineID}','{nextvno}','{avail2}','{transactionDescription}','{DateTime.UtcNow.Date}','{DateTime.UtcNow.Date}','{accccc}','{DateTime.UtcNow.Date}','{"DR"}', '{true}','{true}')";
					db.Database.ExecuteSqlCommand(insercustomer);

					var gls = $"INSERT INTO GLTRANSACTIONS (TransDate,Amount,DocumentNo,TransactionNo,DrAccNo,CrAccNo,TransDescript,AuditTime ,AuditID,Source)values ('{DateTime.UtcNow.Date}','{transaction.Amount}','{nextvno}','{nextvno}','{"942"}','{floatAcc}','{transactionDescription}','{DateTime.Now}','{OperatorName}','{mbno1}')";
					db.Database.ExecuteSqlCommand(gls);

					var insertMasters = $"INSERT INTO Masters(Transdate,Source,Productcode,ProdType,Amount,Refno,Transcode,Users,TransactionNo,Machine,Transtype)values('{DateTime.UtcNow.Date}','{"EasyAgent transactions"}','{"Withdrawal"}','{"EasyAgentWithdrawal"}','{transaction.Amount}','{nextvno}','{nextvno}','{OperatorName}','{"Withdrawal transactions"}','{transaction.MachineID}','{"DR"}')";
					db.Database.ExecuteSqlCommand(insertMasters);



					//function getSacco charges

					var pullFunction = $"Select Sacco_Charges From dbo.Get_POS_Charges ('{transaction.Amount}')";
					decimal poscheckid1 = db.Database.SqlQuery<decimal>(pullFunction).FirstOrDefault();
					//function getAgent charges
					var pullFunction1 = $"Select Agent_Charges From dbo.Get_POS_Charges ('{transaction.Amount}')";
					decimal poscheckid2 = db.Database.SqlQuery<decimal>(pullFunction1).FirstOrDefault();
					//function getExcise charges
					var pullFunction2 = $"Select Excise_Duty From dbo.Get_POS_Charges ('{transaction.Amount}')";
					decimal poscheckid3 = db.Database.SqlQuery<decimal>(pullFunction2).FirstOrDefault();

					var saccoCommission = poscheckid1;
					var agentCommision = poscheckid2;
					decimal totalCommission = saccoCommission + agentCommision;


					avail2 -= totalCommission;
					transactionDescription = "EasyAgent Withdrawal Commission";
					//inser customerbalance
					var insertcustomer = $"INSERT INTO CustomerBalance (IDNo,PayrollNo,CustomerNo,vno,AccName,Auditid,Amount,MachineID,TransactionNo,AvailableBalance,TransDescription,TransDate,ReconDate,AccNO,valuedate,transType,Status,Cash)values('{idd2}','{pay1}','{pay1}','{nextvno}','{Acco}','{OperatorName}','{totalCommission}','{transaction.MachineID}','{nextvno}','{avail2}','{transactionDescription}','{DateTime.UtcNow.Date}','{DateTime.UtcNow.Date}','{accccc}','{DateTime.UtcNow.Date}','{"DR"}','{true}','{true}')";
					db.Database.ExecuteSqlCommand(insertcustomer);


					//total  Commission
					var cretetrnas1 = $"INSERT INTO GLTRANSACTIONS(TransDate,Amount,DocumentNo,TransactionNo,AuditTime,AuditID,DrAccNo,CrAccNo,TransDescript,Source)values('{DateTime.UtcNow.Date}','{totalCommission}','{nextvno}','{nextvno}','{DateTime.Now}','{OperatorName}','{"942"}','{"958"}','{transactionDescription}','{mbno1}')";
					db.Database.ExecuteSqlCommand(cretetrnas1);




					//Sacco Commission
					var cretetrnas = $"INSERT INTO GLTRANSACTIONS(TransDate,Amount,DocumentNo,TransactionNo,AuditTime,AuditID,DrAccNo,CrAccNo,TransDescript,Source)values('{DateTime.UtcNow.Date}','{poscheckid1}','{nextvno}','{nextvno}','{DateTime.Now}','{OperatorName}','{"958"}','{"022"}','{transactionDescription}','{mbno1}')";
					db.Database.ExecuteSqlCommand(cretetrnas);



					//agent Commission
					var insertglss = $"INSERT INTO GLTRANSACTIONS (TransDate,Amount,DocumentNo,TransactionNo,AuditTime,AuditID,DrAccNo,CrAccNo,TransDescript,Source) values('{DateTime.UtcNow.Date}','{poscheckid2}','{nextvno}','{nextvno}','{DateTime.Now}','{OperatorName}','{"958"}','{floatAcc2}','{transactionDescription}','{mbno1}')";
					db.Database.ExecuteSqlCommand(insertglss);




					var Excise_duty = poscheckid3;
					avail2 -= Excise_duty;
					transactionDescription = "Excise duty";

					//insert customerbalance
					var cust = $"INSERT INTO CustomerBalance (IDNo,PayrollNo,CustomerNo,vno,AccName,Amount,TransactionNo,MachineID,Auditid,AvailableBalance,TransDescription,TransDate,ReconDate,AccNO,valuedate,transType,Status,Cash) values ('{idd2}','{pay1}','{pay1}','{nextvno}','{Acco}','{Excise_duty}','{transaction.MachineID}','{OperatorName}','{nextvno}','{avail2}','{transactionDescription}','{DateTime.UtcNow.Date}','{DateTime.UtcNow.Date}','{Acco}','{DateTime.UtcNow.Date}','{"DR"}','{true}','{true}')";
					db.Database.ExecuteSqlCommand(cust);



					//Insert Message gl1
					var insertGltrans = $"INSERT INTO GLTRANSACTIONS (TransDate,Amount,DocumentNo,TransactionNo,AuditTime,AuditID,DrAccNo,CrAccNo,TransDescript,Source) values('{DateTime.UtcNow.Date}','{Excise_duty}','{nextvno}','{nextvno}','{DateTime.Now}','{OperatorName}','{"942"}','{"956"}','{transactionDescription}','{mbno1}')";
					db.Database.ExecuteSqlCommand(insertGltrans);

					//var members = db.MEMBERS.FirstOrDefault(m => m.AccNo.ToUpper().Equals(transaction.AccountNo.ToUpper()));
					var flo22 = $"select MobileNo from MEMBERS where AccNo= '{transaction.SNo}'";
					string phone = db.Database.SqlQuery<string>(flo22).FirstOrDefault();

					if (phone.Length > 9)
					{
						//Insert Message
						var insertMessge1 = $"INSERT INTO Messages (AccNo,Source,Telephone,Processed,AlertType,Charged,MsgType,DateReceived,Content)values('{Acco}','{OperatorName}','{phone}','{false}','{"EasyAgent Withdrawal"}','{false}','{"Outbox"}','{DateTime.UtcNow.Date}','{$"Withdrawal of Ksh {transaction.Amount} on {DateTime.UtcNow.Date} from account {transaction.SNo} at {floatAcc3} successful. Reference Number{nextvno}."}')";
						db.Database.ExecuteSqlCommand(insertMessge1);

					}
					//Update transaction 
					var updatetransactions = $"UPDATE PosReconcilliation set Posted=1,CompletionDate='{DateTime.Now}' where AccNo='{transaction.SNo}' and VoucherNo='{nextvno}' and Amount='{transaction.Amount}'and PosSerialNo='{transaction.MachineID}' and AgencyCode='{floatAcc33}' and InitiatedBy='{OperatorName}' and Activity='{transactionDescription}' and Posted=0";
					db.Database.ExecuteSqlCommand(updatetransactions);


				}
				else
				{
					return new ReturnData
					{
						Success = false,
						Message = "You cannot transact at the moment"
					};
				}
				return new ReturnData
				{
					Success = true,
					Message = "Withdrawal successful"
				};
			}

			catch (Exception ex)
			{
				return new ReturnData
				{
					Success = false,
					Message = "Sorry, Network error occurred"
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
						Message = "Sorry, account number not found"
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


				var opr1 = $"select Name from PosUsers where IDNo= '{transaction.AuditId}'";
				string OperatorName = db.Database.SqlQuery<string>(opr1).FirstOrDefault();
				var flo = $"select FloatAccNo from PosAgents where PosSerialNo= '{transaction.MachineID}'";
				string floatAcc = db.Database.SqlQuery<string>(flo).FirstOrDefault();

				//var members = db.MEMBERS.FirstOrDefault(m => m.AccNo.ToUpper().Equals(transaction.AccountNo.ToUpper()));

				var flo22 = $"select MobileNo from MEMBERS where AccNo= '{transaction.SNo}'";
				string phone = db.Database.SqlQuery<string>(flo22).FirstOrDefault();



				var flo2 = $"select AgencyName from PosAgents where PosSerialNo= '{transaction.MachineID}'";
				string floatAcc3 = db.Database.SqlQuery<string>(flo2).FirstOrDefault();


				//var floatAcc = db.PosAgents.FirstOrDefault(m => m.PosSerialNo.ToUpper().Equals(transaction.MachineID.ToUpper()));
				//var OperatorName = db.PosUsers.FirstOrDefault(m => m.IDNo.ToUpper().Equals(transaction.AuditId.ToUpper()));
				if (phone.Length > 9)
				{
					var inMess = $"insert into Messages(AccNo,Source,Telephone,Processed,AlertType,Charged,MsgType,DateReceived,Content)values('{transaction.SNo}','{OperatorName}','{phone }','{false }','{"EasyAgent Balance Inquiry"}','{false}','{"Outbox"}','{DateTime.UtcNow.Date }','{$"Account balance for account {transaction.SNo} is Ksh {balance}. Your withdrawable amount is Ksh {RoundedWithdrawableAmt} checked at Agency Name {floatAcc3}"}')";
					db.Database.ExecuteSqlCommand(inMess);
				}

				//db.SaveChanges();
				return new ReturnData
				{
					Success = true,
					Message = $"{RoundedWithdrawableAmt}",
					Data = balance,
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
		[Route("calculateAdvance")]
		public ReturnData AdvanceService([FromBody] Transaction transaction)

		{
			try
			{
				var idd2 = ""; var pay1 = ""; var Acco = ""; var avail2 = ""; var mbno1 = ""; var accccc = ""; var Name = "";


				System.Data.SqlClient.SqlCommand cmd;
				string str = "Data Source=HP-PROBOOK-450;Initial Catalog=KONOIN_BOSA;Integrated Security=false;user id=K-PILLAR;password=kpillar;Connection Timeout=60000; max pool size=500";

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
						Message = "Sorry, network error occurred,check your internet and try again"
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
						Message = "Sorry, Account number not found"
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
						Message = "Sorry, Minimum advance amount to apply should be Ksh 200 or more"
					};
				}

				if (transaction.Amount > RecommAdvance)
				{
					return new ReturnData
					{
						Success = false,
						Message = $"Your maximum advance amount for {transaction.ProductDescription} is KES {RecommAdvance}"
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

				var opr = $"select Name from PosUsers where IDNo= '{transaction.AuditId}' and Active=1";
				string OperatorName = db.Database.SqlQuery<string>(opr).FirstOrDefault();

				var flo222 = $"select AgencyCode from PosAgents where PosSerialNo= '{transaction.MachineID}' and Active=1";
				string floatAcc33 = db.Database.SqlQuery<string>(flo222).FirstOrDefault();
				if (String.IsNullOrEmpty(floatAcc33))
				{
					return new ReturnData
					{
						Success = true,
						Message = "You are not allowed to use this device",
					};

				}

				//Store transaction 
				var inserttransactions = $"INSERT INTO PosReconcilliation(InitiationDate,AccNo,VoucherNo,Amount,CompletionDate,PosSerialNo,AgencyCode,InitiatedBy,Activity,Posted,productID)values('{DateTime.Now}','{transaction.SNo}','{seri}','{transaction.Amount}','{DateTime.UtcNow.Date}','{transaction.MachineID}','{floatAcc33}','{OperatorName}','{"Advance Request"}','{true}','{fetchProductID}')";
				db.Database.ExecuteSqlCommand(inserttransactions);

				var insertAdvance = $"INSERT INTO Advance (accno,appamnt,advdate,auditid,serialno,description,payrollno,name,ProductID,custno,amntapp,IntRate,period,audittime)values('{transaction.AccountNo}','{transaction.Amount}','{DateTime.UtcNow.Date}','{OperatorName}','{seri}','{transaction.ProductDescription}','{pay1}','{Name}','{fetchProductID}','{pay1}','{transaction.Amount}','{(int?)Advancedetails}','{3}','{DateTime.Now}')";
				db.Database.ExecuteSqlCommand(insertAdvance);

				var insertDeduct = $"INSERT INTO DEDUCTION(AccNo,Amount,TransDate,AuditID,VoucherNo,ProductID,CustNo,AmountCF,AmountIntCF,AmountInterest,Period,AuditDateTime,RecoverFrom,DedCode,Arrears,IntDate,LastTransDate,MaturityDate)values('{transaction.AccountNo}','{transaction.Amount}','{DateTime.UtcNow.Date}','{OperatorName}','{seri}','{Advancedetails1}','{mbno1}','{transaction.Amount}','{((int?)Advancedetails / 100) * transaction.Amount}','{((int?)Advancedetails / 100) * transaction.Amount}','{3}','{DateTime.Now}','{fetchProductID}','{Advancedetails1}','{0}','{DateTime.UtcNow.Date.AddMonths(1)}','{DateTime.Now}','{DateTime.Now}')";
				db.Database.ExecuteSqlCommand(insertDeduct);

				var InCustom = $"INSERT INTO CustomerBalance(IDNo,PayrollNo,CustomerNo,vno,AccName,Amount,MachineID,TransactionNo,Auditid,AvailableBalance,TransDescription,TransDate,ReconDate,AccNO,valuedate,transType,Status,Cash)values('{idd2}','{pay1}','{pay1}','{seri}','{Acco}','{transaction.Amount}','{transaction.MachineID}','{seri}','{OperatorName}','{avail2}','{Advancedetails2}','{DateTime.UtcNow.Date}','{ DateTime.UtcNow.Date}','{Acco}','{DateTime.UtcNow.Date}','{"CR"}','{true}','{true}')";
				db.Database.ExecuteSqlCommand(InCustom);

				var glsss = $"INSERT INTO GLTRANSACTIONS(TransDate,Amount,DocumentNo,TransactionNo,AuditTime,AuditID,DrAccNo,CrAccNo,TransDescript,Source)values ('{DateTime.UtcNow.Date}','{transaction.Amount}','{seri}','{seri}','{DateTime.Now}','{OperatorName}','{"875"}','{"942"}','{Advancedetails2}','{mbno1}')";


				//var floatAcc = db.PosAgents.FirstOrDefault(m => m.PosSerialNo.ToUpper().Equals(transaction.MachineID.ToUpper()));
				//var members = db.MEMBERS.FirstOrDefault(m => m.AccNo.ToUpper().Equals(transaction.AccountNo.ToUpper()));

				var flo = $"select AgencyName from PosAgents where PosSerialNo= '{transaction.MachineID}' and Active=1";
				string floatAcc = db.Database.SqlQuery<string>(flo).FirstOrDefault();
				var mob = $"select MobileNo from MEMBERS where AccNo= '{transaction.SNo}'";
				string floatAcc9 = db.Database.SqlQuery<string>(mob).FirstOrDefault();

				if (floatAcc9.Length > 9)
				{

					var insertMessge = $"INSERT INTO Messages(AccNo,Source,Telephone,Processed,AlertType,Charged,MsgType,DateReceived,Content)values('{transaction.AccountNo}','{OperatorName}','{floatAcc9}','{false}','{"EasyAgent Advance"}','{false}','{"Outbox"}','{DateTime.UtcNow.Date}','{$"Request for {Advancedetails2} of Ksh {transaction.Amount} to your account number {transaction.AccountNo} at {floatAcc} was successful. Reference Number is {seri}"}')";
					db.Database.ExecuteSqlCommand(insertMessge);

				}
				//Update transaction 
				var updatetransactions = $"UPDATE PosReconcilliation set Posted=1,CompletionDate='{DateTime.Now}' where AccNo='{transaction.SNo}' and VoucherNo='{seri}' and Amount='{transaction.Amount}'and PosSerialNo='{transaction.MachineID}' and AgencyCode='{floatAcc33}' and InitiatedBy='{OperatorName}',Activity='{"Advance Request"}' and Posted=0";
				db.Database.ExecuteSqlCommand(updatetransactions);
				return new ReturnData
				{
					Success = true,
					Message = $"You have successfully applied for an advance of KES {transaction.Amount}"
				};
			}

			catch (Exception ex)
			{

				return new ReturnData
				{
					Success = false,
					Message = "Sorry,an error occured"
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
				var getAcc = $"Select AccNo from CUB where IDNo='{transaction.SNo}'and Frozen=0";
				accounts = db.Database.SqlQuery<string>(getAcc).ToList();
				return accounts;
			}
			catch (Exception ex)
			{
				return accounts;
			}
		}






	}

}

