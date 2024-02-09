using BankAPI.Interfaces;
using BankAPI.Repository;
using System.Security.Principal;
using System.Xml.Linq;

namespace BankAPI.Services
{
    public class StaffService:IStaffService
    {
        private readonly StaffRepository _staffRepository;
        private readonly BankRepository _bankRepository;

        public StaffService(StaffRepository staffRepository,BankRepository bankRepository)
        {
            _staffRepository = staffRepository;
            _bankRepository = bankRepository;
        }

        public async Task<int> CreateStaff(Staff staff)
        {
            var bankName = await _bankRepository.GetBankName(staff.BankId);
            var staffId = bankName.Substring(0, 3) + staff.Name.Substring(0, 3) + DateTime.Now.ToString("ddMMyyyy");
            return await _staffRepository.CreateBankStaff(staffId, staff.Name, staff.Password, staff.BankId);
        }

        public async Task<Staff> GetStaff(string staffId)
        {
            return await _staffRepository.GetStaff(staffId);
        }

        public async Task<IEnumerable<Staff>> GetAllStaff()
        {
            return await _staffRepository.GetAllStaff();
        }

        public async Task<int> AddCurrency(string currencyCode, decimal exchangeRate)
        {
            return await _staffRepository.AddCurrency(currencyCode, exchangeRate);
        }
        public async Task<int> CreateAccount(Account account)
        {
            var bankName = await _bankRepository.GetBankName(account.BankId);
            var accountId = bankName.Substring(0, 3) + account.AccountHolderName.Substring(0, 3) + DateTime.Now.ToString("ddMMyyyy");
            return await _staffRepository.CreateAccount(accountId, account.AccountHolderName, account.Password, account.Balance, account.BankId);
        }

        public async Task<int> DeleteAccount(string accountId)
        {
            return await _staffRepository.DeleteAccount(accountId);
        }

        public async Task<int> ResetPassword(string accountId,string password)
        {
            return await _staffRepository.ResetPassword(accountId, password);
        }

    }
}
