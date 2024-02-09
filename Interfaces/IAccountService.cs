namespace BankAPI.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> GetAllAccounts();
        Task<Account> GetAccount(string accountId);
        Task<int> DepositeAmount(string accountId, decimal amount, string currencyCode);
        Task<int> WithdrawAmount(string accountId, decimal amount);
        Task<(int statusCode, string transactionId)> TransferMoney(string sourceAccountId, string destinationAccountId, string serviceType, decimal amount);
        Task<IEnumerable<TransactionHistory>> GetTransactionHistory(string accountId, DateTime startDate);
    }
}
