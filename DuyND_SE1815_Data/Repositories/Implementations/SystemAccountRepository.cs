using DuyND_SE1815_Data.Entities;
using DuyND_SE1815_Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<SystemAccount?> GetByEmailAndPassword(string email, string password)
        {
            return await _context.SystemAccounts
                .FirstOrDefaultAsync(u => u.AccountEmail == email && u.AccountPassword == password);
        }

        public async Task<SystemAccount?> GetById(int id)
        {
            return await _context.SystemAccounts.FindAsync(id);
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

        public async Task UpdateAccount(SystemAccount account)
        {
            _context.SystemAccounts.Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAccount(int id)
        {
            var account = await _context.SystemAccounts.FindAsync(id);
            if (account != null)
            {
                _context.SystemAccounts.Remove(account);
                await _context.SaveChangesAsync();
            }
        }
    }
}
