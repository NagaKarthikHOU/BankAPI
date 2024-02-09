using System.Data.SqlClient;
using System.Security.Principal;
using Dapper;
namespace BankAPI.Repository
{
    public class BankRepository
    {
        private readonly string _connectionString;

        public BankRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public async Task<int> CreateBank(string bankId, string bankName)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync("Insert Into Bank(BankId,BankName) values (@BankId,@BankName)",
                    new { BankId = bankId, BankName = bankName });
        }
        public async Task<IEnumerable<Bank>> GetAllBanks()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Bank>("Select * From Bank");
        }
        public async Task<Bank> GetBank(string bankId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstAsync<Bank>("Select * From Bank where BankId=@BankId", new { BankId = bankId });
        }

        public async Task<string> GetBankId(string accountId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<string>("SELECT BankId FROM Account WHERE AccountId = @AccountId", new { AccountId = accountId });
        }

        public async Task<string> GetBankName(string bankId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<string>("SELECT BankName FROM Bank WHERE BankId = @BankId", new { BankId = bankId });
        }

        public async Task<decimal> GetCurrencyRate(string CurrencyCode)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<decimal>("SELECT ExchangeRate FROM Currency WHERE CurrencyCode = @CurrencyCode", new { CurrencyCode = CurrencyCode });
        }

        public async Task<decimal> ConvertCurrency(string currency, decimal amount)
        {
            decimal exchangeRate = await GetCurrencyRate(currency);
            amount *= exchangeRate;
            return amount;
        }
    }
}
