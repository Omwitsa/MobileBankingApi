﻿using MobileBanking_API.Models;
using MobileBanking_API.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MobileBanking_API.Controllers
{
	[RoutePrefix("webservice/transacions")]
	public class TransactionController : ApiController
    {
		TESTEntities1 db;
		public TransactionController()
		{
			db = new TESTEntities1();
		}

		[Route("deposit")]
		public ReturnData Deposit([FromBody] Transaction transaction)
		{
			var response = DepositService(transaction);
			return response;
		}

		private ReturnData DepositService(Transaction transaction)
		{
			try
			{
				var member = db.CUBs.FirstOrDefault(m => m.AccNo.ToUpper().Equals(transaction.SNo.ToUpper()));
				if (member == null)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, account number not found"
					};
				member.AvailableBalance += transaction.Amount;
				var transactionDescription = "Cash Deposit";

				var vNo = GetVoucherNo(transaction.Amount);
				var floatAcc = db.PosAgents.FirstOrDefault(m => m.PosSerialNo.ToUpper().Equals(transaction.MachineID.ToUpper()));
				//debit balance for the float account;
				var drBalance = $"Select SUM(Amount) From GLTransactions Where DrAccNo='{floatAcc.FloatAccNo}'";
				decimal poscheckid = db.Database.SqlQuery<decimal>(drBalance).FirstOrDefault();
				//credit balance for the float account;
				var crBalance = $"Select SUM(Amount) From GLTransactions Where CrAccNo='{floatAcc.FloatAccNo}'";
				decimal poscheckid1 = db.Database.SqlQuery<decimal>(crBalance).FirstOrDefault();
				decimal TellerBalance = poscheckid - poscheckid1;
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

					db.CustomerBalances.Add(new CustomerBalance
					{
						IDNo = member.IDNo,
						PayrollNo = member.Payno,
						CustomerNo = member.Payno,
						vno = vNo,
						AccName = member.AccountName,
						Amount = transaction.Amount,
						MachineID = transaction.MachineID,
						TransactionNo = vNo,
						Auditid = transaction.AuditId,
						AvailableBalance = member.AvailableBalance,
						TransDescription = transactionDescription,
						TransDate = DateTime.UtcNow.Date,
						ReconDate = DateTime.UtcNow.Date,
						AccNO = member.AccNo,
						valuedate = DateTime.UtcNow.Date,
						transType = "CR",
						Status = true,
						Cash = true,
					});


					db.GLTRANSACTIONS.Add(new GLTRANSACTION
					{
						TransDate = DateTime.UtcNow.Date,
						Amount = transaction.Amount,
						DocumentNo = vNo,
						TransactionNo = vNo,
						AuditTime = DateTime.UtcNow.AddHours(3),
						AuditID = transaction.AuditId,
						DrAccNo = floatAcc.FloatAccNo,
						CrAccNo = "942",
						TransDescript = transactionDescription,
						Source = member.MemberNo
					});

					//var memberDetails = db.MEMBERS.FirstOrDefault(m => m.MemberNo.ToUpper().Equals(transaction.SNo.ToUpper()));
					db.Messages.Add(new Message
					{
						AccNo = member.AccNo,
						Source = transaction.AuditId,
						Telephone = member.Phone,
						Processed = false,
						AlertType = "Pos Deposit",
						Charged = false,
						MsgType = "Outbox",
						DateReceived = DateTime.UtcNow.Date,
						Content = $"Pos deposit of Ksh {transaction.Amount} on {DateTime.UtcNow.Date} has been credited to account {transaction.SNo}. Reference Number{vNo}."

					});
				}

                db.SaveChanges();
				return new ReturnData
				{
					Success = true,
					Message = "Deposited sucessfully"
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
		public ReturnData Withdraw([FromBody] Transaction transaction)
		{
			var response = WithdrawalService(transaction);
			return response;
		}

		private ReturnData WithdrawalService(Transaction transaction)
		{
			try
			{
				var member = db.CUBs.FirstOrDefault(m => m.AccNo.ToUpper().Equals(transaction.SNo.ToUpper()));
				if (member == null)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, account number not found"
					};

                
                if (member.AvailableBalance < 500)
                    return new ReturnData
                    {
                        Success = false,
                        Message = "Sorry, your account must remain with a minimum of KES. 500"
                    };
				if (transaction.Amount > 70000)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, your cannot transact more than Ksh 70,000 at once"
					};

				var floatAcc = db.PosAgents.FirstOrDefault(m => m.PosSerialNo.ToUpper().Equals(transaction.MachineID.ToUpper()));
		

				member.AvailableBalance -= transaction.Amount;
				var transactionDescription = "Pos Withdrawal";
				var vNo = GetVoucherNo(transaction.Amount);

				db.CustomerBalances.Add(new CustomerBalance
				{
					IDNo = member.IDNo,
					PayrollNo = member.Payno,
					CustomerNo = member.Payno,
					vno = vNo,
					AccName = member.AccountName,
					Auditid = transaction.AuditId,
					Amount = transaction.Amount,
					MachineID = transaction.MachineID,
					TransactionNo = vNo,
					AvailableBalance = member.AvailableBalance,
					TransDescription = transactionDescription,
					TransDate = DateTime.UtcNow.Date,
					ReconDate = DateTime.UtcNow.Date,
					AccNO = member.AccNo,
					valuedate = DateTime.UtcNow.Date,
					transType = "DR",
					Status = true,
					Cash = true,
				});

				//function getSacco charges
				
				var pullFunction = $"Select Sacco_Charges From dbo.Get_POS_Charges '{transaction.Amount}'";
				decimal poscheckid1 = db.Database.SqlQuery<decimal>(pullFunction).FirstOrDefault();
				//function getAgent charges
				var pullFunction1 = $"Select Agent_Charges From dbo.Get_POS_Charges '{transaction.Amount}'";
				decimal poscheckid2 = db.Database.SqlQuery<decimal>(pullFunction1).FirstOrDefault();
				//function getExcise charges
				var pullFunction2 = $"Select Excise_Duty From dbo.Get_POS_Charges '{transaction.Amount}'";
				decimal poscheckid3 = db.Database.SqlQuery<decimal>(pullFunction2).FirstOrDefault();
				var Withdrawal_Charges = 50;
				var saccoCommission = poscheckid1;
				var agentCommision = poscheckid2;
				decimal totalCommission = saccoCommission + agentCommision;

				db.GLTRANSACTIONS.Add(new GLTRANSACTION
				{
					TransDate = DateTime.UtcNow.Date,
					Amount = transaction.Amount,
					DocumentNo = vNo,
					TransactionNo = vNo,
					DrAccNo = "942",
					CrAccNo = floatAcc.FloatAccNo,
					TransDescript = transactionDescription,
					AuditTime = DateTime.UtcNow.AddHours(3),
					AuditID = transaction.AuditId,
					agentCommission = (decimal)agentCommision,
					saccoCommission = (decimal)saccoCommission,
					Source = member.MemberNo
				});

				member.AvailableBalance -= Withdrawal_Charges;
				transactionDescription = "Pos Withdrawal Charge";
				db.CustomerBalances.Add(new CustomerBalance
				{
					IDNo = member.IDNo,
					PayrollNo = member.Payno,
					CustomerNo = member.Payno,
					vno = vNo,
					AccName = member.AccountName,
					Auditid = transaction.AuditId,
					Amount = totalCommission,
					MachineID = transaction.MachineID,
					TransactionNo = vNo,
					AvailableBalance = member.AvailableBalance,
					TransDescription = transactionDescription,
					TransDate = DateTime.UtcNow.Date,
					ReconDate = DateTime.UtcNow.Date,
					AccNO = member.AccNo,
					valuedate = DateTime.UtcNow.Date,
					transType = "DR",
					Status = true,
					Cash = true
				});

				db.GLTRANSACTIONS.Add(new GLTRANSACTION
				{
					TransDate = DateTime.UtcNow.Date,
					Amount = totalCommission,
					DocumentNo = vNo,
					TransactionNo = vNo,
					AuditTime = DateTime.UtcNow.AddHours(3),
					AuditID = transaction.AuditId,
					DrAccNo = "942",
                 //temporary account
					CrAccNo = "897",
					TransDescript = transactionDescription,
					Source = member.MemberNo
				});
//Sacco Commission
				db.GLTRANSACTIONS.Add(new GLTRANSACTION
				{
					TransDate = DateTime.UtcNow.Date,
					Amount = saccoCommission,
					DocumentNo = vNo,
					TransactionNo = vNo,
					AuditTime = DateTime.UtcNow.AddHours(3),
					AuditID = transaction.AuditId,
					DrAccNo = "897",
					//temporary account1
					CrAccNo = "022",
					TransDescript = transactionDescription,
					Source = member.MemberNo
				});
	//Agent Commission
				db.GLTRANSACTIONS.Add(new GLTRANSACTION
				{
					TransDate = DateTime.UtcNow.Date,
					Amount = agentCommision,
					DocumentNo = vNo,
					TransactionNo = vNo,
					AuditTime = DateTime.UtcNow.AddHours(3),
					AuditID = transaction.AuditId,
					DrAccNo = "897",
					//temporary account1
					CrAccNo = floatAcc.CommissionAccNo,
					TransDescript = transactionDescription,
					Source = member.MemberNo
				});

				var Excise_duty = poscheckid3;
				member.AvailableBalance -= Excise_duty;
				transactionDescription = "Excise duty";
				db.CustomerBalances.Add(new CustomerBalance
				{
					IDNo = member.IDNo,
					PayrollNo = member.Payno,
					CustomerNo = member.Payno,
					vno = vNo,
					AccName = member.AccountName,
					Amount = Excise_duty,
					MachineID = transaction.MachineID,
					Auditid = transaction.AuditId,
					TransactionNo = vNo,
					AvailableBalance = member.AvailableBalance,
					TransDescription = transactionDescription,
					TransDate = DateTime.UtcNow.Date,
					ReconDate = DateTime.UtcNow.Date,
					AccNO = member.AccNo,
					valuedate = DateTime.UtcNow.Date,
					transType = "DR",
					Status = true,
					Cash = true
				});

				db.GLTRANSACTIONS.Add(new GLTRANSACTION
				{
					TransDate = DateTime.UtcNow.Date,
					Amount = Excise_duty,
					DocumentNo = vNo,
					TransactionNo = vNo,
					AuditTime = DateTime.UtcNow.AddHours(3),
					AuditID = transaction.AuditId,
					DrAccNo = "942",
					CrAccNo = "956",
					TransDescript = transactionDescription,
					Source = member.MemberNo
				});

				db.SaveChanges();
                db.Messages.Add(new Message
                {
                    AccNo = member.AccNo,
                    Source = transaction.AuditId,
                    Telephone = member.Phone,
                    Processed = false,
                    AlertType = "Pos Withdrawal",
                    Charged = false,
                    MsgType = "Outbox",
                    DateReceived = DateTime.UtcNow.Date,
                    Content = $"Pos Withdrawal of Ksh {transaction.Amount} on {DateTime.UtcNow.Date} has been debited from account {transaction.SNo}. Reference Number{vNo}."

                });

                db.SaveChanges();
                return new ReturnData
				{
					Success = true,
					Message = "Withdrawn successfully"
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

		[Route("balance")]
		public ReturnData Balance([FromBody] Transaction transaction)
		{
			try
			{
                //var isMember = db.CustomerBalances.Any(b => b.AccNO.ToUpper().Equals(transaction.SNo.ToUpper()));
                //if (!isMember)
                //	return new ReturnData
                //	{
                //		Success = false,
                //		Message = "Sorry, You details could not be found"
                //	};
                var member = db.CUBs.FirstOrDefault(m => m.AccNo.ToUpper().Equals(transaction.SNo.ToUpper()));
                if (member == null)
                    return new ReturnData
                    {
                        Success = false,
                        Message = "Sorry, account number not found"
                    };

                var debtQuery = $"SELECT ISNULL(Sum(Amount),0) FROM CUSTOMERBALANCE WHERE AccNO = '{transaction.SNo}' AND cash = 1 AND transtype='DR' AND TransDescription != 'Cheque Dep(uncleared)' AND TransDescription != 'Bounced Cheque'";
				var debts = db.Database.SqlQuery<decimal>(debtQuery).FirstOrDefault();
				var creditQuery = $"SELECT ISNULL(Sum(Amount),0) FROM CUSTOMERBALANCE WHERE AccNO = '{transaction.SNo}' AND cash = 1 AND transtype='CR' AND TransDescription != 'Cheque Dep(uncleared)'";
				var credits = db.Database.SqlQuery<decimal>(creditQuery).FirstOrDefault();

				var balance = credits - debts;
                db.Messages.Add(new Message
                {
                    AccNo = member.AccNo,
                    Source = transaction.AuditId,
                    Telephone = member.Phone,
                    Processed = false,
                    AlertType = "AgencyBalance",
                    Charged = false,
                    MsgType = "balance",
                    DateReceived = DateTime.UtcNow.Date,
                    Content = $"Dear Member,your balance of  your account number {transaction.SNo} is {balance} ."

                });

                db.SaveChanges();
                return new ReturnData
				{
					Success = true,
					Message = $"{balance}",
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

		[Route("applyAdvance")]
		public ReturnData ApplyAdvance([FromBody] Transaction transaction)
		{
			var response = AdvanceService(transaction);
			return response;
		}

		[Route("fetchAdvanceProducts")]
		public List<AdvanceProduct> FetchAdvanceProducts([FromBody] Transaction transaction)
		{
			try
			{
				transaction.AccountNo = transaction.AccountNo ?? "";
				var productDescriptionQuery = $"Select Distinct  I.ProductID, P.Description From INCOME I Inner Join DEDUCTIONLIST P On P.Recoverfrom=I.ProductID  " +
					$"INNER Join PRODUCTSETUP S on S.ProductID=I.ProductID Where AccNo = '{transaction.AccountNo}' and p.DedCode <>'020' and  p.Recoverfrom " +
					$"not in (select RecoverFrom from DEDUCTION where AccNo='{transaction.AccountNo}' and Arrears+AmountCF+AmountIntCF>1) AND DATEDiff(dd,I.Transdate,GETDATE())<=S.intervals " +
					$"AND P.Mobile=1 ";

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
				var accountsQuery =$"select AgencyName from PosAgents";
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
				accounts = db.MEMBERS.Where(m => m.IDNo == transaction.SNo).Select(m => m.AccNo).ToList();
				return accounts;
			}
			catch (Exception ex)
			{
				return accounts;
			}
		}
       


		private ReturnData AdvanceService(Transaction transaction)
		{
			try
			{
                var Accmeber = $"Select MemberNo from CUB  where AccNo='{transaction.AccountNo}'";
                var Membeno = db.Database.SqlQuery<string>(Accmeber).FirstOrDefault();


                var member = db.CUBs.FirstOrDefault(m => m.AccNo.ToUpper().Equals(transaction.AccountNo.ToUpper()));
                if (member == null)
                    return new ReturnData
                    {
                        Success = false,
                        Message = "Sorry, Member number not found"
                    };



                var productDetailsQuery = $"SELECT * FROM DEDUCTIONLIST d INNER JOIN INCOME i ON i.ProductID = d.Recoverfrom WHERE d.Description = '{transaction.ProductDescription}' AND i.AccNo = '{transaction.AccountNo}'";
				var productDetails = db.Database.SqlQuery<AdvanceProduct>(productDetailsQuery).FirstOrDefault();

				var incomeQuery = $"SELECT Amount FROM INCOME WHERE AccNo = '{transaction.AccountNo}' AND Period > (SELECT DATEADD(month, -3, GETDATE())) AND ProductID = '{productDetails.ProductID}'";
				var threeMonthsIncome = db.Database.SqlQuery<decimal>(incomeQuery).Sum();
				var averageIncome = 0m;
                if (threeMonthsIncome > 0)
                    averageIncome = threeMonthsIncome / 3;
                else
                    averageIncome = 0;

                //var advanceBalQuery = $"SELECT Amount FROM DEDUCTION WHERE Amount > 0 AND ProductID = '{productDetails.ProductID}'";
                var advanceBalQuery = $"Select (AmountCF)+(AmountIntCF)+(Arrears) From DEDUCTION Where AccNo='{transaction.AccountNo}' AND RecoverFrom='{productDetails.ProductID}'";
                var threeMonthsAdvanceBal = db.Database.SqlQuery<decimal>(advanceBalQuery).Sum();
				var averageAdvanceBal = 0m;
                if (threeMonthsAdvanceBal > 0)
                    averageAdvanceBal = threeMonthsAdvanceBal / 3;
                else
                    averageAdvanceBal = 0;

                //var advanceArreaersQuery = $"SELECT Arrears FROM DEDUCTION WHERE Amount > 0 AND ProductID = '{productDetails.ProductID}'";
                var advanceArreaersQuery = $"Select Arrears From DEDUCTION Where AccNo='{transaction.AccountNo}' AND RecoverFrom<>'{productDetails.ProductID}' AND Arrears > 0";
                var threeMonthsAdvanceArrears = db.Database.SqlQuery<decimal>(advanceArreaersQuery).Sum();
				var averageAdvanceArrears = 0m;
                if (threeMonthsAdvanceArrears > 0)
                    averageAdvanceArrears = threeMonthsAdvanceArrears / 3;
                else
                    averageAdvanceArrears = 0;

                //var repayRateQuery = $"SELECT RepayRate FROM LOANBAL WHERE ACCNO = '{transaction.AccountNo}'";
                var repayRateQuery = $"Select RepayRate From LOANBAL L Inner Join MEMBERDEDUCTIONS D On D.STONo=L.LoanNo Where L.MemberNo='{Membeno}' AND D.ProductID='{productDetails.ProductID}' AND L.Balance+L.IntBalance>10";
                var repayRate = db.Database.SqlQuery<decimal>(repayRateQuery).Sum();
                var totalrepayrate = 0m;
                if (repayRate > 0)
                    totalrepayrate = repayRate;
                else
                    totalrepayrate = 0;

                //var loanArrearsQuery = $"SELECT arrears FROM LOANARREARS WHERE AccNo = '{transaction.AccountNo}'";
                var loanArrearsQuery = $"SELECT Arrears From LOANARREARS Where AccNo='{transaction.AccountNo}' and Arrears >0";
                var loanArrears = db.Database.SqlQuery<decimal>(loanArrearsQuery).Sum();
                var totalloanarrears = 0m;
                if (loanArrears > 0)
                    totalloanarrears = loanArrears;
                else
                    totalloanarrears = 0;


                var advance = averageIncome - averageAdvanceBal - averageAdvanceArrears - totalrepayrate - totalloanarrears;
                //var Recadvance = 0m;
                if (productDetails.ProductID =="001" || productDetails.ProductID == "077" && advance > 0)
                      advance = 3*(advance - 100);
                 else
                    advance = 0;
                
                    if (transaction.Amount < 200)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, minimum advance amount is KES 200"
					};

				if (transaction.Amount > advance)
					return new ReturnData
					{
						Success = false,
						Message = $"Sorry, your maximum advance amount is KES {advance}"
					};
                //var query = $"select TOP 1 serialno from advance order by serialno desc";
                //var bal = db.Database.SqlQuery<Int64>(query).FirstOrDefault();


                var seri =transaction.MachineID+1;


                var pe = 7;
                var pd = 3;
              	db.Advances.Add(new Advance
				{
					accno = transaction.AccountNo,
					appamnt = transaction.Amount,
					advdate = DateTime.UtcNow.Date,
					auditid = transaction.AuditId,
					serialno = seri,
					description = productDetails.Description,
                    payrollno=member.Payno,
                    name=member.Name,
                    ProductID=productDetails.ProductID,
                    custno=member.Payno,
                    amntapp=transaction.Amount,
                    IntRate= pe,
                    period=pd,
					audittime = DateTime.UtcNow.AddHours(3)
				});

				db.SaveChanges();
                db.Messages.Add(new Message
                {
                    AccNo = transaction.AccountNo,
                    Source = transaction.AuditId,
                    Telephone = member.Phone,
                    Processed = false,
                    AlertType = "AgencyAdvance",
                    Charged = false,
                    MsgType = "Outbox",
                    DateReceived = DateTime.UtcNow.Date,
                    Content = $"Dear Member,your request for advance of KES {transaction.Amount} to your account number {transaction.AccountNo} was successful."

                });

                db.SaveChanges();
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
					Message = "Sorry,Your request was declined beacuse you have a running loan facility"
				};
			}
		}

		[Route("sychTransactions")]
		public ReturnData SychTransactions([FromBody] List<Transaction> transactions)
		{
			var response = new ReturnData();
			foreach(var transaction in transactions)
			{
				if (transaction.Operation.ToLower().Equals("deposit"))
					response = DepositService(transaction);

				if (transaction.Operation.ToLower().Equals("withdraw"))
					response = WithdrawalService(transaction);

				if (transaction.Operation.ToLower().Equals("advance"))
					response = AdvanceService(transaction);
				
				if (!response.Success)
					return response;
			}

			return response;
		}
	}

}
