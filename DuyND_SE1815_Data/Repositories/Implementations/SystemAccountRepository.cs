using DuyND_SE1815_Data.Entities;
using DuyND_SE1815_Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DuyND_SE1815_Data.Repositories.Implementations
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        private readonly FunewsManagementContext _context;

        public SystemAccountRepository(FunewsManagementContext context)
        {
            _context = context;
        }
        public async Task<List<SystemAccount>> SearchAccounts(string keyword)
        {
            return await _context.SystemAccounts
                .Where(a => a.AccountName.Contains(keyword) || a.AccountEmail.Contains(keyword))
                .ToListAsync();
        }

        public async Task<SystemAccount?> GetByEmailAndPassword(string email, string password)
        {
            return await _context.SystemAccounts
                .FirstOrDefaultAsync(u => u.AccountEmail == email && u.AccountPassword == password);
        }

        public async Task<SystemAccount?> GetById(short id)
        {
            return await _context.SystemAccounts.FindAsync(id);
        }
        public async Task<int?> GetRoleById(short id)
        {
            var account = await _context.SystemAccounts
                            .Where(a => a.AccountId == id)
                            .Select(a => a.AccountRole)
                            .FirstOrDefaultAsync();
            return account;
        }

        public async Task<int?> GetIsActiveByEmail(string email)
        {
            var account = await _context.SystemAccounts
                .Where(sa => sa.AccountEmail == email)
                .Select(sa => sa.IsActive)
                .FirstOrDefaultAsync();

            return account;
        }
        public async Task<string?> GetPasswordByEmail(string email)
        {
            var account = await _context.SystemAccounts
                .Where(sa => sa.AccountEmail == email)
                .Select(sa => sa.AccountPassword)
                .FirstOrDefaultAsync();

            return account;
        }

        public async Task<List<SystemAccount>> GetAllAccounts()
        {
            return await _context.SystemAccounts.ToListAsync();
        }

        public async Task AddAccount(SystemAccount account)
        {
            await _context.SystemAccounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }
        public async Task<SystemAccount?> GetLastAccountId()
        {
            return await _context.SystemAccounts
                .OrderByDescending(a => a.AccountId)
                .FirstOrDefaultAsync();
        }


        public async Task UpdateAccount(SystemAccount account)
        {
            _context.SystemAccounts.Update(account);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAccount(short id)
        {
              var account = await _context.SystemAccounts
                .FirstOrDefaultAsync(n => n.AccountId == id);
            if (account == null)
            {
                throw new KeyNotFoundException(" Account not found!");
            }


            if (account.NewsArticles.Any())
            {
                throw new InvalidOperationException(" Cannot delete Account because it has associated NewsArticles.");
            }


            _context.SystemAccounts.Remove(account);
            await _context.SaveChangesAsync();
        }
    }
}