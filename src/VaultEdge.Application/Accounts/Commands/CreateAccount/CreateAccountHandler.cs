using MediatR;
using VaultEdge.Domain.Entities;
using VaultEdge.Domain.Repositories;

namespace VaultEdge.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, Guid>
    {
        private readonly IAccountRepository _accountRepository;

        public CreateAccountHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        //public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        //{
        //    var newAccount = new Account
        //    {
        //        UserId = request.UserId,
        //        AccountType = request.AccountType,
        //        Balance = 0m // New accounts start with a balance of 0
        //    };
        //    await _accountRepository.AddAsync(newAccount);
        //    await _accountRepository.SaveChangesAsync();
        //    return newAccount.Id;
        //}
    }
}
