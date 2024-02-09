using BankAPI.Repository;

namespace BankAPI.Interfaces
{
    public interface IBankService
    {
        Task<int> CreateBank(string bankId, string bankName);

        Task<IEnumerable<Bank>> GetAllBanks();

        Task<Bank> GetBank(string bankId);
    }
}
