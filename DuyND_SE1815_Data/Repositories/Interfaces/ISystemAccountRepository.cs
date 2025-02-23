using DuyND_SE1815_Data.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DuyND_SE1815_Data.Repositories.Interfaces
{
    public interface ISystemAccountRepository
    {
        Task<SystemAccount?> GetByEmailAndPassword(string email, string password);
        Task<SystemAccount?> GetById(short id);
        Task<int?> GetIsActiveByEmail(string email);
        Task<int?> GetRoleById(short id);
        Task<List<SystemAccount>> GetAllAccounts();
        Task AddAccount(SystemAccount account);
        Task UpdateAccount(SystemAccount account);
        Task DeleteAccount(short id);
    }
}
