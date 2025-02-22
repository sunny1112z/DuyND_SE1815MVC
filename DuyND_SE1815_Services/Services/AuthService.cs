using DuyND_SE1815_Data.Entities;
using DuyND_SE1815_Data.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace DuyND_SE1815_Services.Services
{
    public class AuthService
    {
        private readonly ISystemAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;

        public AuthService(ISystemAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        public async Task<SystemAccount?> AuthenticateUser(string email, string password)
        {
            
            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];

            // Kiểm tra nếu là tài khoản admin mặc định
            if (email == adminEmail && password == adminPassword)
            {
                return new SystemAccount
                {
                    AccountId = 0,
                    AccountEmail = adminEmail,
                    AccountPassword = adminPassword,
                    AccountRole = 0,
                    AccountName = "Administrator"
                };
            }

            
            return await _accountRepository.GetByEmailAndPassword(email, password);
        }
    }
}
