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

            try
            {
                var lastAccount = await _accountRepository.GetLastAccountId();
                short lastId = lastAccount?.AccountId ?? 0;
                short newId = (short)(lastId + 1);
                account.AccountId = newId;

                await _accountRepository.AddAccount(account);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Argument is null: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Invalid operation: {ex.Message}");
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<List<SystemAccount>> SystemAccounts(string keyword)
        {
            return await _accountRepository.SearchAccounts(keyword);
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
