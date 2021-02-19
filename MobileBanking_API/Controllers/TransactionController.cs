using MobileBanking_API.Models;
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
		TESTEntities2 db;
		public TransactionController()
		{
			db = new TESTEntities2();
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
				var transactionDescription = "EasyAgent Deposit";
				//get voucher number
				var Vno = $"Select ISNULL(max(ID),1) From Masters";
				string getvno = db.Database.SqlQuery<string>(Vno).FirstOrDefault();
				string nextvno = getvno + 1;
				//Operator name
				var OperatorName = db.PosUsers.FirstOrDefault(m => m.IDNo.ToUpper().Equals(transaction.AuditId.ToUpper()));

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
						vno = nextvno,
						AccName = member.AccountName,
						Amount = transaction.Amount,
						MachineID = transaction.MachineID,
						TransactionNo = nextvno,
						Auditid = OperatorName.Name,
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
						DocumentNo = nextvno,
						TransactionNo = nextvno,
						AuditTime = DateTime.UtcNow.AddHours(3),
						AuditID = OperatorName.Name,
						DrAccNo = floatAcc.FloatAccNo,
						CrAccNo = "942",
						TransDescript = transactionDescription,
						Source = member.MemberNo
					});

					//Agent deposit commission
					var pullFunction2 = $"Select Expense_Amount From dbo.Get_POS_Expenses ('{transaction.Amount}','Deposit')";
					decimal AgentDepositCommission = db.Database.SqlQuery<decimal>(pullFunction2).FirstOrDefault();

					db.GLTRANSACTIONS.Add(new GLTRANSACTION
					{
						TransDate = DateTime.UtcNow.Date,
						Amount = AgentDepositCommission,
						DocumentNo = nextvno,
						TransactionNo = nextvno,
						DrAccNo = "205",
						CrAccNo = floatAcc.CommissionAccNo,
						TransDescript = "EasyAgent Deposit Commission",
						AuditTime = DateTime.UtcNow.AddHours(3),
						AuditID = OperatorName.Name,
						Source = member.MemberNo
					});




					var members = db.MEMBERS.FirstOrDefault(m => m.AccNo.ToUpper().Equals(transaction.AccountNo.ToUpper()));
					if (members.MobileNo.Length > 9)
					{

						//var memberDetails = db.MEMBERS.FirstOrDefault(m => m.MemberNo.ToUpper().Equals(transaction.SNo.ToUpper()));
						db.Messages.Add(new Message
						{
							AccNo = member.AccNo,
							Source = OperatorName.Name,
							Telephone = member.Phone,
							Processed = false,
							AlertType = "EasyAgent Deposit",
							Charged = false,
							MsgType = "Outbox",
							DateReceived = DateTime.UtcNow.Date,
							Content = $"Deposit of Ksh {transaction.Amount} on {DateTime.UtcNow.Date} to account {transaction.SNo} at {floatAcc.AgencyName} successful. Reference Number{nextvno}."

						});
					}
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
				var OperatorName = db.PosUsers.FirstOrDefault(m => m.IDNo.ToUpper().Equals(transaction.AuditId.ToUpper()));
				var debtQuery = $"SELECT ISNULL(Sum(Amount),0) FROM CUSTOMERBALANCE WHERE AccNO = '{transaction.SNo}' AND cash = 1 AND transtype='DR' AND TransDescription != 'Cheque Dep(uncleared)' AND TransDescription != 'Bounced Cheque'";
				var debts = db.Database.SqlQuery<decimal>(debtQuery).FirstOrDefault();
				var creditQuery = $"SELECT ISNULL(Sum(Amount),0) FROM CUSTOMERBALANCE WHERE AccNO = '{transaction.SNo}' AND cash = 1 AND transtype='CR' AND TransDescription != 'Cheque Dep(uncleared)'";
				var credits = db.Database.SqlQuery<decimal>(creditQuery).FirstOrDefault();

				var balance = credits - debts;
				float MMbalance = 0;
				if (member.AccountName == "SAVINGS ACCOUNT")
				{
					MMbalance = 500;


					if (member.AvailableBalance < 500)
						return new ReturnData
						{
							Success = false,
							Message = "Sorry, your account must remain with a minimum of KES. 500"
						};
				}
				if (transaction.Amount < 50)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, withdrawal amount should be Ksh 50 or more"
					};
				if (transaction.Amount > (balance - 500))
				{
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, your account balance is insufficient"
					};
				}
				if (transaction.Amount > 70000)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, your cannot transact more than Ksh 70,000 at once"
					};

				//get voucher number
				var Vno = $"Select ISNULL(max(ID),1) From Masters";
				string getvno = db.Database.SqlQuery<string>(Vno).FirstOrDefault();
				string nextvno = getvno + 1;
				var floatAcc = db.PosAgents.FirstOrDefault(m => m.PosSerialNo.ToUpper().Equals(transaction.MachineID.ToUpper()));


				member.AvailableBalance -= transaction.Amount;
				var transactionDescription = "EasyAgent Withdrawal";
				var vNo = GetVoucherNo(transaction.Amount);

				db.CustomerBalances.Add(new CustomerBalance
				{
					IDNo = member.IDNo,
					PayrollNo = member.Payno,
					CustomerNo = member.Payno,
					vno = nextvno,
					AccName = member.AccountName,
					Auditid = OperatorName.Name,
					Amount = transaction.Amount,
					MachineID = transaction.MachineID,
					TransactionNo = nextvno,
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
					DocumentNo = nextvno,
					TransactionNo = nextvno,
					DrAccNo = "942",
					CrAccNo = floatAcc.FloatAccNo,
					TransDescript = transactionDescription,
					AuditTime = DateTime.UtcNow.AddHours(3),
					AuditID = OperatorName.Name,
					Source = member.MemberNo
				});

				member.AvailableBalance -= Withdrawal_Charges;
				transactionDescription = "EasyAgent Withdrawal Charge";
				db.CustomerBalances.Add(new CustomerBalance
				{
					IDNo = member.IDNo,
					PayrollNo = member.Payno,
					CustomerNo = member.Payno,
					vno = nextvno,
					AccName = member.AccountName,
					Auditid = OperatorName.Name,
					Amount = totalCommission,
					MachineID = transaction.MachineID,
					TransactionNo = nextvno,
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
					DocumentNo = nextvno,
					TransactionNo = nextvno,
					AuditTime = DateTime.UtcNow.AddHours(3),
					AuditID = OperatorName.Name,
					DrAccNo = "942",
					//temporary account
					CrAccNo = "958",
					TransDescript = transactionDescription,
					Source = member.MemberNo
				});
				//Sacco Commission
				db.GLTRANSACTIONS.Add(new GLTRANSACTION
				{
					TransDate = DateTime.UtcNow.Date,
					Amount = saccoCommission,
					DocumentNo = nextvno,
					TransactionNo = nextvno,
					AuditTime = DateTime.UtcNow.AddHours(3),
					AuditID = OperatorName.Name,
					DrAccNo = "958",
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
					DocumentNo = nextvno,
					TransactionNo = nextvno,
					AuditTime = DateTime.UtcNow.AddHours(3),
					AuditID = OperatorName.Name,
					DrAccNo = "958",
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
					vno = nextvno,
					AccName = member.AccountName,
					Amount = Excise_duty,
					MachineID = transaction.MachineID,
					Auditid = OperatorName.Name,
					TransactionNo = nextvno,
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
					DocumentNo = nextvno,
					TransactionNo = nextvno,
					AuditTime = DateTime.UtcNow.AddHours(3),
					AuditID = OperatorName.Name,
					DrAccNo = "942",
					CrAccNo = "956",
					TransDescript = transactionDescription,
					Source = member.MemberNo
				});
				var members = db.MEMBERS.FirstOrDefault(m => m.AccNo.ToUpper().Equals(transaction.AccountNo.ToUpper()));
				if (members.MobileNo.Length > 9)
				  { 
					db.SaveChanges();
				db.Messages.Add(new Message
				{
					AccNo = member.AccNo,
					Source = OperatorName.Name,
					Telephone = member.Phone,
					Processed = false,
					AlertType = "EasyAgent Withdrawal",
					Charged = false,
					MsgType = "Outbox",
					DateReceived = DateTime.UtcNow.Date,
					Content = $"Withdrawal of Ksh {transaction.Amount} on {DateTime.UtcNow.Date} from account {transaction.SNo} at {floatAcc.AgencyName} successful. Reference Number{nextvno}."

				});
			        }

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
				decimal MMbalance = 0;
				//decimal expectedCharges = 0;
				//function getSacco charges

				var pullFunction = $"Select Sacco_Charges+Agent_Charges+Excise_Duty From dbo.Get_POS_Charges ({balance})";
				decimal expectedCharges = db.Database.SqlQuery<decimal>(pullFunction).FirstOrDefault();
				if (member.AccountName == "SAVINGS ACCOUNT")
				{
					MMbalance = 500;
				}
				var withdrawableAmt = balance - MMbalance- expectedCharges;
				//withdrawableAmt = Math.Round(withdrawableAmt, 2);
				var pullFunction1 = $"Select (convert(int,{withdrawableAmt}/5)*5)-5";
				int RoundedWithdrawableAmt = db.Database.SqlQuery<int>(pullFunction1).FirstOrDefault();
				if (withdrawableAmt < 50) 
				{
					withdrawableAmt = 0;
				}
				var members = db.MEMBERS.FirstOrDefault(m => m.AccNo.ToUpper().Equals(transaction.AccountNo.ToUpper()));
				var floatAcc = db.PosAgents.FirstOrDefault(m => m.PosSerialNo.ToUpper().Equals(transaction.MachineID.ToUpper()));
				var OperatorName = db.PosUsers.FirstOrDefault(m => m.IDNo.ToUpper().Equals(transaction.AuditId.ToUpper()));
				//if (members.MobileNo.Length > 9)
				//{
				//	db.Messages.Add(new Message
				//	{
				//		AccNo = member.AccNo,
				//		Source = OperatorName.Name,
				//		Telephone = member.Phone,
				//		Processed = false,
				//		AlertType = "EasyAgent Balance Inquiry",
				//		Charged = false,
				//		MsgType = "Outbox",
				//		DateReceived = DateTime.UtcNow.Date,
				//		Content = $"Account balance for account {transaction.SNo} is Ksh {balance}. Your withdrawable amount is Ksh {RoundedWithdrawableAmt}. Agency Name {floatAcc.AgencyName}"

				//	});
				//}

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


		[Route("fetchAdvance")]
		private ReturnData AdvanceService(Transaction transaction)
		{
			try
			{
                
                var member = db.CUBs.FirstOrDefault(m => m.AccNo.ToUpper().Equals(transaction.AccountNo.ToUpper()));
                if (member == null)
                    return new ReturnData
                    {
                        Success = false,
                        Message = "Sorry, Member number not found"
                    };

				var ProductID = db.PRODUCTSETUPs.FirstOrDefault(m => m.ProductName.ToUpper().Equals(transaction.ProductDescription.ToUpper()));

				var productDetailsQuery = $"Exec dbo.Recommended_Advance ({transaction.AccountNo},{ProductID.ProductID})";
				decimal RecommAdvance = db.Database.SqlQuery<decimal>(productDetailsQuery).FirstOrDefault();

                    if (transaction.Amount < 200)
					return new ReturnData
					{
						Success = false,
						Message = "Sorry, Minimum advance amount to apply should be Ksh 200 or more"
					};

				if (transaction.Amount > RecommAdvance)
					return new ReturnData
					{
						Success = false,
						Message = $"Your maximum advance amount for {transaction.ProductDescription} is KES {RecommAdvance}"
					};
                var query = $"select TOP 1 serialno from advance where ISNUMERIC(serialno)=1 order by serialno desc";
                var advanceSerialno = db.Database.SqlQuery<string>(query).FirstOrDefault();
                 var seri = advanceSerialno + 1;
				//periods
				var OperatorName = db.PosUsers.FirstOrDefault(m => m.IDNo.ToUpper().Equals(transaction.AuditId.ToUpper()));
				var Advancedetails = db.DEDUCTIONLISTs.FirstOrDefault(m => m.Recoverfrom.ToUpper().Equals(ProductID.ProductID.ToUpper()));

              	db.Advances.Add(new Advance
				{
					accno = transaction.AccountNo,
					appamnt = transaction.Amount,
					advdate = DateTime.UtcNow.Date,
					auditid = OperatorName.Name,
					serialno = seri,
					description = transaction.ProductDescription,
                    payrollno=member.Payno,
                    name=member.Name,
                    ProductID=ProductID.ProductID,
                    custno=member.Payno,
                    amntapp=transaction.Amount,
                    IntRate= (int?)Advancedetails.InterestRate,
                    period=3,
					audittime = DateTime.UtcNow.AddHours(3)
				});
				db.DEDUCTIONs.Add(new DEDUCTION
				{
					AccNo = transaction.AccountNo,
					Amount = transaction.Amount,
					TransDate = DateTime.UtcNow.Date,
					AuditID = OperatorName.Name,
					VoucherNo = seri,
					ProductID = Advancedetails.DedCode,
					CustNo = member.MemberNo,
					AmountCF = transaction.Amount,
					AmountIntCF = ((int?)Advancedetails.InterestRate/100)*transaction.Amount,
					AmountInterest= ((int?)Advancedetails.InterestRate / 100) * transaction.Amount,
					Period = 3,
					AuditDateTime = DateTime.UtcNow.AddHours(3),
					RecoverFrom=ProductID.ProductID,
					DedCode=Advancedetails.DedCode,
					Arrears=0,
					IntDate= DateTime.UtcNow.Date.AddMonths(1),
					LastTransDate= DateTime.UtcNow.AddHours(3),
					MaturityDate= DateTime.UtcNow.AddHours(3)
				});
				db.CustomerBalances.Add(new CustomerBalance
				{
					IDNo = member.IDNo,
					PayrollNo = member.Payno,
					CustomerNo = member.Payno,
					vno = seri,
					AccName = member.AccountName,
					Amount = transaction.Amount,
					MachineID = transaction.MachineID,
					TransactionNo = seri,
					Auditid = OperatorName.Name,
					AvailableBalance = member.AvailableBalance,
					TransDescription = Advancedetails.Description,
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
					DocumentNo = seri,
					TransactionNo = seri,
					AuditTime = DateTime.UtcNow.AddHours(3),
					AuditID = OperatorName.Name,
					DrAccNo = "875",
					CrAccNo = "942",
					TransDescript = Advancedetails.Description,
					Source = member.MemberNo
				});
				var floatAcc = db.PosAgents.FirstOrDefault(m => m.PosSerialNo.ToUpper().Equals(transaction.MachineID.ToUpper()));
				var members = db.MEMBERS.FirstOrDefault(m => m.AccNo.ToUpper().Equals(transaction.AccountNo.ToUpper()));
				if (members.MobileNo.Length > 9)
				{
					db.Messages.Add(new Message
					{
						AccNo = transaction.AccountNo,
						Source = OperatorName.Name,
						Telephone = member.Phone,
						Processed = false,
						AlertType = "EasyAgent Advance",
						Charged = false,
						MsgType = "Outbox",
						DateReceived = DateTime.UtcNow.Date,
						Content = $"Request for {Advancedetails.Description} of Ksh {transaction.Amount} to your account number {transaction.AccountNo} at {floatAcc.AgencyName} was successful. Reference Number is {seri}"

					});
				}

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
					Message = "Sorry,an error occured"
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
