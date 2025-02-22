using DuyND_SE1815_Data.Entities;
using DuyND_SE1815_Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DuyND_SE1815_Services.Services
{
    public class AccountService
    {
        private readonly ISystemAccountRepository _accountRepository;

        public AccountService(ISystemAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<SystemAccount?> GetAccountById(int id)
        {
            return await _accountRepository.GetById(id);
        }

        public async Task<List<SystemAccount>> GetAllAccounts()
        {
            return await _accountRepository.GetAllAccounts();
        }

        public async Task AddAccount(SystemAccount account)
        {
            await _accountRepository.AddAccount(account);
        }

        public async Task UpdateAccount(SystemAccount account)
        {
            await _accountRepository.UpdateAccount(account);
        }

        public async Task DeleteAccount(int id)
        {
            await _accountRepository.DeleteAccount(id);
        }
    }
}
