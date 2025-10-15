using Microsoft.AspNetCore.Mvc;
using VaultEdge.Application.Interfaces;

namespace VaultEdge.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount(Guid id)
        {
            var account = await _accountService.GetAccountByIdAsync(id);
            return account != null ? Ok(account) : NotFound();
        }
    }
}
