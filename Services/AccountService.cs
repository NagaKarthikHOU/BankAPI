using BankAPI.Interfaces;
using BankAPI.Repository;
using Dapper;
using System.Data.SqlClient;
using System.Security.Principal;

namespace BankAPI.Services
{
    public class AccountService:IAccountService
    {
        private readonly AccountRepository _accountRepository;

        private readonly BankRepository _bankRepository;
        public AccountService(AccountRepository accountRepository,BankRepository bankRepository)
        {
            _accountRepository = accountRepository;
            _bankRepository = bankRepository;
        }

        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            return await _accountRepository.GetAllAccounts();
        }

        public async Task<Account> GetAccount(string accountId)
        {
            return await _accountRepository.GetAccount(accountId);
        }

        public async Task<int> DepositeAmount(string accountId,decimal amount,string currencyCode)
        {
            decimal balance = await _accountRepository.GetBalance(accountId);
            amount = await _bankRepository.ConvertCurrency(currencyCode, amount);
            balance += amount;
            return await _accountRepository.UpdateBalance(accountId,balance);
        }

        public async Task<int> WithdrawAmount(string accountId, decimal amount)
        {
            decimal balance = await _accountRepository.GetBalance(accountId);
            balance -= amount;
            return await _accountRepository.UpdateBalance(accountId, balance);
        }
        public async Task<(int statusCode, string transactionId)> TransferMoney(string sourceAccountId, string destinationAccountId, string serviceType, decimal amount)
        {
            var sourceAccount = await _accountRepository.GetAccount(sourceAccountId);
            var destinationAccount = await _accountRepository.GetAccount(destinationAccountId);
            if(sourceAccount is not null && destinationAccount is not null) {
                if (sourceAccount.BankId == destinationAccount.BankId)
                {
                    decimal taxAmount = await FindSameBankTaxAmount(destinationAccount.BankId, serviceType, amount);
                    if (sourceAccount.Balance + taxAmount >= amount)
                    {
                        return await PerformTransaction(sourceAccountId, destinationAccountId, amount, taxAmount);
                    }
                    else
                    {
                        return (StatusCodes.Status500InternalServerError, null);
                    }
                }
                else
                {
                    decimal taxAmount = await FindOtherBankTaxAmount(destinationAccount.BankId, serviceType, amount);
                    if (sourceAccount.Balance + taxAmount >= amount)
                    {
                        return await PerformTransaction(sourceAccountId, destinationAccountId, amount, taxAmount);
                    }
                    else
                    {
                        return (StatusCodes.Status500InternalServerError, null);
                    }
                }
            }
            else
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
        }
        public async Task<IEnumerable<TransactionHistory>> GetTransactionHistory(string accountId, DateTime startDate)
        {
            return await _accountRepository.GetTransactionHistory(accountId, startDate);
        }
        private async Task<(int statusCode,string transactionId)> PerformTransaction(string sourceAccountId, string destinationAccountId, decimal amount, decimal taxAmount)
        {
            var sourceAccount = await _accountRepository.GetAccount(sourceAccountId);
            if (sourceAccount == null)
            {
                return (StatusCodes.Status404NotFound, null);
            }
            var destinationAccount = await _accountRepository.GetAccount(destinationAccountId);
            if (destinationAccount == null)
            {
                return (StatusCodes.Status404NotFound, null);
            }
            decimal sourceBalance = sourceAccount.Balance - amount - taxAmount;
            decimal destinationBalance = destinationAccount.Balance + amount;
            int sourceUpdateSuccess = await _accountRepository.UpdateBalance(sourceAccountId, sourceBalance);
            int destinationUpdateSuccess = await _accountRepository.UpdateBalance(destinationAccountId, destinationBalance);
            if (sourceUpdateSuccess <= 0 || destinationUpdateSuccess <= 0)
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
            DateTime dateTime = DateTime.Now;
            long time = dateTime.Ticks;
            string transactionId = "TXN" + sourceAccount.BankId + sourceAccountId + time;
            int addTransactionSuccess = await _accountRepository.AddTransaction(transactionId, sourceAccountId, destinationAccountId, amount, dateTime);
            if(addTransactionSuccess <= 0)
            {
                return (StatusCodes.Status500InternalServerError, null);
            }
            return (StatusCodes.Status200OK, transactionId);
        }
        private async Task<decimal> FindOtherBankTaxAmount(string bankId, string serviceType, decimal amount)
        {
            Bank bank = await _bankRepository.GetBank(bankId);
            decimal taxAmount = 0;
            if (serviceType == "RTGS")
            {
                taxAmount = amount * bank.RTGSChargeOtherBank;
            }
            else if (serviceType == "IMPS")
            {
                taxAmount = amount * bank.IMPSChargeOtherBank;
            }
            else
            {
                Console.WriteLine("Enter Valid Service Type");
            }
            return taxAmount;
        }
        private async Task<decimal> FindSameBankTaxAmount(string bankId, string serviceType, decimal amount)
        {
            Bank bank = await _bankRepository.GetBank(bankId);
            decimal taxAmount = 0;
            if (serviceType == "RTGS")
            {
                taxAmount = amount * bank.RTGSChargeSameBank;
            }
            else if (serviceType == "IMPS")
            {
                taxAmount = amount * bank.IMPSChargeSameBank;
            }
            else
            {
                Console.WriteLine("Enter Valid Service Type");
            }
            return taxAmount;
        }
        


    }
}
