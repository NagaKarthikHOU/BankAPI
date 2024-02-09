using BankAPI.Interfaces;
using BankAPI.Repository;
using Dapper;

namespace BankAPI.Services
{
    public class BankService : IBankService
    {
        private readonly BankRepository _bankRepository;
        public BankService(BankRepository bankRepository)
        {
            _bankRepository = bankRepository;
        }

        public async Task<int> CreateBank(string bankId, string bankName)
        {
            return await _bankRepository.CreateBank(bankId, bankName);
        }
        public async Task<IEnumerable<Bank>> GetAllBanks()
        {
            return await _bankRepository.GetAllBanks();
        }

        public async Task<Bank> GetBank(string bankId)
        {
            return await _bankRepository.GetBank(bankId);
        }
    }
}
