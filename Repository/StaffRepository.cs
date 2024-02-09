using System.Data.SqlClient;
using System.Security.Principal;
using Dapper;

namespace BankAPI.Repository
{
    public class StaffRepository
    {
        private readonly string _connectionString;

        public StaffRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<int> CreateBankStaff(string id, string name, string password, string bankId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync("Insert Into Staff(StaffId,Name,Password,BankId) values (@StaffId,@Name,@Password,@BankId)",
                    new { StaffId = id, Name = name, Password = password, BankId = bankId });
        }
        public async Task<IEnumerable<Staff>> GetAllStaff()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Staff>("Select * From Staff");
        }

        public async Task<Staff> GetStaff(string staffId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstAsync<Staff>("Select * from Staff where StaffId=@StaffId", new { StaffId = staffId });
        }

        public async Task<int> CreateAccount(string accountId, string accountHolderName, string password, decimal balance, string bankId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync("Insert Into Account(AccountId,AccountHolderName,Password,Balance,BankId) values (@AccountId,@AccountHolderName,@Password,@Balance,@BankId)",
                    new { AccountId = accountId, AccountHolderName = accountHolderName, Password = password, Balance = balance, BankId = bankId });
        }

        public async Task<int> DeleteAccount(string accountId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync("Delete From Account where AccountId = @AccountId", new { AccountId = accountId });
        }

        public async Task<int> ResetPassword(string accountId, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync("Update Account Set Password = @Password where AccountId = @AccountId", new { AccountId = accountId, Password = password });
        }
        public async Task<int> AddCurrency(string currencyCode, decimal exchangeRate)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync("Insert into Currency(CurrencyCode,ExchangeRate) values (@CurrencyCode,@ExchangeRate)", new { CurrencyCode = currencyCode, ExchangeRate = exchangeRate });
        }
    }
}
