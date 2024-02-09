using System.Data.SqlClient;
using System.Security.Principal;
using Dapper;

namespace BankAPI.Repository
{
    public class AccountRepository
    {
        private readonly string _connectionString;

        public AccountRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Account>("Select * From Account");
        }
        public async Task<Account> GetAccount(string accountId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstAsync<Account>("Select * from Account where AccountId=@AccountId", new { AccountId = accountId });
        }
        public async Task<int> UpdateBalance(string accountId, decimal balance)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync("UPDATE Account SET Balance = @Balance WHERE AccountId = @AccountId", new { Balance = balance, AccountId = accountId });
        }
        public async Task<decimal> GetBalance(string accountId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<decimal>("SELECT Balance FROM Account WHERE AccountId = @AccountId", new { AccountId = accountId });
        }

        public async Task<int> AddTransaction(string transactionId, string sourceAccountId, string destinationAccountId, decimal amount, DateTime time)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync("INSERT INTO TransactionHistory(TransactionId, SourceAccountId, DestinationAccountId, Amount, Time) VALUES (@TransactionId, @SourceAccountId, @DestinationAccountId, @Amount, @Time)",
                        new { TransactionId = transactionId, SourceAccountId = sourceAccountId, DestinationAccountId = destinationAccountId, Amount = amount, Time = time });

        }

        public async Task<IEnumerable<TransactionHistory>> GetTransactionHistory(string accountId, DateTime startDate)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<TransactionHistory>("SELECT * FROM TransactionHistory WHERE (SourceAccountId = @SourceAccountId Or DestinationAccountId=@DestinationAccountId) AND Time >= @Time ORDER BY Time",
                                                                                     new { SourceAccountId = accountId, DestinationAccountId = accountId, Time = startDate });
        }

       
    }
}
