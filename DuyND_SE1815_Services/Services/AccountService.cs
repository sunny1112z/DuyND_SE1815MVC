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

        public async Task<SystemAccount?> GetAccountById(short id)
        {
            return await _accountRepository.GetById(id);
        }
        public async Task<int?> GetRoleById(short id)
        {
            return await _accountRepository.GetRoleById(id);
        }
        public async Task<List<SystemAccount>> GetAllAccounts()
        {
            return await _accountRepository.GetAllAccounts();
        }

        public async Task AddAccount(SystemAccount account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));

            var lastAccount = await _accountRepository.GetLastAccountId();   
            short lastId = lastAccount?.AccountId ?? 0;
            short newId = (short)(lastId + 1);
            account.AccountId = newId;
            try
            {
                await _accountRepository.AddAccount(account);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to add account to database: {ex.Message}");
            }
        }


        public async Task UpdateAccount(SystemAccount account)
        {
            await _accountRepository.UpdateAccount(account);
        }

        public async Task DeleteAccount(short id)
        {
            await _accountRepository.DeleteAccount(id);
        }
    }
}
