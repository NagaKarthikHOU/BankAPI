namespace BankAPI.Interfaces
{
    public interface IStaffService
    {
        Task<int> CreateStaff(Staff staff);
        Task<Staff> GetStaff(string staffId);
        Task<IEnumerable<Staff>> GetAllStaff();
        Task<int> AddCurrency(string currencyCode, decimal exchangeRate);
        Task<int> CreateAccount(Account account);
        Task<int> DeleteAccount(string accountId);
        Task<int> ResetPassword(string accountId, string password);
    }
}
