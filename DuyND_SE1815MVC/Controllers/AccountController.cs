using DuyND_SE1815_Data.Entities;
using DuyND_SE1815_Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DuyND_SE1815MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: Account/Index
        public async Task<IActionResult> Index()
        {
            var accounts = await _accountService.GetAllAccounts();
            return View(accounts);
        }

        // GET: Account/Details/5
        public async Task<IActionResult> Details(short id)
        {
            var account = await _accountService.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // GET: Account/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SystemAccount account)
        {
            if (!ModelState.IsValid)
            {
                return View(account);
            }

            await _accountService.AddAccount(account);
            return RedirectToAction(nameof(Index));
        }

        // GET: Account/Edit/5
        public async Task<IActionResult> Edit(short id)
        {
            var account = await _accountService.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Account/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(SystemAccount account)
        {
            if (ModelState.IsValid)
            {
                int? currentRole = await _accountService.GetRoleById(account.AccountId);
                Console.WriteLine($"Current Role: {currentRole}");

                int? newRole = account.AccountRole;

                if (currentRole == 0 && newRole != 0)
                {
                    ModelState.AddModelError("", "Không thể thay đổi vai trò của Admin.");
                    return View(account);
                }

                if ((currentRole == 1 || currentRole == 2) && newRole == 0)
                {
                    ModelState.AddModelError("", "Bạn không có quyền cấp quyền Admin.");
                    return View(account);
                }

                await _accountService.UpdateAccount(account);
                return RedirectToAction(nameof(Index));
            }

            return View(account);
        }
        public ActionResult EditProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var account = _accountService.GetAccountById((short)userId);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }
        public async Task<IActionResult> UpdateProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var account = await _accountService.GetAccountById((short)userId.Value);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(SystemAccount account)
        {
            var accountId = HttpContext.Session.GetInt32("UserId");
            if (accountId == null || account.AccountId != accountId.Value)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(account);
            }

            await _accountService.UpdateAccount(account); 

            return RedirectToAction("Index","Category");
        }





        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(short id)
        {
            var account = await _accountService.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Account/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(short id)
        {
            try
            {
                await _accountService.DeleteAccount(id);
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return View("CannotDelete");
            }

            return RedirectToAction("Index");
        }

        // GET: Tìm kiếm tài khoản
        [HttpGet]
        public async Task<IActionResult> SearchAccounts(string key)
        {
            var accounts = await _accountService.SystemAccounts(key);

            if (accounts == null || !accounts.Any())
            {
                ModelState.AddModelError("", "Không tìm thấy tài khoản nào!");
                return View("SearchAccounts", new List<SystemAccount>());
            }

            return View("SearchAccounts", accounts);
        }
    }
}
